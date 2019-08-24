using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WorkoutOrganizer.UI.Data.Lookups;
using WorkoutOrganizer.UI.Event;

namespace WorkoutOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigatioViewModel
    {
        private IClientLookupDataService _clientLookupService;
        private IWorkoutLookupDataService _workoutLookupService;
        private IExerciseLookupDataService _exerciseLookupDataService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(IClientLookupDataService clientLookupService,
            IWorkoutLookupDataService workoutLookupService,
            IExerciseLookupDataService exerciseLookupDataService,
            IEventAggregator eventAggregator)
        {
            _clientLookupService = clientLookupService;
            _workoutLookupService = workoutLookupService;
            _exerciseLookupDataService = exerciseLookupDataService;
            _eventAggregator = eventAggregator;

            Clients = new ObservableCollection<NavigationItemViewModel>();
            Workouts = new ObservableCollection<NavigationItemViewModel>();
            Exercises = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }


        public async Task LoadAsync()
        {
            var lookup = await _clientLookupService.GetClientLookupAsync();
            Clients.Clear();
            foreach (var item in lookup)
            {
                Clients.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(ClientDetailViewModel)));
            }

            lookup = await _workoutLookupService.GetWorkoutLookupAsync();
            Workouts.Clear();
            foreach (var item in lookup)
            {
                Workouts.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(WorkoutDetailViewModel)));
            }

            lookup = await _exerciseLookupDataService.GetExerciseLookupAsync();
            Exercises.Clear();
            foreach (var item in lookup)
            {
                Exercises.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(ExerciseDetailViewModel)));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Clients { get; }
        public ObservableCollection<NavigationItemViewModel> Workouts { get; }
        public ObservableCollection<NavigationItemViewModel> Exercises { get; }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    AfterDetailDeleted(Clients, args);
                    break;
                case nameof(WorkoutDetailViewModel):
                    AfterDetailDeleted(Workouts, args);
                    break;
                case nameof(ExerciseDetailViewModel):
                    AfterDetailDeleted(Exercises, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(c => c.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(ClientDetailViewModel):
                    AfterDetailSaved(Clients, args);
                    break;
                case nameof(WorkoutDetailViewModel):
                    AfterDetailSaved(Workouts, args);
                    break;
                case nameof(ExerciseDetailViewModel):
                    AfterDetailSaved(Exercises, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}
