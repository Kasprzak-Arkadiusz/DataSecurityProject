namespace Application.Common
{
    public class LoginResponse
    {
        public Result Result { get; set; }
        public string Token { get; set; }

        public LoginResponse(Result result)
        {
            Result = result;
        }

        public LoginResponse()
        {
            
        }
    }
}