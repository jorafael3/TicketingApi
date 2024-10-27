using Microsoft.Data.SqlClient;
using System.Data;
using TicketingAPI.Models.Logs;
using TicketingAPI.Models.Usuarios;
using TicketingAPI.Services.Logs;

namespace TicketingAPI.Services.Usuarios
{
    public class UserService
    {
        private readonly string _connectionString;
        private readonly LogService _logService;


        public UserService(IConfiguration configuration, LogService logService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logService = logService;
        }

        public async Task<(bool success, string mensaje)> RegistrarUsuario(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("US_UsuarioRegistro", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@RolID", user.RolID);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Estado", user.Estado);

                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            int success = Convert.ToInt32(reader["success"]);
                            string mensaje = reader["Mensaje"].ToString();
                            return (success == 1, mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var log = new LogError
                {
                    FechaHora = DateTime.Now,
                    Procedimiento = "US_UsuarioRegistro",
                    Mensaje = ex.Message,
                    Severidad = 16,
                    Estado = 1,
                    Detalle = "Error al intentar registrar un usuario"
                };

                await _logService.RegistrarLogError(log);
                return (false, $"Error interno: {ex.Message}");
            }

            return (false, "Error al registrar el usuario");
        }

        public async Task<(bool success, string mensaje)> CrearRoles(Role role)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("US_CrearRol", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", role.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", role.Descripcion);
                        cmd.Parameters.AddWithValue("@Activo", role.Activo);
                        cmd.Parameters.AddWithValue("@UsuarioID", role.UsuarioID);

                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            int success = Convert.ToInt32(reader["success"]);
                            string mensaje = reader["Mensaje"].ToString();
                            return (success == 1, mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un mensaje de error
                return (false, $"Error interno: {ex.Message}");
            }

            return (false, "Error al crear rol");
        }

    }
}
