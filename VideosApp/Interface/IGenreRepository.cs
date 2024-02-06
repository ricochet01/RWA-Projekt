using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int id);
        Genre GetGenre(string name);
        Genre GetVideoGenre(int videoId);
        ICollection<Video> GetVideosByGenre(int id);
        bool GenreExists(int id);

        bool CreateGenre(Genre genre);
        bool UpdateGenre(Genre genre);
        bool DeleteGenre(Genre genre);
        bool Save();
    }
}
