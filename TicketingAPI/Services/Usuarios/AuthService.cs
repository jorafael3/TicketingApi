using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingAPI.Models.Logs;
using TicketingAPI.Services.Logs;

namespace TicketingAPI.Services.Usuarios
{
    public class AuthService
    {
        private readonly string _connectionString;
        private readonly LogService _logService;
        private readonly string _jwtSecretKey;

        public AuthService(IConfiguration configuration, LogService logService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];
            _logService = logService;
        }

        public async Task<(bool success, string mensaje, int userId)> LoginUsuario(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("US_UsuarioLogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            int success = Convert.ToInt32(reader["success"]);
                            string mensaje = reader["Mensaje"].ToString();
                            int userId = success == 1 ? Convert.ToInt32(reader["UserID"]) : 0;

                            return (success == 1, mensaje, userId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error en la tabla de logs usando LogService
                var log = new LogError
                {
                    FechaHora = DateTime.Now,
                    Procedimiento = "sp_LoginUsuario",
                    Mensaje = ex.Message,
                    Severidad = 16,
                    Estado = 1,
                    Detalle = "Error al intentar autenticar el usuario"
                };

                await _logService.RegistrarLogError(log);
                return (false, "Error interno al autenticar el usuario", 0);
            }

            return (false, "Error desconocido", 0);
        }

        public string GenerarTokenJWT(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)); // Cambia a una clave segura
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "TicketingAPI",
                audience: "TicketingAPI",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
