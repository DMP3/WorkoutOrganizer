using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class ExerciseSetupRepository : GenericRepository<ExerciseSetup, WorkoutOrganizerDbContext>, IExerciseSetupRepository
    {
        public ExerciseSetupRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ExerciseSetup>> GetAllAsync(int workoutId)
        {
            return await Context.Set<ExerciseSetup>()
                .Where(e => e.WorkoutId == workoutId)
                .Include(ex => ex.Exercise)
                .OrderBy(p => p.Position)
                .ToListAsync();
        }
    }
}
