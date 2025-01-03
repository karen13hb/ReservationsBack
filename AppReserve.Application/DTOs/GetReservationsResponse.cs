using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Application.DTOs
{
    public class GetReservationsResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public int SpaceId { get; set; }
        public string SpaceName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
