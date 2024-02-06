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

        public ICollection<Tag> GetVideoTags(int id)
            => context.VideoTags.Where(vt => vt.VideoId == id).Select(vt => vt.Tag).ToList();

        public bool CreateVideo(int genreId, int imageId, int[] tagIds, Video video)
        {
            var imageEntity = context.Images.FirstOrDefault(i => i.Id == imageId);
            var genreEntity = context.Genres.FirstOrDefault(g => g.Id == genreId);

            video.Image = imageEntity;
            video.Genre = genreEntity;

            var tags = context.Tags.Where(t => tagIds.Contains(t.Id));

            ICollection<VideoTag> tagsList = new List<VideoTag>();
            foreach (var tag in tags)
            {
                var videoTag = new VideoTag()
                {
                    Video = video,
                    Tag = tag
                };

                tagsList.Add(videoTag);
            }

            video.VideoTags = tagsList;

            context.Add(video);

            return Save();
        }

        public bool UpdateVideo(int genreId, int imageId, int[] tagIds, Video video)
        {
            var imageEntity = context.Images.FirstOrDefault(i => i.Id == imageId);
            var genreEntity = context.Genres.FirstOrDefault(g => g.Id == genreId);

            video.Image = imageEntity;
            video.Genre = genreEntity;

            var tags = context.Tags.Where(t => tagIds.Contains(t.Id));

            ICollection<VideoTag> tagsList = new List<VideoTag>();
            foreach (var tag in tags)
            {
                var videoTag = new VideoTag()
                {
                    Video = video,
                    Tag = tag
                };

                tagsList.Add(videoTag);
            }

            video.VideoTags = tagsList;

            context.Update(video);

            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();

            return saved > 0;
        }
    }
}
