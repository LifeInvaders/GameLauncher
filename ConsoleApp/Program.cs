using System.Net;
using ConsoleApp.Models;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            string url = "https://api.github.com/repos/ezBastion/ezb_srv/releases/latest";
            var web = new Client();
            var a = web.GetRelease();
            foreach (var releaseAsset in a.Assets)
            {
                web.DownloadLastRelease(releaseAsset, a.Id);
            }

        }

       
    }
}