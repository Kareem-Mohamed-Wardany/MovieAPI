using AutoMapper;
using AutoMapper.Execution;
using MovieAPI.DTO;
using MovieAPI.Model;

namespace MovieAPI.Helpers
{
    public class DirectorImageUrlResolver : IValueResolver<Director, DirectorToReturnDTO, string>
    {

            private readonly IConfiguration _configuration;

            public DirectorImageUrlResolver(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string Resolve(Director source, DirectorToReturnDTO destination, string destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Image))
                {
                    return $"{_configuration["ApiBaseUrl"]}{source.Image}";
                }
                return string.Empty;
            
        }
    }
}
