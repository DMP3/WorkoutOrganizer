using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client,WorkoutOrganizerDbContext>, IClientRepository
    {
        public ClientRepository(WorkoutOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Client> GetByIdAsync(int clientId)
        {
            return await Context.Clients.Include(c => c.PhoneNumbers).SingleAsync(c => c.Id == clientId);
        }

        public void RemovePhoneNumber(ClientPhoneNumber model)
        {
            Context.ClientPhoneNumbers.Remove(model);
        }

        public async Task<bool> HasWorkoutsAsync(int clientId)
        {
            return await Context.Workouts.AsNoTracking()
                .Include(wc => wc.Clients)
                .AnyAsync(w => w.Clients.Any(c => c.Id == clientId));
        }
    }
}
