namespace Alschy.LocalizeServer.WebClient.Configurations
{
    public class WebLocalizeConfiguration
    {
        public string Host { get; set; } = null!;

        public int Port { get; set; }

        public bool UseTls { get; set; } = true;

        public string ApiPath { get; set; } = "api/localize/";
    }
}
