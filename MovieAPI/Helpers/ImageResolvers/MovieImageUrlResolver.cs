using AutoMapper;
using AutoMapper.Execution;
using MovieAPI.DTO;
using MovieAPI.Model;

namespace MovieAPI.Helpers.ImageResolvers
{
    public class MovieImageUrlResolver : IValueResolver<Movie, MovieToReturnDTO, string>
    {

            private readonly IConfiguration _configuration;

            public MovieImageUrlResolver(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string Resolve(Movie source, MovieToReturnDTO destination, string destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Image))
                {
                    return $"{_configuration["ApiBaseUrl"]}{source.Image}";
                }
                return string.Empty;
            
        }
    }
}
