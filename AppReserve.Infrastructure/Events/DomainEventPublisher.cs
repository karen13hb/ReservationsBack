using AppReserve.Domain.Events;
using AppReserve.Domain.Interfaces.Events;
using AppReserve.Domain.Interfaces.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Infrastructure.Persistence.Events
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>().ToList();

            if (!handlers.Any())
            {
                throw new InvalidOperationException($"handler no found for {typeof(TEvent).Name}");
            }

            foreach (var handler in handlers)
            {
                await handler.Handle(domainEvent);
            }
        }
    }



}
