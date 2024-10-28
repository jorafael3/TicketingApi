using Microsoft.AspNetCore.Mvc;
using TicketingAPI.Models.Eventos;
using TicketingAPI.Models.Eventos.Eventos;
using TicketingAPI.Services.Eventos;

namespace TicketingAPI.Controllers.Eventos
{
    [ApiController]
    [Route("api/eventos/[controller]")]
    public class EventosController : Controller
    {
        private readonly CategoriasService _categoriaService;
        private readonly EventosService _eventosService;

        public EventosController(CategoriasService categoriaService, EventosService eventosService)
        {
            _categoriaService = categoriaService;
            _eventosService = eventosService;
        }

        [HttpPost("CrearCategoria")]
        public async Task<IActionResult> CrearCategoria([FromBody] Categorias categoria)
        {
            if (categoria == null)
                return BadRequest(new { success = false, mensaje = "Los datos de la categoría no pueden ser nulos." });

            var (success, mensaje) = await _categoriaService.CrearCategorias(categoria);

            if (success)
                return Ok(new { success, mensaje });
            else
                return BadRequest(new { success, mensaje });
        }

        [HttpPost("CrearEvento")]
        public async Task<IActionResult> CrearEvento([FromBody] Eventosc evento)
        {
            var (success, mensaje) = await _eventosService.CrearEvento(evento);

            if (success)
                return Ok(new { success, mensaje });
            else
                return BadRequest(new { success, mensaje });
        }
    }
}
