using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Promact.Oauth.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=========="+ Directory.GetCurrentDirectory() +"=============");
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
