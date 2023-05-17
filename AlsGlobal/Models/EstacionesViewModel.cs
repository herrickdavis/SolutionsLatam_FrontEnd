
namespace AlsGlobal.Models
{
public class AsignarAliasRequestViewModel
{
    public string[] id { get; set; }
    public string alias { get; set; }
}
    public class AsignarAliasResponseModel
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
    }
    public class EstacionesReporteEstacionesRequestViewModel
  {
    public string id_tipo_muestra { get; set; }
    public string id_parametro { get; set; }
  }
  public class EstacionesViewModel
  {
    public string id { get; set; }
    public string nombre_estacion { get; set; }
  }
  public class ReporteEstacionesRequestViewModel
  {
    public string id_tipo_muestra { get; set; }
    public string id_parametro { get; set; }
    public string[] id_estaciones { get; set; }
  }
  public class ReporteEstacionesViewModel
  {
    public string estacion { get; set; }
    public string muestra { get; set; }
    public string fecha_muestreo { get; set; }
    public string valor { get; set; }
    public string unidad { get; set; }
    public string punto_mapa { get; set; }
    public string latitud { get; set; }
    public string longitud { get; set; }
  }
}
