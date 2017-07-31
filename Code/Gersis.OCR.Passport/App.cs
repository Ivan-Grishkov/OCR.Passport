using System;
using System.IO;
using Gersis.OCR.Passport.Service.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Gersis.OCR.Passport.Service
{
    public class App
    {
        private IConfigurationRoot Configuration { get; set; }
        private ILogger<App> Logger { get; set; }
        private FileSystemWatcher FileSystemWatcher { get; set; }

        private string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public void Start(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                // json file is the first config layer
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true)
                // override it with environment variables with specified prefix
                .AddEnvironmentVariables("GERSIS_PASSPORT_SVC_")
                .Build();

            var serviceProvider = new ServiceCollection()
                // inject options/configuration
                // it's highly recommended to use dedicated options instead of whole IConfigurationRoot
                .AddSingleton(Configuration)
                // Add logging services
                .AddLogging()
                // Configure dependency injection
                .ConfigureContainer();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            ConfigureLogger(loggerFactory);
            serviceProvider.

            var loadFolderPath = Configuration.GetValue<string>("LoadingFolder");
            serviceProvider.SubscribeToDirectoryChanged(loggerFactory,loadFolderPath);

            
        }

        public void Stop()
        {
            Logger.LogInformation("Stopping application");
            var fsw = FileSystemWatcher;
            if (fsw != null)
            {
                FileSystemWatcher.Changed -= OnDirectoryContentChanged;
                FileSystemWatcher.Dispose();
            }
        }

        private void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            loggerFactory.AddSerilog();

            Logger = loggerFactory.CreateLogger<App>();
            Logger.LogInformation("Starting application");
            Logger.LogInformation("Environment: {0}", EnvironmentName);
        }
    }
}