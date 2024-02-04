using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext context;

        public GenreRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Genre> GetGenres()
            => context.Genres.OrderBy(g => g.Id).ToList();

        public Genre GetGenre(int id)
            => context.Genres.FirstOrDefault(g => g.Id == id);

        public Genre GetGenre(string name)
            => context.Genres.FirstOrDefault(g => g.Name == name);

        public Genre GetVideoGenre(int videoId)
            => context.Videos.Where(v => v.Id == videoId).Select(v => v.Genre).FirstOrDefault();

        public bool GenreExists(int id)
            => context.Genres.Any(g => g.Id == id);
    }
}
