namespace LatihanEFCore.DTOs
{
    public class UpdateStudentDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; }

        public decimal GPA { get; set; }
        public int IdOrganization { get; set; }
    }
}