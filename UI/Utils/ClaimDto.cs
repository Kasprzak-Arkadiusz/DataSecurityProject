namespace UI.Utils
{
    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public ClaimDto(string type, string value)
        {
            Type = type;
            Value = value;
        }
        public ClaimDto()
        {
            
        }
    }
}