using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Job
    {
        [HiddenInput(DisplayValue = false)]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Укажите название должности")]
        [Display(Name = "Должность")]
        public string? JobName { get; set; }

        [Display(Name = "Группа услуг")]
        public int? GroupId { get; set; }
        [Display(Name = "Группа услуг")]
        public virtual Group? Group { get; set; }

        [Display(Name = "График работы")]
        public string? Schedule { get; set; }

        public virtual ICollection<Employee>? Employee { get; set; }
    }
}
