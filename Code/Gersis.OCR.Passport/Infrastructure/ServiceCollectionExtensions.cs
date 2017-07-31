using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;

namespace Gersis.OCR.Passport.Service.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureContainer(this IServiceCollection services, ILogger logger)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.For<ILogger>().Use(logger);

                // Register stuff in container, using the StructureMap APIs
                // also register MediatR specifics
                config.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Gersis"));
                    scanner.AssemblyContainingType<Program>();
                    scanner.WithDefaultConventions();
                });

                // Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}