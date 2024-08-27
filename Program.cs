using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using RestApiWithServiceWorker.Domain;
using RestApiWithServiceWorker.Service;
using WorkerService;

namespace RestApiWithServiceWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    // logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:8888/", "http://*:1980/", "http://*:8080/");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddMvc();
                    services.AddSingleton<IWiaService, WiaService>();
                    services.AddSingleton<ISendFileToNaumen, SendFileToNaumen>();
                    services.AddSingleton<IWiaDevices, WiaDevice>();
                    services.AddSingleton<IDataStore, DataStore>();
                })
                .UseNLog()
                .UseWindowsService();
    }
}

