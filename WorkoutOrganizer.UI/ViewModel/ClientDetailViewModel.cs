using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutOrganizer.Model;
using WorkoutOrganizer.UI.Data.Repositories;
using WorkoutOrganizer.UI.Event;
using WorkoutOrganizer.UI.View.Services;
using WorkoutOrganizer.UI.Wrapper;

namespace WorkoutOrganizer.UI.ViewModel
{
    public class ClientDetailViewModel : DetailViewModelBase, IClientDetailViewModel
    {
        private IClientRepository _clientRepository;
        private ClientWrapper _client;
        private ClientPhoneNumberWrapper _selectedPhoneNumber;

        public ClientDetailViewModel(IClientRepository clientRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            :base(eventAggregator, messageDialogService)
        {
            _clientRepository = clientRepository;

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            PhoneNumbers = new ObservableCollection<ClientPhoneNumberWrapper>();
        }

        public override async Task LoadAsync(int clientId)
        {
            var client = clientId > 0
                ? await _clientRepository.GetByIdAsync(clientId)
                : CreateNewClient();
            Id = clientId;
            InitializeClient(client);
            InitializeClientPhoneNumbers(client.PhoneNumbers);
        }

        private void InitializeClientPhoneNumbers(ICollection<ClientPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= ClientPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var clientPhoneNumber in phoneNumbers)
            {
                var wrapper = new ClientPhoneNumberWrapper(clientPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += ClientPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void ClientPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _clientRepository.HasChanges();
            }
            if (e.PropertyName == nameof(ClientPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public ClientWrapper Client
        {
            get { return _client; }
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        public ClientPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }
   
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<ClientPhoneNumberWrapper> PhoneNumbers { get; set; }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_clientRepository.SaveAsync,
                () =>
                {
                    HasChanges = _clientRepository.HasChanges();
                    Id = Client.Id;
                    RaiseDetailSavedEvent(Client.Id, $"{Client.FirstName} {Client.LastName}");
                });
        }

        protected override bool OnSaveCanExecute()
        {
            return Client != null
                && !Client.HasErrors
                && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            if(await _clientRepository.HasWorkoutsAsync(Client.Id))
            {
                await MessageDialogService.ShowInfoDialogAsync($"{Client.FirstName} {Client.LastName} не може да бъде изтрит," +
                    $" понеже е част от поне една тренировка.");
                return;
            }

            var result = await MessageDialogService.ShowOkCancelDialogAsync("Наистина ли искате да изтриете този клиент?", "Въпрос");
            if (result == MessageDialogResult.OK)
            {
                _clientRepository.Remove(Client.Model);
                await _clientRepository.SaveAsync();
                RaiseDetailDeletedEvent(Client.Id);
            }
        }

        private Client CreateNewClient()
        {
            var client = new Client();
            _clientRepository.Add(client);
            return client;
        }

        private void InitializeClient(Client client)
        {
            Client = new ClientWrapper(client);
            Client.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _clientRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Client.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if(e.PropertyName == nameof(Client.FirstName) || e.PropertyName == nameof(Client.LastName))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Client.Id == 0)
            {
                //To trigger the validation
                Client.FirstName = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Client.FirstName} {Client.LastName}";
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new ClientPhoneNumberWrapper(new ClientPhoneNumber());
            newNumber.PropertyChanged += ClientPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Client.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; // Trigger validation :-)
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= ClientPhoneNumberWrapper_PropertyChanged;
            _clientRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _clientRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }
    }
}
