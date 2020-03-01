using System;

namespace Alschy.LocalizeServer.Common.Models.Utils
{
    [Flags]
    public enum EResolveState
    {
        None = 0,
        System = 1,
        CultureComplete = 2,
        CultureParent = 4
    }
}
