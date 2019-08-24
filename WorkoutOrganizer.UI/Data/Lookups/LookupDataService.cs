using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WorkoutOrganizer.DataAccess;
using WorkoutOrganizer.Model;

namespace WorkoutOrganizer.UI.Data.Lookups
{
    public class LookupDataService : IClientLookupDataService, IWorkoutLookupDataService, IMusculeGroupLookupDataService, IExerciseLookupDataService, IEquipmentLookupDataService
    {
        private Func<WorkoutOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<WorkoutOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetClientLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Clients.AsNoTracking()
                  .Select(f =>
                  new LookupItem
                  {
                      Id = f.Id,
                      DisplayMember = f.FirstName + " " + f.LastName
                  })
                  .OrderBy(n => n.DisplayMember)
                  .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetWorkoutLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Workouts.AsNoTracking()
                  .Select(w =>
                  new LookupItem
                  {
                      Id = w.Id,
                      DisplayMember = w.Title
                  })
                  .OrderBy(n => n.DisplayMember)
                  .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetMusculeGroupLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.MusculeGroups.AsNoTracking()
                  .Select(f =>
                  new LookupItem
                  {
                      Id = f.Id,
                      DisplayMember = f.Name
                  })
                  .OrderBy(n => n.DisplayMember)
                  .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetExerciseLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Exercises.AsNoTracking()
                  .Select(f =>
                  new LookupItem
                  {
                      Id = f.Id,
                      DisplayMember = f.Name
                  })
                  .OrderBy(n => n.DisplayMember)
                  .ToListAsync();
            }
        }
        public async Task<IEnumerable<LookupItem>> GetEquipmentLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Equipments.AsNoTracking()
                    .Select(e =>
                    new LookupItem
                    {
                        Id = e.Id,
                        DisplayMember = e.Name
                    })
                    .OrderBy(n => n.DisplayMember)
                    .ToListAsync();
            }
        }
    }
}
