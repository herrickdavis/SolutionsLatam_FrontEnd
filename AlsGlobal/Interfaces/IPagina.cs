namespace AlsGlobal.Interfaces
{
  public interface IPagina
  {
    int current_page { get; set; }
    int? from { get; set; }
    int last_page { get; set; }
    int per_page { get; set; }
    int? to { get; set; }
    int total { get; set; }
    bool UltimoBloque { get; set; }
    bool PrimerBloque { get; set; }
  }
}
