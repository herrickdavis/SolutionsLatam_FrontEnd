namespace AlsGlobal.Models{
    public class ColumnasUsuarioRequestViewModel{
        public int numero_tabla { get; set; }
        public string[] orden { get; set; }
    }
    public class ColumnasUsuarioResponseViewModel{
        public string success { get; set; }
        public string mensaje { get; set; }
    }
}