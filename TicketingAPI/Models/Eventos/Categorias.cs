namespace TicketingAPI.Models.Eventos.Eventos
{
    public class Categorias
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int Usuario { get; set; }
    }
}
