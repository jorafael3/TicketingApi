namespace TicketingAPI.Models.Eventos
{
    public class EventoImagen
    {
        public Guid EventoID { get; set; } // Clave foránea al ID_UNICO de EV_Eventos
        public string URL { get; set; } // URL de la imagen
        public string Descripcion { get; set; } // Descripción de la imagen
        public bool EsPrincipal { get; set; } // Indicador si es imagen principal
        public bool Activo { get; set; } = true; // Indicador de actividad
        public int UsuarioID { get; set; } // Indicador de actividad
    }
}
