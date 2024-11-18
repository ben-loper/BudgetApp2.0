namespace BackEnd.DTOs.UserDtos
{
    public class UpdateUserRequestDto
    {
        public DateTime? PreviousPayDate { get; set; }
        public decimal BiWeeklySalary { get; set; }
    }
}
