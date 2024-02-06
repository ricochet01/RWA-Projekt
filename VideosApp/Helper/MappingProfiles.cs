using AutoMapper;
using VideosApp.Dto;
using VideosApp.Model;

namespace VideosApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Mapping only the properties which we want to display
            // (we don't want to see e.g. every user in a specific country)

            CreateMap<Country, CountryDto>();
            // ...and vice versa
            CreateMap<CountryDto, Country>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();

            CreateMap<Image, ImageDto>();
            CreateMap<ImageDto, Image>();

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<Video, VideoDto>();
            CreateMap<VideoDto, Video>();
        }
    }
}
