using Dev_Events_App.Entities;
using Dev_Events_App.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_Events_App.Controllers
{
    [ApiController]
    [Route("api/dev-events")]
    public class DevEventController : ControllerBase
    {
        private readonly DevEventsDbContext _context;

        public DevEventController(DevEventsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllDevEvents()
        {
            var devEvents = _context.DevEvents.Where(
                e => !e.IsDeleted
            ).ToList();

            return Ok(devEvents);
        }

        [HttpGet("{id}")]
        public IActionResult GetEventById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(d => d.Speakers)
                .SingleOrDefault(
                    e => e.Id == id
                );

            if (devEvent != null) return Ok(devEvent);

            return NotFound();
        }

        [HttpPost]
        public IActionResult SaveEvent([FromBody] DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetEventById),
                new { id = devEvent.Id },
                devEvent
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvent(Guid id, [FromBody] DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(
                e => e.Id == id
            );

            if (devEvent != null)
            {
                devEvent.Update(
                    input.Title,
                    input.Description,
                    input.StartDate,
                    input.EndDate
                );

                _context.DevEvents.Update(devEvent);
                _context.SaveChanges();

                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(e => e.Id == id);

            if (devEvent != null)
            {
                devEvent.Delete();

                _context.SaveChanges();

                return NoContent();
            }

            return NotFound();
        }

        [HttpPost("{id}/palestrante")]
        public IActionResult SaveSpeakerInEvent(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;

            var devEvent = _context.DevEvents.Any(
                d => d.Id == id
            );

            if (!devEvent)
            {
                return NotFound();
            }

            _context.DevEventsSpeakers.Add(speaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}