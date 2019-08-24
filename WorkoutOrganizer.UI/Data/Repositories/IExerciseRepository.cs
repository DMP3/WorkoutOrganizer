using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        Task<bool> IsReferencedByExerciseSetup(int exerciseId);
    }
}