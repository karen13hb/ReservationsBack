using AppReserve.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate);
        Task<Reservation?> GetByIdAsync(int id);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasOverlappingReservation(int spaceId, DateTime startDate, DateTime endDate);
        Task<bool> IsUserDoubleBooking(int userId, DateTime startDate, DateTime endDate);
    }
}
