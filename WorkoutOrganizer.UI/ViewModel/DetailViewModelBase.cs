using Prism.Commands;
using Prism.Events;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutOrganizer.UI.Event;
using WorkoutOrganizer.UI.View.Services;

namespace WorkoutOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        private int _id;
        private string _title;
        protected readonly IMessageDialogService MessageDialogService;
        protected readonly IEventAggregator EventAggregator;

        public DetailViewModelBase(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            MessageDialogService = messageDialogService;
            EventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            protected set {
                _title = value;
                OnPropertyChanged();
            }
        }

        public abstract Task LoadAsync(int id);

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CloseDetailViewCommand { get;}
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new
             AfterDetailDeletedEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = this.GetType().Name
                });
        }

        protected async virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = await MessageDialogService.ShowOkCancelDialogAsync(
                    "Вие направихте промени. Наистина ли искате да затворите този прозорец?",
                    "Въпрос");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }
        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc, Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex) //handle the concurrency exception
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    await MessageDialogService.ShowInfoDialogAsync("Записът вече е изтрит от друг потребител.");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }

                var result = await MessageDialogService.ShowOkCancelDialogAsync("Записът вече е променен от някой друг. Натиснете OK, за да запазите вашите промени"
                    + ", натиснете Cancel за да презаредите записа от базата данни.", "Въпрос");
                if (result == MessageDialogResult.OK)
                {
                    //Update the original values with database-values
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    //Reload entity from the database
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            };
            afterSaveAction();
        }
    }
}