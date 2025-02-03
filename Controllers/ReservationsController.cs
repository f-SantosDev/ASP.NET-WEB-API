using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API.Models.DTO;
using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        // sets the DI's
        private readonly IRepository _repository;

        public ReservationsController(IRepository repository)
        {
            _repository = repository;
        }

        //***********************************************************************************************//
        //                                            Endpoints                                          //
        //***********************************************************************************************//
        //
        // Read
        [HttpGet]
        [Route("AllReservations")]
        public IActionResult GetReservations() => Ok(_repository.GetReservations);

        // Read - by id
        [HttpGet("{reservationId}")]
        public IActionResult GetReservationById([FromRoute] int reservationId)
        {
            //var searchReservation = _repository[reservationId];
            var searchReservation = _repository.GetReservationById(reservationId);

            if (searchReservation == null)
            {
                return NotFound();
            }

            return Ok(searchReservation);
        }

        // Create
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpPost]
        public IActionResult AddReservation([FromBody] Reservations reservation)
        {
            var newReservation = _repository.AddReservation(reservation);
            
            return CreatedAtAction(nameof(GetReservationById), new // call the Action GetReservationById defined in this controller
            {
                reservationId = newReservation.ReservationId // the reciver parameter must match the same name defined in the method - in this case GetReservationById
            }, newReservation);
        }

        // Update
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpPut("{reservationId}")]
        public IActionResult UpdateReservation([FromRoute] int reservationId, [FromBody] Reservations reservation)
        {
            if (reservationId != reservation.ReservationId)
            {
                return BadRequest();
            }

            var updateReservation = _repository.UpdateReservation(reservation);

            return Ok(updateReservation);
        }

        // Delete
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpDelete("{reservationId}")]
        public IActionResult DeleteReservation([FromRoute] int reservationId) 
        {
            _repository.DeleteReservation(reservationId);
            
            return NoContent();
        }
    }
}
