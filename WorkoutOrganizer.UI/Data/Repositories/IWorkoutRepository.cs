using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<List<Exercise>> GetAllExercisesAsync();
        Task ReloadClientAsync(int clientId);
        Task ReloadExerciseAsync(int exerciseId);
        Task<List<ExerciseSetup>> GetAllWorkoutSetupsAsync(int workoutId);
    }
}