namespace AlsGlobal.Models{
    public class ColumnasRequestViewModel{
        public ColumnasRequestViewModel(int tabla)
        {
            this.tabla = tabla;
        }
        public int tabla { get; set; }
    }
    public class ColumnasResponseViewModel{
        public string columna { get; set; }
        public bool estado { get; set; }
    }
}