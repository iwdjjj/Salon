using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Group
    {
        [HiddenInput(DisplayValue = false)]
        public int GroupId { get; set; }

        //[Required(ErrorMessage = "Укажите название группы услуг")]
        [Display(Name = "Группа услуг")]
        public string? GroupName { get; set; }

        [Display(Name = "Описание группы услуг")]
        public string? Description { get; set; }

        [Display(Name = "Количество услуг")]
        public int? Services_Count { get; set; }

        public virtual ICollection<Service>? Service { get; set; }
        public virtual ICollection<Job>? Job { get; set; }
    }
}
