using Alschy.LocalizeServer.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.Common.Services
{
    public interface ILocalizeService
    {
        Task<ResourceResponseModel> LocalizeAsync(ResourceRequestModel request, CancellationToken cancel);
    }
}
