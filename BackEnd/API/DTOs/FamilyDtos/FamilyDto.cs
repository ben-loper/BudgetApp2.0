using BackEnd.DTOs.BudgetDtos;

namespace BackEnd.DTOs.FamilyDtos
{
    public class FamilyDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required List<string> AdminUsers { get; set; }
        public required List<string> Members { get; set; }
        public BudgetDto? Budget { get; set; }
    }
}
