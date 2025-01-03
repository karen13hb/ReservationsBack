using Microsoft.AspNetCore.Mvc;


using AppReserve.Domain;
using AppReserve.Application.Services;
using AppReserve.Infrastructure.Persistence.Context;
using AppReserve.Infrastructure.Persistence.Repositories;
using AppReserve.Application.Interfaces;
using AppReserve.Domain.Entities;
using AppReserve.Application.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace AppReserve.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Create([FromBody] CreateReservationRequest request)
        {
            try
            {
                var createdReservation = await _service.CreateReservationAsync(request);

                if (createdReservation == null)
                {
                    return BadRequest("Unable to create reservation.");
                }

                return Ok(createdReservation);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelReservationAsync(id);
            if (result)
            {
                return Ok(new { message = "Reservation deleted successfully" });
            }
            return BadRequest(new { error = "Failed to delete reservation" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? spaceId, [FromQuery] int? userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reservations = await _service.GetReservationsAsync(spaceId, userId, startDate, endDate);
            return Ok(reservations);
        }

    }
}
