using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Service
    {
        [HiddenInput(DisplayValue = false)]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Укажите название услуги")]
        [Display(Name = "Услуга")]
        public string? ServiceName { get; set; }

        [Display(Name = "Группа услуг")]
        public int? GroupId { get; set; }
        [Display(Name = "Группа услуг")]
        public virtual Group? Group { get; set; }

        [Required(ErrorMessage = "Укажите себестоимость")]
        [Display(Name = "Себестоимость")]
        public int? ProductionCost { get; set; }

        [Required(ErrorMessage = "Укажите цену")]
        [Display(Name = "Цена")]
        public int? Price { get; set; }

        [Display(Name = "Описание услуги")]
        public string? Description { get; set; }

        public virtual ICollection<Visit>? Visit { get; set; }
    }
}
