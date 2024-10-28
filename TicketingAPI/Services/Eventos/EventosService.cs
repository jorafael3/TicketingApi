using Microsoft.Data.SqlClient;
using System.Data;
using TicketingAPI.Models.Eventos;
using TicketingAPI.Services.Logs;

namespace TicketingAPI.Services.Eventos
{
    public class EventosService
    {
        private readonly string _connectionString;
        private readonly LogService _logService;


        public EventosService(IConfiguration configuration, LogService logService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logService = logService;
        }

        public async Task<(bool success, string mensaje)> CrearEvento(Eventosc evento)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("EV_CrearEvento", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CODIGO_EVENTO", evento.CODIGO_EVENTO);
                        command.Parameters.AddWithValue("@Nombre", evento.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", evento.Descripcion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Fecha", evento.Fecha);
                        command.Parameters.AddWithValue("@Lugar", evento.Lugar);
                        command.Parameters.AddWithValue("@Ciudad", evento.Ciudad);
                        command.Parameters.AddWithValue("@CapacidadTotal", evento.CapacidadTotal);
                        command.Parameters.AddWithValue("@PrecioBase", evento.PrecioBase ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Estado", evento.Estado);
                        command.Parameters.AddWithValue("@CreadoPor", evento.CreadoPor);
                        command.Parameters.AddWithValue("@ImagenURL", evento.ImagenURL ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@HoraInicio", evento.HoraInicio ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@HoraFin", evento.HoraFin ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CategoriaID", evento.CategoriaID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EsVisible", evento.EsVisible);
                        command.Parameters.AddWithValue("@UsuarioID", evento.Usuario);

                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync();
                        connection.Close();

                        return (true, $"Evento creado con éxito. ID: {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear el evento: {ex.Message}");
            }
        }
    }
}
