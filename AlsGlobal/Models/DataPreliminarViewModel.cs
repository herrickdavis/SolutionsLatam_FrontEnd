using AlsGlobal.Interfaces;
using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class DataPreliminarViewModel
  {
    public List<string> cabecera { get; set; }
    public Pagina pagina { get; set; }
    public ResponseResult ResponseResult { get; set; }
  }

  public class Pagina:IPagina
  {
    public int current_page { get; set; }
    public List<List<string>> data { get; set; }
    public int? from { get; set; }
    public int last_page { get; set; }
    public int per_page { get; set; }
    public int? to { get; set; }
    public int total { get; set; }
    public bool UltimoBloque { get; set; }
    public bool PrimerBloque { get; set; }
  }
}
