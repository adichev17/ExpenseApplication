using FNSApi.Models.Fns.Requests;
using Mapster;

namespace FNSApi.Common.Mapping
{
    public class FnsTokenRequestMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<FnsSettings, FnsTokenRequest>();
        }
    }
}
