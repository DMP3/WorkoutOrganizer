using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutOrganizer.Model
{
    public class Exercise
    {
        public Exercise()
        {
            ExerciseSetups = new Collection<ExerciseSetup>();
             
        }
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [StringLength(120)]
        public string Description { get; set; }

        [Url]
        public string DemoLink { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int? MusculeGroupId { get; set; }
        public MusculeGroup MusculeGroup { get; set; }

        public int? EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        public ICollection<ExerciseSetup> ExerciseSetups { get; set; }
    }
}
