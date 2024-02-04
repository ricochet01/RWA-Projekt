using VideosApp.Model;

namespace VideosApp.Dto
{
    public class VideoDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int TotalSeconds { get; set; }
        public string? StreamingUrl { get; set; }
    }
}
