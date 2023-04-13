using System.Collections.Generic;
using System.IO;

namespace AlsGlobal.Models
{
  public class MatrizViewModel
  {
    public string nombre { get; set; }
    public IEnumerable<Tipo_Muestra> tipo_muestras { get; set; }
    public class Tipo_Muestra
    {
      public string id { get; set; }
      public string tipo_muestra { get; set; }
    }
  }
 
}
