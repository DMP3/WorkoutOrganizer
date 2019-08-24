using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Lookups
{
    public interface IClientLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetClientLookupAsync();
    }
}