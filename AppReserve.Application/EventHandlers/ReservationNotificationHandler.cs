using AppReserve.Domain.Events;
using AppReserve.Domain.Interfaces.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppReserve.Application.EventHandlers
{
    public class ReservationNotificationHandler :
        IEventHandler<ReservationCreatedEvent>,
        IEventHandler<ReservationCancelledEvent>
    {
        public async Task Handle(ReservationCreatedEvent domainEvent)
        {
            Console.WriteLine($"Reserva creada: ID={domainEvent.ReservationId}, Usuario={domainEvent.UserId}, Espacio={domainEvent.SpaceId}, FechaInicio={domainEvent.StartDate}, FechaFin={domainEvent.EndDate}");
        }

        public async Task Handle(ReservationCancelledEvent domainEvent)
        {
            Console.WriteLine($"Reserva cancelada: ID={domainEvent.ReservationId}");
        }
    }
}
