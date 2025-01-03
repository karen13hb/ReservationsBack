using AppReserve.Domain.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Events
{
    public class ReservationCreatedEvent : IDomainEvent
    {
        public int ReservationId { get; }
        public int UserId { get; }
        public int SpaceId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public DateTime OccurredOn { get; }

        public ReservationCreatedEvent(int reservationId, int userId, int spaceId, DateTime startDate, DateTime endDate)
        {
            ReservationId = reservationId;
            UserId = userId;
            SpaceId = spaceId;
            StartDate = startDate;
            EndDate = endDate;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
