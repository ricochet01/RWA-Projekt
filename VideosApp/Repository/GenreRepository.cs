﻿using VideosApp.Data;
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

        public ICollection<Video> GetVideosByGenre(int id)
            => context.Videos.Where(v => v.GenreId == id).ToList();

        public bool GenreExists(int id)
            => context.Genres.Any(g => g.Id == id);

        public bool CreateGenre(Genre genre)
        {
            context.Add(genre);

            return Save();
        }

        public bool UpdateGenre(Genre genre)
        {
            context.Update(genre);

            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            // Obtain all videos of that genre
            var videos = context.Videos.Where(v => v.GenreId == genre.Id).ToList();

            // Then, we remove all VideoTag entries
            var videoTags = context.VideoTags.Where(vt => videos.Contains(vt.Video)).ToList();

            videoTags.ForEach(vt =>
            {
                context.Remove(vt);
            });

            // Next, we have to delete all videos with the genre
            videos.ForEach(v =>
            {
                context.Remove(v);
            });

            // Finally, we can remove the genre
            context.Remove(genre);

            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();

            return saved > 0;
        }
    }
}
