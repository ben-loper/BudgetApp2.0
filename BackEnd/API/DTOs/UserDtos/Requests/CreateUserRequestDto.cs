namespace BackEnd.DTOs.UserDtos.Requests
{
    public class CreateUserRequestDto
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
