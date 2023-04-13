namespace AlsGlobal.Models
{
  public class CambiarPasswordRequestViewModel
  {
    public string email { get; set; }
    public string password { get; set; }
    public string nueva_clave { get; set; }
  }
  public class CambiarPasswordResponseViewModel
  {
    public bool success { get; set; }
    public string message { get; set; }
  }
}
