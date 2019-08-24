using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task<List<Client>> GetAllClientsAsync();
        Task ReloadClientAsync(int clientId);
        Task<List<ExerciseSetup>> GetAllWorkoutSetupsAsync(int workoutId);
    }
}