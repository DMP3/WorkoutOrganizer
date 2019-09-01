using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class MusculeGroupDetailViewModel : DetailViewModelBase
    {
        private IMusculeGroupRepository _musculeGroupRepository;
        private MusculeGroupWrapper _selectedMusculeGroup;

        public MusculeGroupDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IMusculeGroupRepository musculeGroupRepository) 
            : base(eventAggregator, messageDialogService)
        {
            _musculeGroupRepository = musculeGroupRepository;
            Title = "Мускулни групи";
            MusculeGroups = new ObservableCollection<MusculeGroupWrapper>();

            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public ObservableCollection<MusculeGroupWrapper> MusculeGroups { get; }

        public MusculeGroupWrapper SelectedMusculeGroup
        {
            get { return _selectedMusculeGroup; }
            set {
                _selectedMusculeGroup = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

        public async override Task LoadAsync(int id)
        {
            Id = id;
            foreach (var wrapper in MusculeGroups)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            MusculeGroups.Clear();
            var musculeGroups = await _musculeGroupRepository.GetAllAsync();

            foreach (var model in musculeGroups)
            {
                var wrapper = new MusculeGroupWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                MusculeGroups.Add(wrapper);
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && MusculeGroups.All(m => !m.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            try
            {
                await _musculeGroupRepository.SaveAsync();
                HasChanges = _musculeGroupRepository.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch(Exception ex)
            {
                
                while(ex.InnerException != null)
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
                HasChanges = _musculeGroupRepository.HasChanges();
            }
            if (e.PropertyName == nameof(MusculeGroupWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private bool OnRemoveCanExecute()
        {
            return SelectedMusculeGroup != null;
        }

        private async void OnRemoveExecute()
        {
            var isReferenced = await _musculeGroupRepository.IsReferencedByExerciseAsync(SelectedMusculeGroup.Id);
            if (isReferenced)
            {
                await MessageDialogService.ShowInfoDialogAsync($"Мускулната {SelectedMusculeGroup.Name} не може да бъде премахната" +
                    $", понеже се съдържа в поне едно упражнение.");
                return;
            }

            SelectedMusculeGroup.PropertyChanged -= Wrapper_PropertyChanged;
            _musculeGroupRepository.Remove(SelectedMusculeGroup.Model);
            MusculeGroups.Remove(SelectedMusculeGroup);
            SelectedMusculeGroup = null;
            HasChanges = _musculeGroupRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddExecute()
        {
            var wrapper = new MusculeGroupWrapper(new MusculeGroup());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _musculeGroupRepository.Add(wrapper.Model);
            MusculeGroups.Add(wrapper);

            //trigger the validation
            wrapper.Name = "";
        }
    }
}
