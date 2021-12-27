namespace Application.Common.Dto
{
    public class SecretDto
    {
        public string ServiceName { get; set; }
        public byte[] Password { get; set; }
        public string UserName { get; set; }
    }
}