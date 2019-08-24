using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutOrganizer.Model
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Полето е с максимална дължина- {2} символа")]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}
