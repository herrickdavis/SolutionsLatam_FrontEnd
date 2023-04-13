using System;
using System.Collections.Generic;
using System.IO;

namespace AlsGlobal.Models
{
  public class MuestraViewModel
  {
    public MuestraViewModel()
    {
      info = new Info();
      limite = new List<Limite>();
      parametros = new List<Parametros>();
      coordenadas = new Coordenadas();
    }
    public Info info { get; set; }
    public List<Limite> limite { get; set; }
    public bool mostrar_limite { get; set; }
    public List<Parametros> parametros { get; set; }
    public Coordenadas coordenadas { get; set; }
    public ResponseResult ResponseResult { get; set; }
    public class Info
    {
      public string estado { get; set; }
      public string numero_grupo { get; set; }
      public string numero_muestra { get; set; }
      public int codigo_muestra { get; set; }
      public string estacion { get; set; }
      public string proyecto { get; set; }
      public string tipo_muestra { get; set; }
      public string contratante { get; set; }
      public string solicitante { get; set; }
      public string fecha_muestreo { get; set; }
      public string latitud { get; set; }
      public string longitud { get; set; }
      public string zona { get; set; }
      public string procedencia { get; set; }
    }
    public class Limite
    {
      public int id { get; set; }
      public string nombre { get; set; }
      public bool select { get; set; }
    }
    public class Coordenadas
    {
      public bool mostrar_mapa { get; set; }
      public string latitud { get; set; }
      public string longitud { get; set; }
    }
    public class Parametros
    {
      public string nombre { get; set; }
      public string valor { get; set; }
      public string limite { get; set; }
      public bool mostrar { get; set; }
      public string color { get; set; }
      public string unidad { get; set; }
    }
  }

  public class MuestraRequestViewModel
  {
    public int id_muestra { get; set; }
    public int? id_limite { get; set; }
  }
  public class TipoMuestraRequestViewModel
  {
    public TipoMuestraRequestViewModel()
    {

    }
    public TipoMuestraRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin, bool fuera_limite)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
      this.fuera_limite = fuera_limite;
    }
    public TipoMuestraRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin, int id_matriz)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
      this.id_matriz = id_matriz;
    }
    public TipoMuestraRequestViewModel(DateTime fecha_inicio, DateTime fecha_fin, int id_matriz, bool fuera_limite)
    {
      this.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
      this.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
      this.id_matriz = id_matriz;
      this.fuera_limite = fuera_limite;
    }
    public string fecha_inicio { get; set; }
    public string fecha_fin { get; set; }
    public bool fuera_limite { get; set; }
    public int id_matriz { get; set; }
  }
  public class TipoMuestraViewModel
  {
    public string id { get; set; }
    public string nombre_tipo_muestra { get; set; }
  }

  public class DocumentoMuestraRequestViewModel
  {
    public string[] id_muestra { get; set; }
    public string[] id_tipo_archivo { get; set; }
    public string id_documento { get; set; }
  }
  public class DocumentoMuestraViewModel
  {
    public string id { get; set; }
    public string nombre_documento { get; set; }
    public string extension { get; set; }
  }
  
}
