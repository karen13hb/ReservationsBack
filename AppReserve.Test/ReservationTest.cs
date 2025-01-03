using AppReserve.Domain.Interfaces.Events;
using AppReserve.Domain.Interfaces.Repositories;
using AppReserve.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using AppReserve.Application.Services;
using AppReserve.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace AppReserve.Test
{
    public class ReservationTest
    {
        private readonly Mock<IReservationRepository> _mockReservationRepository;
        private readonly Mock<ISpaceRepository> _mockSpaceRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDomainEventPublisher> _mockEventPublisher;
        private readonly ReservationService _reservationService;

        public ReservationTest()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockSpaceRepository = new Mock<ISpaceRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEventPublisher = new Mock<IDomainEventPublisher>();
            _reservationService = new ReservationService(_mockReservationRepository.Object, _mockEventPublisher.Object, _mockSpaceRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateReservation_ShouldThrowException_WhenReservationOverlaps()
        {
            // Arrange
            var existingReservation = new Reservation
            {
                Id = 1,
                SpaceId = 1,
                StartDate = new DateTime(2025, 1, 10, 9, 0, 0),
                EndDate = new DateTime(2025, 1, 10, 12, 0, 0),
                UserId = 1
            };

            var newReservationRequest = new CreateReservationRequest
            {
                UserId = 2,
                SpaceId = 1,
                StartDate = new DateTime(2025, 1, 10, 10, 0, 0),
                EndDate = new DateTime(2025, 1, 10, 11, 0, 0)
            };


            _mockReservationRepository.Setup(r => r.HasOverlappingReservation(newReservationRequest.SpaceId, newReservationRequest.StartDate, newReservationRequest.EndDate))
                .ReturnsAsync(true); // Simulamos que hay un solapamiento

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReservationAsync(newReservationRequest));

            // Assert that the exception message is as expected
            Assert.Equal("Space is already reserved for the selected time range.", exception.Message);
        }

        [Fact]
        public async Task CreateReservation_ShouldThrowException_WhenReservationDurationIsTooShort()
        {
            var space = new Space
            {
                Id = 1,
                Name = "Sala A",
                Description= "Main building, ground floor",
                MinReservationDuration = TimeSpan.FromHours(2),
                MaxReservationDuration = TimeSpan.FromHours(8)
            };

            var newReservationRequest = new CreateReservationRequest
            {
                SpaceId = space.Id,
                StartDate = new DateTime(2025, 1, 10, 9, 0, 0),
                EndDate = new DateTime(2025, 1, 10, 10, 0, 0),
                UserId = 1
            };

            _mockSpaceRepository.Setup(r => r.GetSpaceByIdAsync(space.Id)).ReturnsAsync(space);

           
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _reservationService.CreateReservationAsync(newReservationRequest));

            Assert.Equal($"La duración de la reserva debe ser al menos de {space.MinReservationDuration.TotalHours} horas.", exception.Message);
        }

        [Fact]
        public async Task CreateReservation_ShouldThrowException_WhenSpaceNotAvailable()
        {

            var space = new Space { Id = 1,
                Name = "Sala A",
                Description = "Main building, ground floor",
                MinReservationDuration = TimeSpan.FromHours(2), 
                MaxReservationDuration = TimeSpan.FromHours(8) };
            var reservationRequest = new CreateReservationRequest
            {
                SpaceId = 1,
                StartDate = new DateTime(2025, 1, 10, 9, 0, 0),
                EndDate = new DateTime(2025, 1, 10, 10, 0, 0),
                UserId = 1
            };

            _mockSpaceRepository.Setup(r => r.GetSpaceByIdAsync(space.Id)).ReturnsAsync(space);
            _mockReservationRepository.Setup(r => r.HasOverlappingReservation(reservationRequest.SpaceId, reservationRequest.StartDate, reservationRequest.EndDate))
                .ReturnsAsync(true);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReservationAsync(reservationRequest));

            Assert.Equal("Space is already reserved for the selected time range.", exception.Message);
        }

        [Fact]
        public async Task CreateReservation_ShouldReturnCreatedReservation_WhenSuccessful()
        {

            var space = new Space
            {
                Id = 1,
                Name = "Conference Room",
                MinReservationDuration = TimeSpan.FromHours(2),
                MaxReservationDuration = TimeSpan.FromHours(8)
            };

            var newReservation = new CreateReservationRequest
            {
                UserId = 1,
                SpaceId = space.Id,
                StartDate = new DateTime(2025, 1, 10, 9, 0, 0),
                EndDate = new DateTime(2025, 1, 10, 12, 0, 0)
            };

            _mockSpaceRepository.Setup(r => r.GetSpaceByIdAsync(space.Id)).ReturnsAsync(space);
            _mockReservationRepository.Setup(r => r.HasOverlappingReservation(newReservation.SpaceId, newReservation.StartDate, newReservation.EndDate))
                .ReturnsAsync(false); 
            _mockReservationRepository.Setup(r => r.CreateReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync(new Reservation
                {
                    Id = 1,
                    UserId = newReservation.UserId,
                    SpaceId = newReservation.SpaceId,
                    StartDate = newReservation.StartDate,
                    EndDate = newReservation.EndDate,
                    CreatedAt = DateTime.UtcNow
                });

            var response = await _reservationService.CreateReservationAsync(newReservation);

            Assert.NotNull(response);
            Assert.Equal(newReservation.UserId, response.UserId);
            Assert.Equal(newReservation.SpaceId, response.SpaceId);
            Assert.Equal(newReservation.StartDate, response.StartDate);
            Assert.Equal(newReservation.EndDate, response.EndDate);
        }
    }

}
