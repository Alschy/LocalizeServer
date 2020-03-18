using Alschy.LocalizeServer.Core.Management.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.Core.Management.Services
{
    public interface ILocalizeManagementService
    {
        Task AddResourceItemAsync(ResourceModifyRequestModel model, CancellationToken cancel);
    }
}
