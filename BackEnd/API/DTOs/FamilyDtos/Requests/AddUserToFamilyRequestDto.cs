using Domain.Enums;

namespace BackEnd.DTOs.FamilyDtos.Requests
{
    public class AddUserToFamilyRequestDto
    {
        public required string UserName { get; set; }
        public FamilyRole Role { get; set; }
    }
}
