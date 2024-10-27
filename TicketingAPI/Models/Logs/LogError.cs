namespace TicketingAPI.Models.Logs
{
    public class LogError
    {
        public int ID { get; set; }
        public DateTime FechaHora { get; set; }
        public string Procedimiento { get; set; }
        public string Mensaje { get; set; }
        public int Severidad { get; set; }
        public int Estado { get; set; }
        public int? UsuarioID { get; set; }
        public string Detalle { get; set; }
    }
}
