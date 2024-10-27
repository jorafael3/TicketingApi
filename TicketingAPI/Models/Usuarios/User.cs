namespace TicketingAPI.Models.Usuarios
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
    }
}
