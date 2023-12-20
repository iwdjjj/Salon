using Microsoft.EntityFrameworkCore;

namespace Salon.Models
{
    [Keyless]
    public class Visit_CountOtchet
    {
        public int? id { get; set; }
        public string? nm { get; set; }
        public int? kol { get; set; }
    }
}
