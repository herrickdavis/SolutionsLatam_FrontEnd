
using System.IO;

namespace AlsGlobal.Models
{
  public class ArchivoViewModel
  {
    public ArchivoViewModel(string contentType, string nombre, Stream stream)
    {
      ContentType = contentType;
      Nombre = nombre;
      Stream = stream;
    }
    public string ContentType { get; set; }
    public string Nombre { get; set; }
    public Stream Stream { get; set; }
  }
}
