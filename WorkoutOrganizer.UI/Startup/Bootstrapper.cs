using Autofac;
using Prism.Events;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.UI.Data;
using WorkoutOrganizer.UI.Data.Lookups;
using WorkoutOrganizer.UI.Data.Repositories;
using WorkoutOrganizer.UI.View.Services;
using WorkoutOrganizer.UI.ViewModel;

namespace WorkoutOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WorkoutOrganizerDbContext>().AsSelf();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigatioViewModel>();

            builder.RegisterType<ClientDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(ClientDetailViewModel));
            builder.RegisterType<WorkoutDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(WorkoutDetailViewModel));
            builder.RegisterType<MusculeGroupDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(MusculeGroupDetailViewModel));
            builder.RegisterType<EquipmentDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(EquipmentDetailViewModel));
            builder.RegisterType<ExerciseDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(ExerciseDetailViewModel));
            //builder.RegisterType<ExerciseSetupDetailViewModel>()
            //    .Keyed<IDetailViewModel>(nameof(ExerciseSetupDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<ClientRepository>().As<IClientRepository>();
            builder.RegisterType<WorkoutRepository>().As<IWorkoutRepository>();
            builder.RegisterType<MusculeGroupRepository>().As<IMusculeGroupRepository>();
            builder.RegisterType<ExerciseRepository>().As<IExerciseRepository>();
            builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>();
            builder.RegisterType<ExerciseSetupRepository>().As<IExerciseSetupRepository>();

            return builder.Build();
        }
    }
}
