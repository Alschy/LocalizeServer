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
        Task ModifyResourceItem(ResourceModifyRequestModel model, CancellationToken cancel);

        Task DeleteResourceItem(ResourceRemoveRequestModel model, CancellationToken cancel);
    }
}
