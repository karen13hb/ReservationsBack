using AppReserve.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppReserve.Application.Interfaces;
using AppReserve.Domain.Entities;
using AppReserve.Domain.Interfaces;
using AppReserve.Domain.Interfaces.Repositories;
using AppReserve.Domain.Interfaces.Events;
using AppReserve.Domain.Events;
using AppReserve.Application.DTOs;

namespace AppReserve.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly IDomainEventPublisher _eventPublisher;
        private readonly ISpaceRepository _spaceRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(IReservationRepository repository, IDomainEventPublisher eventPublisher, ISpaceRepository spaceRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _spaceRepository = spaceRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var success = await _repository.DeleteAsync(reservationId);

            if (success)
            {
                var domainEvent = new ReservationCancelledEvent(reservationId);
                await _eventPublisher.Publish(domainEvent);
            }

            return success;
        }

        public async Task<ReservationResponse> CreateReservationAsync(CreateReservationRequest request)
        {

            ReservationResponse response = new ReservationResponse();

            if (await _repository.HasOverlappingReservation(request.SpaceId, request.StartDate, request.EndDate))
                throw new InvalidOperationException("Space is already reserved for the selected time range.");


            if (await _repository.IsUserDoubleBooking(request.UserId, request.StartDate, request.EndDate))
                throw new InvalidOperationException("User already has a reservation during the selected time range.");

            var space = await _spaceRepository.GetSpaceByIdAsync(request.SpaceId);

            if (space == null)
            {
                throw new ArgumentException("El espacio seleccionado no existe.");
            }

            var reservationDuration = request.EndDate - request.StartDate;

            if (reservationDuration < space.MinReservationDuration)
            {
                throw new ArgumentException($"La duración de la reserva debe ser al menos de {space.MinReservationDuration.TotalHours} horas.");
            }

            var reservation = new Reservation
            {
                UserId = request.UserId,
                SpaceId = request.SpaceId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedAt = DateTime.UtcNow
            };


            var createdReservation = await _repository.CreateReservationAsync(reservation);


            if (createdReservation != null)
            {
                response = new ReservationResponse
                {
                    Id = createdReservation.Id,
                    UserId = createdReservation.UserId,
                    SpaceId = createdReservation.SpaceId,
                    StartDate = createdReservation.StartDate,
                    EndDate = createdReservation.EndDate,
                    CreatedAt = createdReservation.CreatedAt
                };
                var domainEvent = new ReservationCreatedEvent(response?.Id ?? 0, response.UserId, response.SpaceId, response.StartDate, response.EndDate);
                await _eventPublisher.Publish(domainEvent);
            }

            return response;
        }

        public async Task<IEnumerable<GetReservationsResponse>> GetReservationsAsync(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate)
        {
            var reservations = await _repository.GetAllAsync(spaceId, userId, startDate, endDate);

            var reservationDtos = reservations.Select(reservation => new GetReservationsResponse
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                UserName = _userRepository.GetUserByIdAsync(reservation.UserId).Result.Name,  
                SpaceId = reservation.SpaceId,
                SpaceName = _spaceRepository.GetSpaceByIdAsync(reservation.SpaceId).Result.Name,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                CreatedAt = reservation.CreatedAt
            });

            return reservationDtos;
        }
    }

}
