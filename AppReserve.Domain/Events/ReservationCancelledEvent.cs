using AppReserve.Domain.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Events
{
    public class ReservationCancelledEvent : IDomainEvent
    {
        public int ReservationId { get; }
        public DateTime OccurredOn { get; }

        public ReservationCancelledEvent(int reservationId)
        {
            ReservationId = reservationId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
