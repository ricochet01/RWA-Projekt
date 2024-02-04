using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext context;

        public TagRepository(DataContext context)
        {
            this.context = context;
        }

        public Tag GetTag(int id)
            => context.Tags.FirstOrDefault(t => t.Id == id);

        public Tag GetTag(string name)
            => context.Tags.FirstOrDefault(t => t.Name == name);

        public ICollection<Tag> GetTags()
            => context.Tags.OrderBy(t => t.Id).ToList();

        public ICollection<Video> GetVideosByTag(int tagId)
            => context.VideoTags.Where(vt => vt.TagId == tagId).Select(vt => vt.Video).ToList();
        

        public bool TagExists(int id)
            => context.Tags.Any(t => t.Id == id);
    }
}
