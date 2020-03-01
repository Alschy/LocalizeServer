using Alschy.LocalizeServer.Common.Models.Utils;

namespace Alschy.LocalizeServer.Common.Models
{
    public class ResourceResponseModel
    {
        public ResourceResponseModel(string value, EResolveState resolveState)
        {
            Value = value;
            ResolveState = resolveState;
        }

        public string Value { get; }
        public EResolveState ResolveState { get; }
    }
}
