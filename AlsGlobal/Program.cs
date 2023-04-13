using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AlsGlobal
{
  public class Program
  {
    /*
     Se pensó trabajar todas las vistas de manera dinamica, pero un cambio de estructura entre
    la vista Data Preliminar y las otras por eso esta vista maneja un ViewModel distinto a las demás.
     */
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder
              //.UseKestrel()
              //.UseUrls("http://10.10.80.141:5004")
              .UseStartup<Startup>();
            });
  }
}
