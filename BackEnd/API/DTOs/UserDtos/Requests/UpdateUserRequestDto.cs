namespace BackEnd.DTOs.UserDtos.Requests
{
    public class UpdateUserRequestDto
    {
        public DateTime? PreviousPayDate { get; set; }
        public decimal BiWeeklySalary { get; set; }
    }
}
