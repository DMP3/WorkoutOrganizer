using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IMusculeGroupRepository : IGenericRepository<MusculeGroup>
    {
        Task<bool> IsReferencedByExerciseAsync(int musculeGroupId);
    }
}