using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.DataAccess
{
    public class WorkoutOrganizerDbContext : DbContext
    {
        public WorkoutOrganizerDbContext():base("WorkoutOrganizerDb")
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<MusculeGroup> MusculeGroups { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ClientPhoneNumber> ClientPhoneNumbers { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<ExerciseSetup> ExerciseSetups { get; set; }

        /// <summary>
        /// Used to remove the pluralizing of table names convention
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
