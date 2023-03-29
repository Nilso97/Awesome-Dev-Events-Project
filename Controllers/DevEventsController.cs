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

        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllDevEvents()
        {
            var devEvents = _context.DevEvents.Where(
                e => !e.IsDeleted
            ).ToList();

            return Ok(devEvents);
        }

        /// <summary>
        /// Obter um os eventos
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {
        ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "title": "string",
        ///   "description": "string",
        ///   "startDate": "2023-03-29T10:03:40.674Z",
        ///   "endDate": "2023-03-29T10:03:40.674Z",
        ///   "isDeleted": true,
        ///   "speakers": [
        ///     {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "name": "string",
        ///       "talkTitle": "string",
        ///       "talkDescription": "string",
        ///       "linkedInProfile": "string",
        ///       "devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///   ]
        /// }
        /// </remarks>
        /// <param name="devEvent">Dados do evento</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        /// {
        ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "title": "string",
        ///   "description": "string",
        ///   "startDate": "2023-03-29T10:03:40.674Z",
        ///   "endDate": "2023-03-29T10:03:40.674Z",
        ///   "isDeleted": true,
        ///   "speakers": [
        ///     {
        ///       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "name": "string",
        ///       "talkTitle": "string",
        ///       "talkDescription": "string",
        ///       "linkedInProfile": "string",
        ///       "devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///   ]
        /// }
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Não retorna nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Não retorna nada</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEvent(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(
                e => e.Id == id
            );

            if (devEvent != null)
            {
                devEvent.Delete();

                _context.SaveChanges();

                return NoContent();
            }

            return NotFound();
        }

        /// <summary>
        /// Adicionar um palestrante à um evento
        /// </summary>
        /// <remarks>
        ///         {
        ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "name": "string",
        ///   "talkTitle": "string",
        ///   "talkDescription": "string",
        ///   "linkedInProfile": "string",
        ///   "devEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        /// }
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Não retorna nada</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpPost("{id}/palestrante")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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