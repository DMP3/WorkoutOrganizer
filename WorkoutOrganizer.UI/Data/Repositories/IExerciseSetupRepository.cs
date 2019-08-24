using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IExerciseSetupRepository : IGenericRepository<ExerciseSetup>
    {
        Task<IEnumerable<ExerciseSetup>> GetAllAsync(int workoutId);
    }
}