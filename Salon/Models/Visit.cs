using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class Visit
    {
        [HiddenInput(DisplayValue = false)]
        public int VisitId { get; set; }

        [Display(Name = "Клиент")]
        public int? CustomerId { get; set; }
        [Display(Name = "Клиент")]
        public virtual Customer? Customer { get; set; }

        [Display(Name = "Услуга")]
        public int? ServiceId { get; set; }
        [Display(Name = "Клиент")]
        public virtual Service? Service { get; set; }

        [Display(Name = "Сотрудник")]
        public int? EmployeeId { get; set; }
        [Display(Name = "Сотрудник")]
        public virtual Employee? Employee { get; set; }

        [Display(Name = "Дата и время планируемого оказания услуги")]
        public DateTime? VisitDate { get; set; }
    }
}
