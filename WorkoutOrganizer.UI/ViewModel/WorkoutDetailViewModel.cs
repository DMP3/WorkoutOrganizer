using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using WorkoutOrganizer.Model;
using WorkoutOrganizer.UI.Data.Lookups;
using WorkoutOrganizer.UI.Data.Repositories;
using WorkoutOrganizer.UI.Event;
using WorkoutOrganizer.UI.View.Services;
using WorkoutOrganizer.UI.Wrapper;

namespace WorkoutOrganizer.UI.ViewModel
{
    public class WorkoutDetailViewModel : DetailViewModelBase, IWorkoutDetailViewModel
    {
        private WorkoutWrapper _workout;
        private IWorkoutRepository _workoutRepository;
        private IExerciseSetupRepository _exerciseSetupRepository;
        private IExerciseLookupDataService _exerciseLookupDataService;
        private Client _selectedAvailableClient;
        private Client _selectedAddedClient;
        private string _searchText;
        private Exercise _selectedFilteredExercise;
        private List<Client> _allClients;
        private List<Exercise> _allExercises;
        private ExerciseSetupWrapper _selectedExerciseSetup;

        public WorkoutDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IWorkoutRepository workoutRepository,
            IExerciseSetupRepository exerciseSetupRepository,
            IExerciseLookupDataService exerciseLookupDataService
            ) : base(eventAggregator, messageDialogService)
        {
            _workoutRepository = workoutRepository;
            _exerciseSetupRepository = exerciseSetupRepository;
            _exerciseLookupDataService = exerciseLookupDataService;

            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddedClients = new ObservableCollection<Client>();
            AvailableClients = new ObservableCollection<Client>();
            Exercises = new ObservableCollection<LookupItem>();
            ExerciseSetups = new ObservableCollection<ExerciseSetupWrapper>();

            AddToExerciseSetups = new DelegateCommand(OnAddToExerciseSetupsExecute, OnAddToExerciseSetupsCanExecute);

            AddClientCommand = new DelegateCommand(OnAddClientExecute, OnAddClientCanExecute);
            RemoveClientCommand = new DelegateCommand(OnRemoveClientExecute, OnRemoveClientCanExecute);
            AddExerciseSetupCommand = new DelegateCommand(OnAddExerciseSetup);
            RemoveExerciseSetupCommand = new DelegateCommand(OnRemoveExerciseSetupExecute, OnRemoveExerciseSetupCanExecute);
            MoveUp = new DelegateCommand(MoveUpExecute, MoveUpCanExecute);
            MoveDown = new DelegateCommand(MoveDownExecute, MoveDownCanExecute);
        }

