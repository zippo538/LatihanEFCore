using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Student
    {
        [Key]
        public int IdStudent { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }
        public decimal GPA { get; set; }
        public string? Address { get; set; }

        // Foreign key Organization
        [Required]
        public int IdOrganization { get; set; }

        // Navigation property Organization
        public Organization Organization { get; set; } = null!;

        // Relasi many-to-many
        public ICollection<ActivityPoints> ActivityPoints { get; set; }
            = new List<ActivityPoints>();

        // Relasi one-to-many
        public ICollection<Tuition> Tuitions { get; set; }
            = new List<Tuition>();
        public ICollection<Course> Courses { get; set; }
            = new List<Course>();
    }
}
