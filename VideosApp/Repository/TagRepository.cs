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

        public bool CreateTag(Tag tag)
        {
            context.Add(tag);

            return Save();
        }

        public bool UpdateTag(Tag tag)
        {
            context.Update(tag);

            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            var videoTags = context.VideoTags.Where(vt => vt.TagId == tag.Id).ToList();
            videoTags.ForEach(vt =>
            {
                context.Remove(vt);
            });

            context.Remove(tag);

            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();

            return saved > 0;
        }
    }
}
