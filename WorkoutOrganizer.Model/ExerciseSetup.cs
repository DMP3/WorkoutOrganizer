using System.ComponentModel.DataAnnotations;

namespace WorkoutOrganizer.Model
{
    public class ExerciseSetup
    {
        public int Id { get; set; }

        [Required]
        public int Position { get; set; }

        [StringLength(10, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Sets { get; set; }

        [StringLength(10, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Reps { get; set; }

        [StringLength(20, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Weight { get; set; }

        [StringLength(10, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Rest { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
    }
}
