using System;
using System.Collections.Generic;
using System.Text;

namespace Alschy.LocalizeServer.Core.Management.Models
{
    public class ResourceRemoveRequestModel
    {
        public string Key { get; set; } = null!;

        public string? Culture { get; set; }

        public string? Application { get; set; }

    }
}
