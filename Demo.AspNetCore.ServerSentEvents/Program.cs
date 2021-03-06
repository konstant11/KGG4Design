﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Demo.AspNetCore.ServerSentEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://localhost:5000", "http://0.0.0.0:5000", "https://localhost:5001")
                   .UseStartup<Startup>();
    }
}
