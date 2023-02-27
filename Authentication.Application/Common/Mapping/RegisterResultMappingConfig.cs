using Authentication.Application.Common.Dtos;
using Authentication.Domain.Entities;
using Mapster;

namespace Authentication.Application.Common.Mapping
{
    public class RegisterResultMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserEntity, RegisterResultDto>();
        }
    }
}
