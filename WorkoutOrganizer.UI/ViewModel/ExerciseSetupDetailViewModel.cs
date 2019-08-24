//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using Prism.Commands;
//using Prism.Events;
//using WorkoutOrganizer.UI.Data.Repositories;
//using WorkoutOrganizer.UI.View.Services;
//using WorkoutOrganizer.UI.Wrapper;

//namespace WorkoutOrganizer.UI.ViewModel
//{
//    public class ExerciseSetupDetailViewModel : DetailViewModelBase
//    {
//        private IExerciseRepository _exerciseRepository;
//        private IExerciseSetupRepository _exerciseSetupRepository;

//        public ExerciseSetupDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
//            IExerciseRepository exerciseRepository, IExerciseSetupRepository exerciseSetupRepository)
//            : base(eventAggregator, messageDialogService)
//        {
//            _exerciseRepository = exerciseRepository;
//            _exerciseSetupRepository = exerciseSetupRepository;

//            ExerciseSetups = new ObservableCollection<ExerciseSetupWrapper>();

//        }

//        public ObservableCollection<ExerciseSetupWrapper> ExerciseSetups { get; }

//        public async override Task LoadAsync(int id)
//        {
//            Id = id;
//            foreach (var wrapper in ExerciseSetups)
//            {
//                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
//            }

//            ExerciseSetups.Clear();
//            var exerciseSetups = await _exerciseSetupRepository.GetAllAsync(id);

//            foreach (var model in exerciseSetups)
//            {
//                var wrapper = new ExerciseSetupWrapper(model);
//                wrapper.PropertyChanged += Wrapper_PropertyChanged;
//                ExerciseSetups.Add(wrapper);
//            }
//        }

//        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (!HasChanges)
//            {
//                HasChanges = _exerciseSetupRepository.HasChanges();
//            }
//            if (e.PropertyName == nameof(MusculeGroupWrapper.HasErrors))
//            {
//                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
//            }
//        }

//        protected override void OnDeleteExecute()
//        {
//            throw new System.NotImplementedException();
//        }

//        protected override bool OnSaveCanExecute()
//        {
//            throw new System.NotImplementedException();
//        }

//        protected override void OnSaveExecute()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
