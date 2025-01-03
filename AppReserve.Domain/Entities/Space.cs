using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Entities
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TimeSpan MinReservationDuration { get; set; }  
        public TimeSpan MaxReservationDuration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
