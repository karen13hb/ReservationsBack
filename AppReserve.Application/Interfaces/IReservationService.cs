using AppReserve.Application.DTOs;
using AppReserve.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponse> CreateReservationAsync(CreateReservationRequest request);
        Task<IEnumerable<GetReservationsResponse>> GetReservationsAsync(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate);
        Task<bool> CancelReservationAsync(int id);
    }
}
