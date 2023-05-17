using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using AlsGlobal.Services;
using AlsGlobal.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using AlsGlobal.Configurations;
using Microsoft.AspNetCore.HttpOverrides;

namespace AlsGlobal
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddWebConfiguration();
      services.AddLanguageConfiguration();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                 options.LoginPath = "/";
                 options.AccessDeniedPath = "/Error/403";
               });

      string baseAddress = Configuration.GetValue<string>("AppSetting:Url");
      services.AddHttpClient<IAutenticacionService, AutenticacionService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IDataPreliminarService, DataPreliminarService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IInformesService, InformesService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IMuestrasService, MuestrasService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IFueraLimiteService, FueraLimiteService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IReporteHistoricoService, ReporteHistoricoService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IMatrizService, MatrizService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IParametrosService, ParametrosService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IEstacionesService, EstacionesService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<ILimitesService, LimitesService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IPerfilService, PerfilService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IEmpresaService, EmpresaService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IUsuarioService, UsuarioService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IProyectoService, ProyectoService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<IAllEstacionesService, AllEstacionesService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddHttpClient<ICadenasService, CadenasService>(x => x.BaseAddress = new Uri(baseAddress));
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IAspNetUser, AspNetUser>();
      services.AddScoped<IRolService, RolService>();
      services.AddScoped<IIdiomasService, IdiomasService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseWebConfiguration(env);

      app.UseLanguageConfiguration();
    }
  }
}
