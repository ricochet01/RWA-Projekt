using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        Tag GetTag(string name);
        ICollection<Video> GetVideosByTag(int id);
        bool TagExists(int id);

        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
        bool Save();
    }
}
