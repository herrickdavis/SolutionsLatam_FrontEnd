using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlsGlobal.Components
{
  public class SummaryViewComponent:ViewComponent
  {
    public async Task<IViewComponentResult> InvokeAsync()
    {
      return View();
    }
  }
}
