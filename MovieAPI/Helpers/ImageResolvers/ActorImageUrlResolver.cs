using AutoMapper;
using AutoMapper.Execution;
using MovieAPI.DTO;
using MovieAPI.Model;

namespace MovieAPI.Helpers.ImageResolvers
{
    public class ActorImageUrlResolver : IValueResolver<Actor, ActorToReturnDTO, string>
    {

            private readonly IConfiguration _configuration;

            public ActorImageUrlResolver(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string Resolve(Actor source, ActorToReturnDTO destination, string destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Image))
                {
                    return $"{_configuration["ApiBaseUrl"]}{source.Image}";
                }
                return string.Empty;
            
        }
    }
}
