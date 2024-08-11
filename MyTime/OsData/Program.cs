using System.Threading.Tasks;

namespace OsData
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static async Task Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //

            OsData app = new OsData();
            await app.Start(args);

            //  using async to define function all the way to the top (Main) to avoid blocking the main thread and to avoid Main exit before the async function is done.
            // await Task.WhenAll(task1, task2, task3);
        }
    }
}