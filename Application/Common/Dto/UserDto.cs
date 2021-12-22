using Application.Entities;

namespace Application.Common.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserDto(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}