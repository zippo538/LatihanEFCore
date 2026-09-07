
namespace LatihanEFCore.DTOs
{
    public class StudentDTO
    {
        public int IdStudent { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public decimal GPA { get; set; }

        public int  IdOrganization { get; set; } 
        public int TotalActivityPoint {get;set;}

        public List<ActivityPointDto> ActivityPoints { get; set; }
            = new();

        public List<TuitionDTO> Tuitions { get; set; }
            = new();
        public List<CourseDto> Courses { get; set; }
            = new();
    }
}