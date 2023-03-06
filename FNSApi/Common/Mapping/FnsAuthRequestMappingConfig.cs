using FNSApi.Models.Fns.Requests;
using Mapster;

namespace FNSApi.Common.Mapping
{
    public class FnsAuthRequestMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<FnsSettings, FnsAuthRequest>();
        }
    }
}
