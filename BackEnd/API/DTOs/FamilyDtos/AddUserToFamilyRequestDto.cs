using Domain.Enums;

namespace BackEnd.DTOs.FamilyDtos
{
    public class AddUserToFamilyRequestDto
    {
        public required string UserName { get; set; }
        public FamilyRole Role { get; set; }
    }
}
