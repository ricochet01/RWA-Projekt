using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IVideoRepository
    {
        ICollection<Video> GetVideos();
        Video GetVideo(int id);
        Video GetVideo(string name);
        bool VideoExists(int id);
        ICollection<Tag> GetVideoTags(int id);

        bool CreateVideo(int genreId, int imageId, int[] tagIds, Video video);
        bool UpdateVideo(int genreId, int imageId, int[] tagIds, Video video);
        bool Save();
    }
}
