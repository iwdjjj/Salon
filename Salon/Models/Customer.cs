using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Customer
    {
        [HiddenInput(DisplayValue = false)]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите отчество")]
        [Display(Name = "Отчество")]
        public string? Midname { get; set; }

        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Укажите адрес")]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "ФИО")]
        public string? FIO
        {
            get
            {
                return Surname + " " + Name + " " + Midname;
            }
        }

        public virtual ICollection<Visit>? Visit { get; set; }
    }
}
