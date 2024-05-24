namespace AlsGlobal.Models
{
    public class NotificacionViewModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int empresa_id { get; set; }
        public int tipo_notificacion_id { get; set; }
        public int nivel_notificacion_id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string informacion_adicional { get; set;}
        public string leido { get; set; }
        public string created_at { get; set; }
    }
}
