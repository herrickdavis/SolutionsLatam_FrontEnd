using System.Collections.Generic;
using static AlsGlobal.Models.MuestraViewModel;

namespace AlsGlobal.Models
{
    public class TelemetriaViewModel
    {
    }

    public class TelemetriaInfoViewModel
    {
        public int id_estacion { get; set; }
        public string nombre_estacion { get; set; }
        public IEnumerable<TelemetriaParametrosViewModel> parametros { get; set; }
    }

    public class TelemetriaParametrosViewModel
    {
        public int id_parametro { get; set; }
        public string nombre_parametro { get; set; }
    }

    public class TelemetriaDataViewModel
    {
        public int parametro_id { get; set; }
        public string nombre_parametro { get; set; }
        public string nombre_estacion { get; set; }
        public string fecha_muestreo { get; set; }
        public string resultado { get; set; }
    }

    public class TelemetriaDataWindRoseViewModel
    {
        public int parametro_id { get; set; }
        public string nombre_parametro { get; set; }
        public string nombre_estacion { get; set; }
        public string fecha_muestreo { get; set; }
        public string resultado { get; set; }
        public string WindDir_D1_WVT { get; set; }
        public string PM25_Avg { get; set; }
    }

    public class TelemetriaAllParametro
    {
        public int id { get; set; }
        public string nombre_parametro { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class TelemetriaAllGrupoParametro
    {
        public int id_grupo { get; set; }
        public string nombre_grupo_parametro { get; set; }
        public IEnumerable<TelemetriaParametrosViewModel> parametros { get; set; }
    }

    public class TelemetriaAllStation
    {
        public string nombre_estacion { get; set; }
    }

    public class TelemetriaSetLimite
    {
        public string id_parametro { get; set; }
        public string limite_inferior { get; set; }
        public string limite_superior { get; set; }
    }

    public class TelemetriaSetGrupoParametros
    {
        public string parametro_id { get; set; }
    }

    public class TelemetriaGetLimite
    {
        public int id_limite { get; set; }
        public string nombre_limite { get; set; }
        public IEnumerable<TelemetriaParametroLimite> parametros { get; set; }
    }

    public class TelemetriaParametroLimite
    {
        public int parametro_id { get; set; }
        public string nombre_parametro { get; set; }
        public string limite_inferior { get; set; }
        public string limite_superior { get; set; }
    }
}
