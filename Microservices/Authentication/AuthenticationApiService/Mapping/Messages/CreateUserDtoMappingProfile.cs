using Authentication.Application.Common.Dtos;
using AuthenticationApiService.Messages;
using Mapster;

namespace AuthenticationApiService.Mapping.Messages
{
    public class CreateUserDtoMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterResultDto, CreateUserDto>();
        }
    }
}
