using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using ConsoleApp.Models;
using Newtonsoft.Json;

namespace ConsoleApp
{
    public class Client
    {
        private WebClient _webClient;
        private OperatingSystem os_info;
        private OS OperatingSystem;
        private string pathDownload;

        public Client()
        {
            pathDownload =
                Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                    "builds");
            switch (Environment.OSVersion.Platform.ToString())
            {
                case "Win32NT":
                    OperatingSystem = OS.Windows;
                    break;
                default:
                    OperatingSystem = OS.Null;
                    break;
            }

            _webClient = new WebClient();
            _webClient.Headers.Add(
                "user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.182 Safari/537.36");
        }

        public Release GetRelease()
        {
            try
            {
                var content =
                    _webClient.DownloadString("https://api.github.com/repos/ezBastion/ezb_srv/releases/latest");
                return JsonConvert.DeserializeObject<Release>(content);
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public void DownloadLastRelease(ReleaseAsset asset, int version)
        {
            try
            {
                if (!Directory.Exists(pathDownload))
                    Directory.CreateDirectory(pathDownload);

                _webClient.DownloadFile(asset.browser_download_url, Path.Combine(pathDownload, asset.Name));

                ZipFile.ExtractToDirectory(Path.Combine(pathDownload, asset.Name),
                    Path.Combine(pathDownload, asset.Name));
                File.Delete(Path.Combine(pathDownload, asset.Name));
                File.WriteAllText(Path.Combine(pathDownload, "version"), Convert.ToString(version));
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
            }
        }

        public bool IsInstalled()
        {
            return File.Exists(Path.Combine(pathDownload, "version"));
        }

        public int GetVersionInstalled() =>
            int.Parse(File.ReadAllText(Path.Combine(pathDownload, "version")) ?? string.Empty);

        public void RemoveVersionInstalled()
        {
            ClearFolder(pathDownload);
            Directory.CreateDirectory(pathDownload);
        }


        private void ClearFolder(string folderName)
        {
            DirectoryInfo dir = new DirectoryInfo(folderName);
            foreach (FileInfo fi in dir.GetFiles())
                fi.Delete();

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }
    }
}