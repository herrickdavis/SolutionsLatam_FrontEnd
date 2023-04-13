using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlsGlobal.Components
{
  public class IdiomasViewComponent : ViewComponent
  {
    public async Task<IViewComponentResult> InvokeAsync()
    {
      return View();
    }
  }
}
