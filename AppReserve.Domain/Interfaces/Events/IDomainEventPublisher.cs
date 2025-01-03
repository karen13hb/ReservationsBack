using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Interfaces.Events
{
    public interface IDomainEventPublisher
    {
        Task Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    }
}
