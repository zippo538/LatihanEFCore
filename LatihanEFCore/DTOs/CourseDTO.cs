
namespace LatihanEFCore.DTOs
{
    public class CourseDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Credits { get; set; }

        public DateTime Hours { get; set; }
    }
}