using System.Data.Entity;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class EquipmentRepository : GenericRepository<Equipment, WorkoutOrganizerDbContext>, IEquipmentRepository
    {
        public EquipmentRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }
        public async Task<bool> IsReferencedByExerciseAsync(int equipmentId)
        {
            return await Context.Exercises.AsNoTracking()
                .AnyAsync(e => e.EquipmentId == equipmentId);
        }
    }
}
