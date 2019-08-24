using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        void RemovePhoneNumber(ClientPhoneNumber model);
        Task<bool> HasWorkoutsAsync(int clientId);
    }
}