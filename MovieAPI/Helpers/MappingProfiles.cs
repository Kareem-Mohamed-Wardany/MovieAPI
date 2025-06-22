using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MovieAPI.DTO;
using MovieAPI.Helpers.ImageResolvers;
using MovieAPI.Model;

namespace MovieAPI.Helpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<AddUpdateDTO, Actor>();
            CreateMap<Actor, ActorToReturnDTO>().ForMember(d => d.Image, o => o.MapFrom<ActorImageUrlResolver>());

            CreateMap<AddUpdateDTO, Director>();
            CreateMap<Director, DirectorToReturnDTO>().ForMember(d => d.Image, o => o.MapFrom<DirectorImageUrlResolver>());
            CreateMap<Movie, DirectorMovieToReturnDTO>().ForMember(d => d.Image, o => o.MapFrom<DirectorMovieImageUrlResolver>());

            CreateMap<MovieToAddDTO, Movie>();
            CreateMap<Movie, MovieToReturnDTO>().ForMember(d => d.DirectorName, o => o.MapFrom(s=>s.Director.Name)).ForMember(d => d.Image, o => o.MapFrom<MovieImageUrlResolver>())
                .ForMember(d => d.Genre, o => o.MapFrom(s => s.Genre.ToString()));

            CreateMap<ReviewToAddDTO, Review>();
            CreateMap<Review, ReviewToReturnDTO>().ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName)).ForMember(d => d.ReviewId, o => o.MapFrom(s => s.Id));


            CreateMap<Favorite, FavoriteToReturnDTO>()
                .ForMember(d => d.MovieId, o => o.MapFrom(s => s.MovieId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Movie.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Movie.Description))
                .ForMember(d => d.Genre, o => o.MapFrom(s => s.Movie.Genre.ToString()))
                .ForMember(d => d.Duration, o => o.MapFrom(s => s.Movie.Duration))
                .ForMember(d => d.Image, o => o.MapFrom<FavoriteMovieImageUrlResolver>())
                .ForMember(d => d.Rate, o => o.MapFrom(s => s.Movie.Rate))
                .ForMember(d => d.DirectorName, o => o.MapFrom(s => s.Movie.Director.Name))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate));





            CreateMap<IdentityUser, UserToReturnDTO>();
        }
    }
}
