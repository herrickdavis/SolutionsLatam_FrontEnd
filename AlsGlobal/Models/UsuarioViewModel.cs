using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlsGlobal.Interfaces;

namespace AlsGlobal.Models
{
  public class LoginViewModel
  {
     
    [Required(ErrorMessage = "EmailRequerido")]
    [EmailAddress(ErrorMessage = "EmailFormatoIncorrecto")]
    public string email { get; set; }

    [Required(ErrorMessage = "ContrasenaRequerido")]
    public string password { get; set; }
  }

  public class UsuarioRespuestaViewModel
  {
    public string nombre { get; set; }
    public string email { get; set; }
    public int rol { get; set; }
    public string data_campo{get;set;}
    public IEnumerable<MenuViewModel> menu { get; set; }
    public string token { get; set; }
    public ResponseResult ResponseResult { get; set; }
  }
  public class MenuViewModel
  {
    public int id { get; set; }
    public string nombre_matriz { get; set; }
  }
  public class UsuarioRequestViewModel{
    public string nombre { get; set; }
    public string email { get; set; }
  }
  public class UsuarioResponseViewModel:IPagina{
      public IEnumerable<UsuarioData> data { get; set; }
      public int current_page { get; set; }
      public int? from { get; set; }
      public int last_page { get; set; }
      public int per_page { get; set; }
      public int? to { get; set; }
      public int total { get; set; }
      public bool UltimoBloque { get; set; }
      public bool PrimerBloque { get; set; }
      public class UsuarioData{
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string nombre_empresa { get; set; }
        public string rol { get; set; }
        public string activo { get; set; }
      }
  }
  public class CrearUsuarioRequestViewModel{
    [Required(ErrorMessage = "El Nombre es obligatorio")]
    public string nombre { get; set; }
    [Required(ErrorMessage = "El Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato invalido")]
    public string email { get; set; }
    [Required(ErrorMessage = "La Contraseña es obligatoria")]
    public string password { get; set; }
    [Required(ErrorMessage = "La Empresa es obligatoria")]
    public int[] id_empresas { get; set; }
    [Required(ErrorMessage = "El Rol es obligatorio")]
    public int id_rol { get; set; }
    [Required(ErrorMessage = "El Idioma es obligatorio")]
    public string idioma { get; set; }
    [Required(ErrorMessage = "El Data Campo es obligatorio")]
    public string data_campo { get; set; }
    public string ver_empresa_solicitante { get; set; }
    public string ver_contacto_solicitante { get; set; }
    public string ver_empresa_contratante { get; set; }
    public string ver_contacto_contratante { get; set; }
  }
  public class CrearUsuarioResponseViewModel{
    public string error { get; set; }
    public string success { get; set; }
    public string mensaje { get; set; }
  }
  public class EditarUsuarioRequestViewModel{
    public int id { get; set; }
    [Required(ErrorMessage = "El Nombre es obligatorio")]
    public string nombre { get; set; }
     [Required(ErrorMessage = "El Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato invalido")]
    public string email { get; set; }
     public string password { get; set; }
    [Required(ErrorMessage = "La Empresa es obligatoria")]
    public int[] id_empresas { get; set; }
    [Required(ErrorMessage = "El Rol es obligatorio")]
    public int id_rol { get; set; }
    [Required(ErrorMessage = "El Idioma es obligatorio")]
    public string idioma { get; set; }
     [Required(ErrorMessage = "El Data Campo es obligatorio")]
    public string data_campo { get; set; }
    public string ver_empresa_sol { get; set; }
    public string ver_contacto_sol { get; set; }
    public string ver_empresa_con { get; set; }
    public string ver_contacto_con { get; set; }
  }
  public class EditarUsuarioResponseViewModel{
    public string error { get; set; }
    public string success { get; set; }
    public string mensaje { get; set; }
  }
}