        public WorkoutWrapper Workout
        {
            get { return _workout; }
            private set
            {
                _workout = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddClientCommand { get; }
        public ICommand RemoveClientCommand { get; }
        public ICommand AddExerciseSetupCommand { get; }
        public ICommand RemoveExerciseSetupCommand { get; }
        public ICommand AddToExerciseSetups { get; }
        public ICommand MoveUp { get; }
        public ICommand MoveDown { get; }
        public ObservableCollection<Client> AddedClients { get; }
        public ObservableCollection<Client> AvailableClients { get; }
        public ObservableCollection<ExerciseSetupWrapper> ExerciseSetups { get; }
        public ObservableCollection<LookupItem> Exercises { get; }
        public IEnumerable<Exercise> FilteredExercises
        {
            get
            {
                if (SearchText == null) return _allExercises;

                return _allExercises.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()));
            }
        }
        public Exercise SelectedFilteredExercise
        {
            get { return _selectedFilteredExercise; }
            set
            {
                _selectedFilteredExercise = value;
                OnPropertyChanged();
                ((DelegateCommand)AddToExerciseSetups).RaiseCanExecuteChanged();
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;

                OnPropertyChanged("SearchText");
                OnPropertyChanged("FilteredExercises");
            }
        }
        public Client SelectedAvailableClient
        {
            get { return _selectedAvailableClient; }
            set
            {
                _selectedAvailableClient = value;
                OnPropertyChanged();
                ((DelegateCommand)AddClientCommand).RaiseCanExecuteChanged();
            }
        }
        public Client SelectedAddedClient
        {
            get { return _selectedAddedClient; }
            set
            {
                _selectedAddedClient = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveClientCommand).RaiseCanExecuteChanged();
            }
        }
        public ExerciseSetupWrapper SelectedExerciseSetup
        {
            get { return _selectedExerciseSetup; }
            set
            {
                _selectedExerciseSetup = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveExerciseSetupCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)MoveUp).RaiseCanExecuteChanged();
                ((DelegateCommand)MoveDown).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int workoutId)
        {
            var workout = workoutId > 0
                ? await _workoutRepository.GetByIdAsync(workoutId)
                : CreateNewWorkout();

            Id = workoutId;

            InitializeWorkout(workout);

            _allClients = await _workoutRepository.GetAllClientsAsync();
            _allExercises = await _workoutRepository.GetAllExercisesAsync();
            SetupPickList();

            await InitializeExerciseSetups(workoutId);
        }

        protected async override void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Наистина ли искате да изтриете {Workout.Title}?", "Въпрос");
            if (result == MessageDialogResult.OK)
            {
                _workoutRepository.Remove(Workout.Model);
                await _workoutRepository.SaveAsync();
                RaiseDetailDeletedEvent(Workout.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Workout != null && !Workout.HasErrors && HasChanges && ExerciseSetups.All(ex => !ex.HasErrors);
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_workoutRepository.SaveAsync,
                () =>
                {
                    HasChanges = _workoutRepository.HasChanges();
                    Id = Workout.Id;
                    RaiseDetailSavedEvent(Workout.Id, Workout.Title);
                });
            try
            {
                await _exerciseSetupRepository.SaveAsync();
                HasChanges = _exerciseSetupRepository.HasChanges();
            }
            catch
            {
                return;
            }

        }

        private void SetupPickList()
        {
            var wokoutClientIds = Workout.Model.Clients.Select(c => c.Id).ToList();
            var addedClients = _allClients.Where(c => wokoutClientIds.Contains(c.Id)).OrderBy(c => c.FirstName);
            var availableClients = _allClients.Except(addedClients).OrderBy(c => c.FirstName);

            AddedClients.Clear();
            AvailableClients.Clear();

            foreach (var addedClient in addedClients)
            {
                AddedClients.Add(addedClient);
            }
            foreach (var availableClient in availableClients)
            {
                AvailableClients.Add(availableClient);
            }
        }

        private Workout CreateNewWorkout()
        {
            var workout = new Workout
            {
                Date = DateTime.Now
            };

            _workoutRepository.Add(workout);
            return workout;
        }

        private void InitializeWorkout(Workout workout)
        {
            Workout = new WorkoutWrapper(workout);
            Workout.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _workoutRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Workout.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Workout.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Workout.Id == 0)
            {
                Workout.Title = ""; //TO TRIGGER THE VALIDATION
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Workout.Title;
        }

        private async Task InitializeExerciseSetups(int workoutId)
        {
            foreach (var wrapper in ExerciseSetups)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            ExerciseSetups.Clear();
            var exerciseSetups = await _exerciseSetupRepository.GetAllAsync(workoutId);

            foreach (var model in exerciseSetups)
            {
                var wrapper = new ExerciseSetupWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ExerciseSetups.Add(wrapper);
            }

            await LoadExerciseLookupAsync();
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _exerciseSetupRepository.HasChanges();
            }
            if (e.PropertyName == nameof(ExerciseSetupWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadExerciseLookupAsync()
        {
            Exercises.Clear();
            //Exercises.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _exerciseLookupDataService.GetExerciseLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Exercises.Add(lookupItem);
            }
        }

        private bool OnRemoveClientCanExecute()
        {
            return SelectedAddedClient != null;
        }

        private void OnRemoveClientExecute()
        {
            var clientToRemove = SelectedAddedClient;

            Workout.Model.Clients.Remove(clientToRemove);
            AddedClients.Remove(clientToRemove);
            AvailableClients.Add(clientToRemove);
            HasChanges = _workoutRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddClientExecute()
        {
            var clientToAdd = SelectedAvailableClient;

            Workout.Model.Clients.Add(clientToAdd);
            AddedClients.Add(clientToAdd);
            AvailableClients.Remove(clientToAdd);
            HasChanges = _workoutRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddClientCanExecute()
        {
            return SelectedAvailableClient != null;
        }

        private async void OnAddExerciseSetup()
        {
            if (Id > 0)
            {
                var position = ExerciseSetups.Count + 1;

                var wrapper = new ExerciseSetupWrapper(new ExerciseSetup { WorkoutId = Id, Position = position });
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                _exerciseSetupRepository.Add(wrapper.Model);
                ExerciseSetups.Add(wrapper);
            }
            else
            {
                await MessageDialogService.ShowInfoDialogAsync("Моля натиснете бутона Запази преди да добавите нов ред.");
            }
        }

        private void OnRemoveExerciseSetupExecute()
        {
            SelectedExerciseSetup.PropertyChanged -= Wrapper_PropertyChanged;
            _exerciseSetupRepository.Remove(SelectedExerciseSetup.Model);
            ExerciseSetups.Remove(SelectedExerciseSetup);
            SelectedExerciseSetup = null;
            for (int i = 1; i <= ExerciseSetups.Count; i++)
            {
                ExerciseSetups[i - 1].Position = i;
            }
            HasChanges = _exerciseSetupRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveExerciseSetupCanExecute()
        {
            return SelectedExerciseSetup != null;
        }

        private async void OnAddToExerciseSetupsExecute()
        {
            var exerciseToAdd = SelectedFilteredExercise;
            var position = ExerciseSetups.Count + 1;
            var wrapper = new ExerciseSetupWrapper(new ExerciseSetup { WorkoutId = Id, Position = position, ExerciseId = exerciseToAdd.Id });

            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _exerciseSetupRepository.Add(wrapper.Model);
            ExerciseSetups.Add(wrapper);

            await _exerciseSetupRepository.SaveAsync();
            HasChanges = _exerciseSetupRepository.HasChanges();

            await _workoutRepository.SaveAsync();
            HasChanges = _workoutRepository.HasChanges();
            Id = Workout.Id;
            RaiseDetailSavedEvent(Workout.Id, Workout.Title);

            SelectedFilteredExercise = null;
        }

        private bool OnAddToExerciseSetupsCanExecute()
        {
            return SelectedFilteredExercise != null;
        }

        private bool MoveDownCanExecute()
        {
            return SelectedExerciseSetup != null && ExerciseSetups.Count != 1;
        }

        private bool MoveUpCanExecute()
        {
            return SelectedExerciseSetup != null && ExerciseSetups.Count != 1;
        }

        private void MoveUpExecute()
        {
            if (SelectedExerciseSetup == null)
            {
                return;
            }
            var selectedItemPosition = SelectedExerciseSetup.Position - 1;
            if (selectedItemPosition == 0)
            {
                return;
            }

            var toSwap = ExerciseSetups[selectedItemPosition];
            ExerciseSetups[selectedItemPosition] = ExerciseSetups[selectedItemPosition - 1];
            ExerciseSetups[selectedItemPosition - 1] = toSwap;

            ExerciseSetups[selectedItemPosition - 1].Position = selectedItemPosition;
            ExerciseSetups[selectedItemPosition].Position = selectedItemPosition + 1;
            SelectedExerciseSetup = ExerciseSetups[selectedItemPosition - 1];
        }

        private void MoveDownExecute()
        {
            if (SelectedExerciseSetup == null)
            {
                return;
            }
            var selectedItemPosition = SelectedExerciseSetup.Position - 1;
            if (selectedItemPosition == ExerciseSetups.Count - 1)
            {
                return;
            }

            var toSwap = ExerciseSetups[selectedItemPosition];
            ExerciseSetups[selectedItemPosition] = ExerciseSetups[selectedItemPosition + 1];
            ExerciseSetups[selectedItemPosition + 1] = toSwap;

            ExerciseSetups[selectedItemPosition + 1].Position = selectedItemPosition + 2;
            ExerciseSetups[selectedItemPosition].Position = selectedItemPosition + 1;
            SelectedExerciseSetup = ExerciseSetups[selectedItemPosition + 1];
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ClientDetailViewModel))
            {
                await _workoutRepository.ReloadClientAsync(args.Id);
                _allClients = await _workoutRepository.GetAllClientsAsync();
                SetupPickList();
            }
            if (args.ViewModelName == nameof(ExerciseDetailViewModel))
            {
                await LoadAsync(Id);
            }
        }
        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(ClientDetailViewModel))
            {
                _allClients = await _workoutRepository.GetAllClientsAsync();
                SetupPickList();
            }
            if (args.ViewModelName == nameof(ExerciseDetailViewModel))
            {
                _allExercises = await _workoutRepository.GetAllExercisesAsync();
                await LoadAsync(Id);
                var exerciseCount = ExerciseSetups.Count;
                if (exerciseCount != 0)
                {
                    for (int i = 1; i <= exerciseCount; i++)
                    {
                        ExerciseSetups[i - 1].Position = i;
                    }
                }
                await _workoutRepository.SaveAsync();
                HasChanges = _workoutRepository.HasChanges();
                Id = Workout.Id;
                RaiseDetailSavedEvent(Workout.Id, Workout.Title);
            }
        }
    }
}