using AutoMapper;
using AutoMapper.Execution;
using MovieAPI.DTO;
using MovieAPI.Model;

namespace MovieAPI.Helpers.ImageResolvers
{
    public class FavoriteMovieImageUrlResolver : IValueResolver<Favorite, FavoriteToReturnDTO, string>
    {

            private readonly IConfiguration _configuration;

            public FavoriteMovieImageUrlResolver(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string Resolve(Favorite source, FavoriteToReturnDTO destination, string destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Movie.Image))
                {
                    return $"{_configuration["ApiBaseUrl"]}{source.Movie.Image}";
                }
                return string.Empty;
            
        }
    }
}
