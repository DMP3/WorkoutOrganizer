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

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, ErrorMessage = "Името трябва да е с дължина между {2} и {1} символа.", MinimumLength = 2)]
        [RegularExpression(@"^[а-яА-Яa-zA-Z''-'\s]{1,50}$", ErrorMessage = "Симовлът не е позволен")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Името трябва да е с максимална дължина- {1} символа")]
        [RegularExpression(@"^[а-яА-Яa-zA-Z''-'\s]{1,50}$", ErrorMessage = "Симовлът не е позволен")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Невалиден Email адрес")]
        [StringLength(50, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Невалиден Email адрес")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Goals { get; set; }

        [StringLength(200, ErrorMessage = "Полето е с максимална дължина- {1} символа")]
        public string Injuries { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<ClientPhoneNumber> PhoneNumbers { get; set; }
        public ICollection<Workout> Workouts { get; set; }
    }
}
