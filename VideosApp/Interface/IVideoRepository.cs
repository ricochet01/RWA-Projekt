using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IVideoRepository
    {
        ICollection<Video> GetVideos();
        Video GetVideo(int id);
        Video GetVideo(string name);
        bool VideoExists(int id);
    }
}
