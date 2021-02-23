using System.Net;
using ConsoleApp.Models;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var web = new Client();
            web.Update();
            
        }

       
    }
}