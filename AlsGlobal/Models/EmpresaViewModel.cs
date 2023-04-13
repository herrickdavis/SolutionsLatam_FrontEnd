using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class EmpresaViewModel
  {
    public int id { get; set; }
    public string nombre_empresa { get; set; }
  }
  public class EmpresaResponseViewModel
  {
    public string success { get; set; }
    public bool successBoolean { get => success == "Ok"; private set => successBoolean = value; }
    public string mensaje { get; set; }
    public IEnumerable<MenuViewModel> menu { get; set; }
  }
}
