using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using TicketingAPI.Models.Usuarios;
using TicketingAPI.Services.Usuarios;

namespace TicketingAPI.Controllers.Usuarios
{
    [ApiController]
    [Route("api/usuarios/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, UserService UserService)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _userService = UserService;
        }


        [HttpPost("RegistroUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] User user)
        {
            //{
            //   "Username": "usuarioEjemplo",
            //  "Password": "contraseñaSegura",
            //  "RolID": 1,
            //  "Email": "ejemplo@correo.com",
            //  "Estado": true
            //}
            if (user == null)
                return BadRequest(new { success = false, mensaje = "Los datos del usuario no pueden ser nulos." });

            var (success, mensaje) = await _userService.RegistrarUsuario(user);

            if (success)
                return Ok(new { success, mensaje });
            else
                return BadRequest(new { success, mensaje });
        }

        [HttpPost("CrearRoles")]
        public async Task<IActionResult> CrearRoles([FromBody] Role role)
        {
            //{
            //"Nombre": "Moderador",
            //  "Descripcion": "Gestiona contenido y usuarios",
            //  "Activo": true
            //}
            if (role == null)
                return BadRequest(new { success = false, mensaje = "Los datos del rol no pueden ser nulos." });

            var (success, mensaje) = await _userService.CrearRoles(role);

            if (success)
                return Ok(new { success, mensaje });
            else
                return BadRequest(new { success, mensaje });
        }

    }
}
