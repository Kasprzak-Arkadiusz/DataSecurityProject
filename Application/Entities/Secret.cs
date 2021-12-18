namespace Application.Entities
{
    public class Secret
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Password { get; set; } // Should be stored as hash encrypted with symmetric algorithm (i.e AES-256) 

        public User User { get; set; }
    }
}