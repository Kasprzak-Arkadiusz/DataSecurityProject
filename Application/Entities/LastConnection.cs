namespace Application.Entities
{
    public class LastConnection
    {
        public int Id { get; set; }
        public string DeviceDetails { get; set; }
        public string Location { get; set; }

        public User User { get; set; }
    }
}