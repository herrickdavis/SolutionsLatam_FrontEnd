using System;
using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class FueraDeLimiteRequestViewModel
  {
    public FueraDeLimiteRequestViewModel()
    {

    }
    public FueraDeLimiteRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
    }
    public string fecha_inicio { get; set; }
    public string fecha_fin { get; set; }
    public string id_tipo_muestra { get; set; }
    public string[] id_estaciones { get; set; }
  }
  public class FueraDeLimiteViewModel
  {
    public string numero_muestra { get; set; }
    public string fecha_muestreo { get; set; }
    public string nombre_estacion { get; set; }
    public Coordenadas coordenadas { get; set; }
    public List<Data> data { get; set; }

    public class Coordenadas
    {
      public bool mostrar_mapa { get; set; }
      public string latitud { get; set; }
      public string longitud { get; set; }
    }
    public class Data
    {
      public string nombre_parametro { get; set; }
      public string resultado { get; set; }
      public string norma { get; set; }
      public string minimo { get; set; }
      public string maximo { get; set; }
    }
  }
}
