using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutOrganizer.Model
{
    public class Workout
    {
        public Workout()
        {
            Clients = new Collection<Client>();
            ExerciseSetups = new Collection<ExerciseSetup>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Client> Clients { get; set; }
        public ICollection<ExerciseSetup> ExerciseSetups { get; set; }
    }
}
