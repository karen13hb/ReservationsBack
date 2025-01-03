using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppReserve.Domain.Entities;
using AppReserve.Domain.Interfaces.Repositories;
using AppReserve.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AppReserve.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Reservation>> GetAllAsync(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Reservations.AsQueryable();

            if (spaceId.HasValue) query = query.Where(r => r.SpaceId == spaceId);
            if (userId.HasValue) query = query.Where(r => r.UserId == userId);
            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(r => r.StartDate < endDate && r.EndDate > startDate);

            return await query.ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
            => await _context.Reservations.FindAsync(id);

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasOverlappingReservation(int spaceId, DateTime startDate, DateTime endDate)
            => await _context.Reservations.AnyAsync(r =>
                r.SpaceId == spaceId && r.StartDate < endDate && r.EndDate > startDate);

        public async Task<bool> IsUserDoubleBooking(int userId, DateTime startDate, DateTime endDate)
            => await _context.Reservations.AnyAsync(r =>
                r.UserId == userId && r.StartDate < endDate && r.EndDate > startDate);
    }
}
