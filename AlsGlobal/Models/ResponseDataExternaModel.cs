using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using AlsGlobal.Interfaces;

namespace AlsGlobal.Models
{
    public class GetInfoModel
    {
        public List<Parametro> Parametros { get; set; }
        public List<Matriz> Matrices { get; set; }
        public List<TipoMuestra> tipo_muestras { get; set; }
        public List<Estacion> Estaciones { get; set; }
        public List<Proyecto> Proyectos { get; set; }
        public List<Empresa> Empresas { get; set; }

        // Define clases adicionales según sea necesario
        public class Parametro
        {
            public string Id { get; set; }
            public string nombre_parametro { get; set; }
        }

        public class Matriz
        {
            public string Id { get; set; }
            public string nombre_matriz { get; set; }
        }

        public class TipoMuestra
        {
            public string Id { get; set; }
            public string nombre_tipo_muestra { get; set; }
        }
        public class Estacion
        {
            public string Id { get; set; }
            public string nombre_estacion { get; set; }
        }
        public class Proyecto
        {
            public string Id { get; set; }
            public string nombre_proyecto { get; set; }
        }
        public class Empresa
        {
            public string Id { get; set; }
            public string nombre_empresa { get; set; }
        }
    }

    public class GetDataExternaMuestra
    {
        public int fila { get; set; }
        public int id { get; set; }
        public int id_user { get; set; }
        public int id_muestra { get; set; }
        public string fecha_muestreo { get; set; }
        public string id_matriz { get; set; }
        public string matriz { get; set; }
        public string id_tipo_muestra { get; set; }
        public string tipo_muestra { get; set; }
        public string id_proyecto { get; set; }
        public string proyecto { get; set; }
        public string id_estacion { get; set; }
        public string estacion { get; set; }
        public string id_empresa_contratante { get; set; }
        public string empresa_contratante { get; set; }
        public string id_empresa_solicitante { get; set; }
        public string empresa_solicitante { get; set; }
        public string id_parametro { get; set; }
        public string parametro { get; set; }
        public string valor { get; set; }
        public string unidad { get; set; }
    }

    public class agregarEstacionDataExternaModel {
        public string id_empresa_sol { get; set; }
        public string id_empresa_con { get; set; }
        public string nombre_estacion { get; set; }
        public string latitud_n { get; set; }
        public string longitud_e { get; set; }
        public string zona { get; set; }
        public string procedencia { get; set; }
    }

    public class agregarProyectoDataExternaModel
    {
        public string id_empresa_sol { get; set; }
        public string id_empresa_con { get; set; }
        public string nombre_proyecto { get; set; }
    }
}
