using AlsGlobal.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface IIdiomasService
  {
    Task<IEnumerable<IdiomaViewModel>> Obtener();
  }
  public class IdiomasService: Service, IIdiomasService
  {
    private readonly IWebHostEnvironment _webHost;
    public IdiomasService(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }
    public async Task<IEnumerable<IdiomaViewModel>> Obtener()
    {
        try{
            MemoryStream inMemoryCopy = new MemoryStream();
            var path = Path.Combine(_webHost.ContentRootPath, "Additionals", "Idiomas.json");
            using (StreamReader fs =  new StreamReader(path))
            {
                string json = await fs.ReadToEndAsync();
                var idiomas = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<IdiomaViewModel>>(json);
                return idiomas;
            }
        }
        catch{
            return null;
        }
    }
  }
}
