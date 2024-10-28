namespace TicketingAPI.Models.Eventos
{
    public class Eventosc
    {
        public string CODIGO_EVENTO { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public string Ciudad { get; set; }
        public int CapacidadTotal { get; set; }
        public decimal? PrecioBase { get; set; }
        public string Estado { get; set; }
        public int CreadoPor { get; set; }
        public string ImagenURL { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string CategoriaID { get; set; }
        public bool EsVisible { get; set; }
        public int Usuario { get; set; }
    }
}
