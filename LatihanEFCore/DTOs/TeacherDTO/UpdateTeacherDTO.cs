namespace LatihanEFCore.DTOs;

public class UpdateTeacherDto
{
    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Address { get; set; }
    public string Department { get; set; } = string.Empty;
    public string IdCourse { get; set; } = string.Empty;
}