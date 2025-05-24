using AutoMapper;
using AutoMapper.Execution;
using MovieAPI.DTO;
using MovieAPI.Model;

namespace MovieAPI.Helpers.ImageResolvers
{
    public class DirectorMovieImageUrlResolver : IValueResolver<Movie, DirectorMovieToReturnDTO, string>
    {

            private readonly IConfiguration _configuration;

            public DirectorMovieImageUrlResolver(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string Resolve(Movie source, DirectorMovieToReturnDTO destination, string destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Image))
                {
                    return $"{_configuration["ApiBaseUrl"]}{source.Image}";
                }
                return string.Empty;
            
        }
    }
}
