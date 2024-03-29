﻿using System.ComponentModel.DataAnnotations;

namespace WorkoutOrganizer.Model
{
    public class ClientPhoneNumber
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето не може да бъде празно")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?(0{1})([8|9{1}])([9|8|7]{1})([0-9]{7})(.*)$", ErrorMessage = "Невалиден телефонен номер")]
        [MaxLength(10 , ErrorMessage = "Дължината на полето не може да е повече от {1} символа")]
        public string Number { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
