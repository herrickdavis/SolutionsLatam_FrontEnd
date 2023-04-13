using System;
using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class ReporteHistoricoRequestViewModel
  {
    public DateTime fecha_inicio { get; set; }
    public DateTime fecha_fin { get; set; }
    public int id_matriz { get; set; }
    public int? id_limite { get; set; }
    public string[] id_proyecto { get; set; }
    public string id_tipo_muestra { get; set; }
    public string[] estaciones { get; set; }
    public string[] parametros { get; set; }

  }
  public class ReporteHistoricoViewModel
  {
    public string[] dates { get; set; }
    public IEnumerable<Graficas> graficas { get; set; }
    public class Graficas {
      public string titulo { get; set; }
      public string ejex { get; set; }
      public string ejey { get; set; }
      public IEnumerable<Serie> series { get; set; }
      public class Serie 
      {
        public string nombre { get; set; }
        public IEnumerable<decimal?> datos { get; set; }
        public string color { get; set; }
        public bool showSymbol { get; set; }
      }
    }
  }
  
  public class EstacionesRequestViewModel
  {
    public EstacionesRequestViewModel()
    {

    }
    public EstacionesRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin, string id_tipo_muestra)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
      this.id_tipo_muestra = id_tipo_muestra;
      this.fuera_limite = true;
    }
    public string fecha_inicio { get; set; }
    public string fecha_fin { get; set; }
    public string id_tipo_muestra { get; set; }
    public string[] id_proyecto { get; set; }
    public bool fuera_limite { get; set; }
  }
 
  public class ParametrosRequestViewModel
  {
    public ParametrosRequestViewModel()
    {

    }
    public ParametrosRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin, string id_tipo_muestra, string[] estaciones)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
      this.id_tipo_muestra = id_tipo_muestra;
      this.estaciones = estaciones;
    }
    public string fecha_inicio { get; set; }
    public string fecha_fin { get; set; }
    public string id_tipo_muestra { get; set; }
    public string[] id_proyecto { get; set; }
    public bool fuera_limite { get; set; }
    public string[] estaciones { get; set; }
  }
}
