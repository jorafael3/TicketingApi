using Microsoft.Data.SqlClient;
using System.Data;
using TicketingAPI.Models.Eventos.Eventos;
using TicketingAPI.Models.Logs;
using TicketingAPI.Models.Usuarios;
using TicketingAPI.Services.Logs;

namespace TicketingAPI.Services.Eventos
{
    public class CategoriasService
    {
        private readonly string _connectionString;
        private readonly LogService _logService;


        public CategoriasService(IConfiguration configuration, LogService logService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logService = logService;
        }

        public async Task<(bool success, string mensaje)> CrearCategorias(Categorias user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("EV_Crear_Categorias", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", user.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", user.Descripcion);
                        cmd.Parameters.AddWithValue("@Activo", user.Activo);
                        cmd.Parameters.AddWithValue("@UsuarioID", user.Usuario);

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

    }
}
