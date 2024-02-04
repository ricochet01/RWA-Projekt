using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class VideoRepository : IVideoRepository
    {
        private readonly DataContext context;

        public VideoRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Video> GetVideos()
            => context.Videos.OrderBy(v => v.Id).ToList();

        public Video GetVideo(int id)
            => context.Videos.FirstOrDefault(v => v.Id == id);

        public Video GetVideo(string name)
            => context.Videos.FirstOrDefault(v => v.Name == name);

        public bool VideoExists(int id)
            => context.Videos.Any(v => v.Id == id);
    }
}
