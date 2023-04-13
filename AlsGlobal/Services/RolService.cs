using AlsGlobal.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface IRolService
  {
    Task<IEnumerable<RolViewModel>> Obtener();
  }
  public class RolService: Service, IRolService
  {
    private readonly IWebHostEnvironment _webHost;
    public RolService(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }
    public async Task<IEnumerable<RolViewModel>> Obtener()
    {
        try{
            MemoryStream inMemoryCopy = new MemoryStream();
            var path = Path.Combine(_webHost.ContentRootPath, "Additionals", "Rol.json");
            using (StreamReader fs =  new StreamReader(path ))
            {
                string json = await fs.ReadToEndAsync();
                var roles = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<RolViewModel>>(json);
                return roles;
            }
        }
        catch{
            return null;
        }
    }
  }
}
