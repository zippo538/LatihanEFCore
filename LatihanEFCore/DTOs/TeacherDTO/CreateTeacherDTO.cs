namespace LatihanEFCore.DTOs;

public class CreateTeacherDto
{
    
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string Department { get; set; } = string.Empty;
    public string IdCourse { get; set; } = string.Empty;
}