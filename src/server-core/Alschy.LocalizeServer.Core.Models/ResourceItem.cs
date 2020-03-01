namespace Alschy.LocalizeServer.Core.Models
{
    public class ResourceItem
    {
        public string? Culture { get; set; }

        public string? Application { get; set; }

        public string Value { get; set; } = null!;
    }
}
