using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class MusculeGroupRepository : GenericRepository<MusculeGroup, WorkoutOrganizerDbContext>,
        IMusculeGroupRepository
    {
        public MusculeGroupRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }
        public async Task<bool> IsReferencedByExerciseAsync(int musculeGroupId)
        {
            return await Context.Exercises.AsNoTracking()
                .AnyAsync(e => e.MusculeGroupId == musculeGroupId);
        }
    }
}
