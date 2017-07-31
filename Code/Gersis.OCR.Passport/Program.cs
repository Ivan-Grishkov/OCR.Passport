using Topshelf;

namespace Gersis.OCR.Passport.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            InitService(args);
        }

        private static void InitService(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<App>(c =>
                {
                    c.ConstructUsing(() => new App());
                    c.WhenStarted(d => d.Start(args));
                    c.WhenStopped(d => d.Stop());
                });

                x.SetDescription("Gersis Passport recognition Service");
                x.SetDisplayName("Gersis Passport recognition Service");
                x.SetServiceName("Gersis.OCR.Passport.Service");
                x.RunAsNetworkService();
                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1); // restart the service after 1 minute
                    rc.SetResetPeriod(1); // set the reset interval to one day
                });
            });
        }
    }
}