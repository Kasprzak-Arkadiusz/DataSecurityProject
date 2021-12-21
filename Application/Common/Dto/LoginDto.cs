namespace Application.Common.Dto
{
    public record LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}