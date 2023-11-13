using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Employee
    {
        [HiddenInput(DisplayValue = false)]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите отчество")]
        [Display(Name = "Отчество")]
        public string? Midname { get; set; }

        [Required(ErrorMessage = "Укажите должность")]
        [Display(Name = "Должность")]
        public int? JobId { get; set; }
        [Display(Name = "Группа услуг")]
        public virtual Job? Job { get; set; }

        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Укажите адрес")]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Дата приема на работу")]
        public DateTime? EmpDate { get; set; }

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
