using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AlsGlobal.Configurations
{
  public static class LanguageConfiguration
  {
    public static void AddLanguageConfiguration(this IServiceCollection services)
    {
      services.AddLocalization(o => o.ResourcesPath = "Resources");
      services.AddScoped<IStringLocalizer, StringLocalizer<SharedResources>>();

      services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

      services.AddRazorPages()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new[]{
            new CultureInfo("es-PE"),
            new CultureInfo("en-US"),
            new CultureInfo("pt-BR")
        };
        options.DefaultRequestCulture = new RequestCulture("es-PE");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders = new List<IRequestCultureProvider>() {
          new QueryStringRequestCultureProvider(),
          new CookieRequestCultureProvider()};
      });
    }

    public static void UseLanguageConfiguration(this IApplicationBuilder app)
    {
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-PE");
      CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-PE");
    }
  }
}
