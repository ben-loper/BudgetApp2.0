namespace BackEnd.DTOs.FamilyDtos
{
    public class FamilyDto
    {
        public required string Name { get; set; }
        public required List<string> AdminUsers { get; set; }
        public required List<string> Members { get; set; }
    }
}
