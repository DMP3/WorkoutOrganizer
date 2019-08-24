using System.ComponentModel.DataAnnotations;

namespace WorkoutOrganizer.Model
{
    public class ClientPhoneNumber
    {
        public int Id { get; set; }

        [Phone]
        [Required]
        public string Number { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
