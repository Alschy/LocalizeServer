using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alschy.LocalizeServer.AspNetCore.Localization.Extensions
{
    public static class LocalizeServiceExtensions
    {
        public static ResourceResponseModel Localize(this ILocalizeService localizeService, ResourceRequestModel requestModel)
        {
            var task = localizeService.LocalizeAsync(requestModel, CancellationToken.None);
            task.Wait();
            return task.Result;
        }
    }
}
