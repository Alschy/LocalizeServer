using System.Collections.Generic;

namespace Alschy.LocalizeServer.Core.Models
{

    public abstract class ResourceHead<TItem>
        where TItem : ResourceItem
    {
        public string ResourceKey { get; set; } = null;

        public virtual IList<TItem> ResourceItems { get; set; } = new List<TItem>();
    }

    public class ResourceHead : ResourceHead<ResourceItem>
    { }
}
