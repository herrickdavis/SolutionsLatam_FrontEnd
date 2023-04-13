using AlsGlobal.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace AlsGlobal.Configurations
{
  public static class WebConfiguration
  {
    public static void AddWebConfiguration(this IServiceCollection services)
    {
      services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();
      services.AddMvc()
        .AddDataAnnotationsLocalization(options =>
        {
          options.DataAnnotationLocalizerProvider = (type, factory) =>
              factory.Create(typeof(SharedResources));
        });
    }

    public static void UseWebConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();
      app.UseRequestLocalization();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseMiddleware<ExceptionMiddleware>();
      app.UseMiddleware<CultureMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
