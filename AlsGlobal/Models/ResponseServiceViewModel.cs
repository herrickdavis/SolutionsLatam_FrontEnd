using AlsGlobal.Interfaces;
using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class ResponseServiceViewModel<THead, TData, TFormat>
  {
    public THead cabecera { get; set; }
    public TFormat format { get; set; }
    public Pagina<List<string>> pagina { get; set; }
    public ResponseResult ResponseResult { get; set; }
  }
  public class Pagina<TData>: IPagina
  {
    public int current_page { get; set; }
    public List<Data<TData>> data { get; set; }
    public int? from { get; set; }
    public int last_page { get; set; }
    public int per_page { get; set; }
    public int? to { get; set; }
    public int total { get; set; }
    public bool UltimoBloque { get; set; }
    public bool PrimerBloque { get; set; }
  }
  public class Data<TData>
  {
    public TData data { get; set; }
    public int id { get; set; }
    public Render render { get; set; }
  }
  public class Render
  {
    public bool con_documentos { get; set; }
    public string color { get; set; }
    public bool flag { get; set; }
  }
}
