using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class WorkoutRepository : GenericRepository<Workout, WorkoutOrganizerDbContext>, IWorkoutRepository
    {
        public WorkoutRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Workout> GetByIdAsync(int Id)
        {
            return await Context.Workouts
                .Include(w => w.Clients)
                .Include(e => e.ExerciseSetups)
                .SingleAsync(w => w.Id == Id);
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await Context.Set<Client>()
                .ToListAsync();
        }

        public async Task ReloadClientAsync(int clientId)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Client>()
                .SingleOrDefault(db => db.Entity.Id == clientId);
            if(dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }

        public async Task<List<ExerciseSetup>> GetAllWorkoutSetupsAsync(int workoutId)
        {
            return await Context.Set<ExerciseSetup>()
                .Where(e => e.WorkoutId == workoutId)
                .Include(ex => ex.Exercise)
                .ToListAsync();
        }
    }
}
