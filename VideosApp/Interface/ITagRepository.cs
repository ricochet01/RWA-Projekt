using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        Tag GetTag(string name);
        ICollection<Video> GetVideosByTag(int tagId);
        bool TagExists(int id);
    }
}
