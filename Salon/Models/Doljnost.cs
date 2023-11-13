namespace Salon.Models
{
    public class Doljnost
    {
        public int DoljnostId { get; set; }

        public string? DoljnostName { get; set; }

        public virtual ICollection<CustomUser>? CustomUsers { get; set; }
    }
}
