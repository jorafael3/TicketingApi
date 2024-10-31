using Microsoft.Data.SqlClient;
using System.Data;
using TicketingAPI.Models.Eventos;
using TicketingAPI.Models.Logs;
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
                        evento.CODIGO_EVENTO = GenerarCodigoEventoAsync();
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

                        return (true, "Evento creado con éxito");
                    }
                }
            }
            catch (Exception ex)
            {
                var log = new LogError
                {
                    FechaHora = DateTime.Now,
                    Procedimiento = "EV_CrearEvento",
                    Mensaje = ex.Message,
                    Severidad = 16,
                    Estado = 1,
                    Detalle = "Error al Crear el evento"
                };

                await _logService.RegistrarLogError(log);
                return (false, $"Error al crear el evento: {ex.Message}");
            }
        }

        public string GenerarCodigoEventoAsync()
        {
            // Genera un código de evento único, e.g., "EVT-2024-0001"
            var fechaActual = DateTime.Now.ToString("yyyyMMdd"); // Año, mes, día
            var codigoUnico = $"EVT-{fechaActual}-{Guid.NewGuid().ToString().Substring(0, 5)}";
            return codigoUnico;
        }

        public async Task<(bool Success, string Message)> GuardarImagenAsync(EventoImagen eventoImagen)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
             
                    using (var command = new SqlCommand("sp_GuardarEventoImagen", connection))
                    {
                        command.Parameters.AddWithValue("@EventoID", eventoImagen.EventoID);
                        command.Parameters.AddWithValue("@URL", eventoImagen.URL);
                        command.Parameters.AddWithValue("@Descripcion", eventoImagen.Descripcion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EsPrincipal", eventoImagen.EsPrincipal);
                        command.Parameters.AddWithValue("@Activo", eventoImagen.Activo);
                        command.Parameters.AddWithValue("@Activo", eventoImagen.UsuarioID);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return (rowsAffected > 0, "Imagen guardada exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar la imagen: {ex.Message}");
            }
        }

    }
}
