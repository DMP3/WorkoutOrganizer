using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using WorkoutOrganizer.Model;
using WorkoutOrganizer.UI.Data.Repositories;
using WorkoutOrganizer.UI.View.Services;
using WorkoutOrganizer.UI.Wrapper;

namespace WorkoutOrganizer.UI.ViewModel
{
    public class EquipmentDetailViewModel : DetailViewModelBase
    {
        private IEquipmentRepository _equipmentRepository;
        private EquipmentWrapper _selectedEquipment;

        public EquipmentDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IEquipmentRepository equipmentRepository)
            : base(eventAggregator, messageDialogService)
        {
            _equipmentRepository = equipmentRepository;
            Title = "Оборудване";

            Equipments = new ObservableCollection<EquipmentWrapper>();
            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }

        public ObservableCollection<EquipmentWrapper> Equipments { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public EquipmentWrapper SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }
        public async override Task LoadAsync(int id)
        {
            Id = id;
            foreach (var wrapper in Equipments)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Equipments.Clear();
            var equipments = await _equipmentRepository.GetAllAsync();

            foreach (var model in equipments)
            {
                var wrapper = new EquipmentWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                Equipments.Add(wrapper);
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && Equipments.All(m => !m.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            try
            {
                await _equipmentRepository.SaveAsync();
                HasChanges = _equipmentRepository.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                await MessageDialogService.ShowInfoDialogAsync("Грешка при запазването. Информацията ще бъде презаредена. Детайли: " + ex.Message);
                await LoadAsync(Id);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _equipmentRepository.HasChanges();
            }
            if (e.PropertyName == nameof(EquipmentWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private bool OnRemoveCanExecute()
        {
            return SelectedEquipment != null;
        }

        private async void OnRemoveExecute()
        {
            var isReferenced = await _equipmentRepository.IsReferencedByExerciseAsync(SelectedEquipment.Id);
            if (isReferenced)
            {
                await MessageDialogService.ShowInfoDialogAsync($"{SelectedEquipment.Name} не може да бъде премахнато" +
                    $", понеже се съдържа в поне едно упражнение.");
                return;
            }

            SelectedEquipment.PropertyChanged -= Wrapper_PropertyChanged;
            _equipmentRepository.Remove(SelectedEquipment.Model);
            Equipments.Remove(SelectedEquipment);
            SelectedEquipment = null;
            HasChanges = _equipmentRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddExecute()
        {
            var wrapper = new EquipmentWrapper(new Equipment());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _equipmentRepository.Add(wrapper.Model);
            Equipments.Add(wrapper);

            //trigger the validation
            wrapper.Name = "";
        }
    }
}
