using Alschy.LocalizeServer.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.Utils.Caching.Services
{
    public interface ILocalizeCacheProvider
    {
        Task<ResourceResponseModel?> GetCachedItemAsync(ResourceRequestModel model, CancellationToken cancel);

        Task WriteChacheItemAsync(ResourceRequestModel requestModel, ResourceResponseModel responseModel, CancellationToken cancel);
    }
}
