using Microsoft.AspNetCore.Mvc;
using TicketingAPI.Models.Auth;
using TicketingAPI.Services.Usuarios;

namespace TicketingAPI.Controllers.Usuarios
{
    [ApiController]
    [Route("api/usuarios/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(IConfiguration configuration, AuthService authService)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Auth user)
        {
            var (success, mensaje, userId) = await _authService.LoginUsuario(user.Username, user.Password);

            if (success)
            {
                // Aquí puedes generar un token JWT si se autentica correctamente
                string token = _authService.GenerarTokenJWT(userId);
                return Ok(new { success, mensaje, userId, token });
            }
            else
            {
                return BadRequest(new { success, mensaje });
            }
        }
    }
}
