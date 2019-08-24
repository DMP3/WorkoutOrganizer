using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class ExerciseRepository : GenericRepository<Exercise, WorkoutOrganizerDbContext>, IExerciseRepository
    {
        public ExerciseRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }
        public async Task<bool> IsReferencedByExerciseSetup(int exerciseId)
        {
            return await Context.ExerciseSetups.AsNoTracking()
                .AnyAsync(ex => ex.ExerciseId == exerciseId);
        }
    }
}
