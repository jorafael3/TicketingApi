using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using TicketingAPI.Models.Logs;

namespace TicketingAPI.Services.Logs
{
    public class LogService
    {
        private readonly string _connectionString;

        public LogService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task RegistrarLogError(LogError log)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LogsErrores (FechaHora, Procedimiento, Mensaje, Severidad, Estado, UsuarioID, Detalle) VALUES (@FechaHora, @Procedimiento, @Mensaje, @Severidad, @Estado, @UsuarioID, @Detalle)", conn))
                    {
                        cmd.Parameters.AddWithValue("@FechaHora", log.FechaHora);
                        cmd.Parameters.AddWithValue("@Procedimiento", log.Procedimiento);
                        cmd.Parameters.AddWithValue("@Mensaje", log.Mensaje);
                        cmd.Parameters.AddWithValue("@Severidad", log.Severidad);
                        cmd.Parameters.AddWithValue("@Estado", log.Estado);
                        cmd.Parameters.AddWithValue("@UsuarioID", log.UsuarioID.HasValue ? (object)log.UsuarioID.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Detalle", log.Detalle);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el log: {ex.Message}");
            }
        }
    }
}
