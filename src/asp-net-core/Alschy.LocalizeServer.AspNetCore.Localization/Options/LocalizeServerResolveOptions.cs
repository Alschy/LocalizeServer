using System;
using System.Collections.Generic;
using System.Text;

namespace Alschy.LocalizeServer.AspNetCore.Localization.Options
{
    public class LocalizeServerResolveOptions
    {
        public string? ApplicationName { get; set; }

        public bool UseApplicationName { get; set; } = true;

        public string DefaultCulture { get; set; } = "en-US";
    }
}
