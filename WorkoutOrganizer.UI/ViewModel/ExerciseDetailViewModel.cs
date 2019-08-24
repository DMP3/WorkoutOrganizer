using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public class ExerciseDetailViewModel : DetailViewModelBase, IExerciseDetailViewModel
    {

        private ExerciseWrapper _exercise;
        private IExerciseRepository _exerciseRepository;
        private IMusculeGroupLookupDataService _musculeGroupLookupDataService;
        private IEquipmentLookupDataService _equipmentLookupDataService;

        public ExerciseDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IExerciseRepository exerciseRepository,
            IMusculeGroupLookupDataService musculeGroupLookupDataService,
            IEquipmentLookupDataService equipmentLookupDataService
            )
            : base(eventAggregator, messageDialogService)
        {
            _exerciseRepository = exerciseRepository;
            _musculeGroupLookupDataService = musculeGroupLookupDataService;
            _equipmentLookupDataService = equipmentLookupDataService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>()
             .Subscribe(AfterCollectionSaved);

            OpenUrlCommand = new DelegateCommand(OpenUrl);

            MusculeGroups = new ObservableCollection<LookupItem>();
            Equipments = new ObservableCollection<LookupItem>();
        }

        public ICommand OpenUrlCommand { get; }

        public async override Task LoadAsync(int exerciseId)
        {
            var exercise = exerciseId > 0
                ? await _exerciseRepository.GetByIdAsync(exerciseId)
                : CreateNewExercise();

            Id = exerciseId;
            InitializeExercise(exercise);

            await LoadMusculeGroupsLookupAsync();
            await LoadEquipmentsLookupAsync();
        }

        public ObservableCollection<LookupItem> MusculeGroups { get; }
        public ObservableCollection<LookupItem> Equipments { get; }

        public ExerciseWrapper Exercise
        {
            get { return _exercise; }
            set
            {
                _exercise = value;
                OnPropertyChanged();
            }
        }

        protected async override void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Наистина ли искате да изтриете {Exercise.Name}?", "Въпрос");
            if (result == MessageDialogResult.OK)
            {
                if (await _exerciseRepository.IsReferencedByExerciseSetup(Exercise.Id))
                {
                    result = await MessageDialogService.ShowOkCancelDialogAsync($"Упражнението {Exercise.Name} е част от поне една тренировъчна програма." +
                           $" Ако го изтриете, ще бъде премахнато от всички тренировъчни програми." +
                           $" Натиснете ОК за да го изтриете въпреки това. Натисете Cancel за да отмените изтриването.", "Информация");
                    if (result == MessageDialogResult.OK)
                    {
                        _exerciseRepository.Remove(Exercise.Model);
                        await _exerciseRepository.SaveAsync();
                        RaiseDetailDeletedEvent(Exercise.Id);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _exerciseRepository.Remove(Exercise.Model);
                    await _exerciseRepository.SaveAsync();
                    RaiseDetailDeletedEvent(Exercise.Id);
                }
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Exercise != null
              && !Exercise.HasErrors
              && HasChanges;
        }

        protected async override void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_exerciseRepository.SaveAsync,
                () =>
                {
                    HasChanges = _exerciseRepository.HasChanges();
                    Id = Exercise.Id;
                    RaiseDetailSavedEvent(Exercise.Id, Exercise.Name);
                });
        }

        private Exercise CreateNewExercise()
        {
            var exercise = new Exercise();
            _exerciseRepository.Add(exercise);
            return exercise;
        }

        private void InitializeExercise(Exercise exercise)
        {
            Exercise = new ExerciseWrapper(exercise);
            Exercise.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _exerciseRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Exercise.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Exercise.Name))
                {
                    SetTitle();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Exercise.Id == 0)
            {
                // Little trick to trigger the validation
                Exercise.Name = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Exercise.Name;
        }
        private async Task LoadMusculeGroupsLookupAsync()
        {
            MusculeGroups.Clear();
            //MusculeGroups.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _musculeGroupLookupDataService.GetMusculeGroupLookupAsync();
            foreach (var lookupItem in lookup)
            {
                MusculeGroups.Add(lookupItem);
            }
        }
        private async Task LoadEquipmentsLookupAsync()
        {
            Equipments.Clear();
            //Equipments.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _equipmentLookupDataService.GetEquipmentLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Equipments.Add(lookupItem);
            }
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(MusculeGroupDetailViewModel))
            {
                await LoadMusculeGroupsLookupAsync();
            }
            if (args.ViewModelName == nameof(EquipmentDetailViewModel))
            {
                await LoadEquipmentsLookupAsync();
            }
        }

        private void OpenUrl()
        {
            Process.Start(Exercise.DemoLink);
        }
    }
}
