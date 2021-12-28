namespace ApiLibrary.Entities
{
    public class Secret
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Iv { get; set; }
        public User User { get; set; }
    }
}