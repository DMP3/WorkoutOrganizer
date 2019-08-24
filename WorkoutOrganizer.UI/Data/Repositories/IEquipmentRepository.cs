using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<bool> IsReferencedByExerciseAsync(int musculeGroupId);
    }
}