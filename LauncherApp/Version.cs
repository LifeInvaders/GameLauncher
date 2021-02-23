using System.Net;
using System.Text.Json;

namespace LauncherApp
{
    public class Version
    {
        public Version()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "C# console program");
            
            string url = "https://api.github.com/repos/Renaud-Dov/CheckStudents/releases/latest";
            string content = client.DownloadString(url);

        }

        // JsonDocument toto = JsonDocument.Parse()
    }
}