using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int id);
        Genre GetGenre(string name);
        Genre GetVideoGenre(int videoId);
        bool GenreExists(int id);
    }
}
