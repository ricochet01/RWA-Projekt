using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetVideos();
        // Necessary for MVC 
        IQueryable<Video> GetAsyncVideos();
        Video GetVideo(int id);
        Video GetVideo(string name);
        bool VideoExists(int id);
        ICollection<Tag> GetVideoTags(int id);


		bool CreateVideo(int genreId, int imageId, int[] tagIds, Video video);
        bool UpdateVideo(int genreId, int imageId, int[] tagIds, Video video);
        bool DeleteVideo(Video video);
        bool Save();
    }
}
