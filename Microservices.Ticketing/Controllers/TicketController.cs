using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace Microservices.Ticketing.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;
        public TicketController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                ticket.BookedOn = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");

                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);

                return Ok();
            }

            return BadRequest();
        }
    }
}
