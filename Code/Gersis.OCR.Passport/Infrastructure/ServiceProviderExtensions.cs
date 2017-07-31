using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gersis.OCR.Passport.Service.Infrastructure
{
    public static class ServiceProviderExtensions
    {

        public static void SubscribeToDirectoryChanged (this IServiceProvider provider, string loadFolderPath)
        {
            var logger = provider.GetService<ILogger>();

            if (!Directory.Exists(loadFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(loadFolderPath);
                }
                catch (Exception e)
                {
                    logger.LogError($"Failed to create loading directory {loggerFactory} .", e);
                    throw;
                }
            }

            FileSystemWatcher = new FileSystemWatcher
            {
                Path = loadFolderPath,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.*",
                EnableRaisingEvents = true
            };
            FileSystemWatcher.Changed += OnDirectoryContentChanged;

            provider.GetRequiredService<ScheduleServiceImpl>();
        }



        private static void OnDirectoryContentChanged(object sender, FileSystemEventArgs e)
        {
            Logger.LogDebug("DirectoryContentChanged: ", e);


        }
    }
}