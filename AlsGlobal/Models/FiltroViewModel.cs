using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class FiltrosViewModel
  {
    public FiltrosViewModel()
    {
      filtros = new List<FiltroViewModel>();
    }
    public ICollection<FiltroViewModel> filtros { get; set; }
  }
  public class FiltroViewModel {
    public string cabecera { get; set; }
    public string condicion { get; set; }
    public string valor { get; set; }
  }
}
