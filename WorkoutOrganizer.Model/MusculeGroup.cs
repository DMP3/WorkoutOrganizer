using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutOrganizer.Model
{
    public class MusculeGroup
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}
