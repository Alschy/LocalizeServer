using System;
using System.Collections.Generic;
using System.Text;

namespace Alschy.LocalizeServer.Core.Management.Models
{
    public class ResourceModifyRequestModel
    {
        public string Key { get; set; } = null!;

        public string Culture { get; set; } = null!;

        public string? Application { get; set; }

        public string Value { get; set; } = null!;
    }
}
