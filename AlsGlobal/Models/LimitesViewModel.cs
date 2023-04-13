
namespace AlsGlobal.Models
{
  public class LimitesRequestViewModel
  {
    public LimitesRequestViewModel(string id)
    {
      id_tipo_muestra = id;
    }
    public string id_tipo_muestra { get; set; }
  }
  public class LimitesViewModel
  {
    public int id { get; set; }
    public string nombre_limite { get; set; }
  }
}
