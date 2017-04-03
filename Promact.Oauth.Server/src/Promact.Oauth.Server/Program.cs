using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;

namespace Promact.Oauth.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {            
            var host = new WebHostBuilder()
                .UseKestrel(KestralOptions)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static void KestralOptions(KestrelServerOptions kestrelServerOptions)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Production")
            {
                var password = Environment.GetEnvironmentVariable("SELFSIGNED_CERT_PASSWORD");
                kestrelServerOptions.UseHttps("cert.pfx", password);                
            }
        }
    }
}
