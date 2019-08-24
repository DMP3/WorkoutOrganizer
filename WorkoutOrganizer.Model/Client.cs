using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutOrganizer.Model
{
    public class Client
    {
        public Client()
        {
            PhoneNumbers = new Collection<ClientPhoneNumber>();
            Workouts = new Collection<Workout>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Goals { get; set; }

        [StringLength(50)]
        public string Injuries { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<ClientPhoneNumber> PhoneNumbers { get; set; }
        public ICollection<Workout> Workouts { get; set; }
    }
}
