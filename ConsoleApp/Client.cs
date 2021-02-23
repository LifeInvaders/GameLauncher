using System;
using System.Collections.Generic;
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
        private string OperatingSystem;
        private string pathDownload;

        public Client()
        {
            pathDownload =
                Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                    "builds");
            switch (Environment.OSVersion.Platform.ToString())
            {
                case "Win32NT":
                    OperatingSystem = "Windows";
                    break;
            }

            _webClient = new WebClient();
            _webClient.Headers.Add(
                "user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.182 Safari/537.36");
        }

        private Release GetRelease()
        {
            try
            {
                var content =
                    _webClient.DownloadString("https://api.github.com/repos/LifeInvaders/game/releases/latest");
                return JsonConvert.DeserializeObject<Release>(content);
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private void DownloadLastRelease(ReleaseAsset asset, int version)
        {
            string install_path = Path.Combine(pathDownload, "game.zip");
            _webClient.DownloadFile(asset.browser_download_url, install_path);

            ZipFile.ExtractToDirectory(install_path,
                pathDownload);
            File.Delete(install_path);
            File.WriteAllText(Path.Combine(pathDownload, "version"), Convert.ToString(version));
        }

        public void Update()
        {
            var release = GetRelease();
            if (!IsInstalled() || GetVersionInstalled() < release.Id)
            {
                // Télécharge la dernière version
                RemoveVersionInstalled();
                DownloadLastRelease(FindAsset(release.Assets), release.Id);
            }
        }

        private ReleaseAsset FindAsset(IReadOnlyList<ReleaseAsset> assets)
        {
            foreach (var asset in assets)
            {
                if (asset.Name.StartsWith(OperatingSystem))
                {
                    return asset;
                }
            }

            return null;
        }

        private bool IsInstalled()
        {
            return File.Exists(Path.Combine(pathDownload, "version"));
        }

        private int GetVersionInstalled() =>
            int.Parse(File.ReadAllText(Path.Combine(pathDownload, "version")) ?? string.Empty);

        public void RemoveVersionInstalled()
        {
            if (Directory.Exists(pathDownload))
            {
                ClearFolder(pathDownload);
                Directory.CreateDirectory(pathDownload);
            }
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