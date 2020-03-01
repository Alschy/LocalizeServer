namespace Alschy.LocalizeServer.Common.Models
{
    public class ResourceRequestModel
    {
        public ResourceRequestModel(string key, string culture, string? system)
        {
            Key = key;
            Culture = culture;
            System = system;
        }

        public string Key { get; }
        public string Culture { get; }
        public string? System { get; }
    }
}
