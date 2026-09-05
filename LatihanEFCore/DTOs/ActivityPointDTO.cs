
namespace LatihanEFCore.DTO.Responses.DTOs
{
    public class ActivityPointDTO
    {
        public int IdActivityPoints { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public int Points { get; set; }
    }
}