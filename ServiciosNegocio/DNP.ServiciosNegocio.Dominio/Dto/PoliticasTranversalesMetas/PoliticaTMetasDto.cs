using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas
{
    public class PoliticaTMetasDto
    {
        public string BPIN { get; set; }
        public int ProyectoId { get; set; }
        public List<POLITICA> POLITICAS { get; set; }
    }
    public class POLITICA
    {
        public string PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<Productos> Productos { get; set; }
    }
    public class Productos
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int IndicadorId { get; set; }
        public string Indicador { get; set; }
        public int UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public double MetaTotalProducto { get; set; }
        public List<Vigencia> Vigencias { get; set; }
    }

    public class Vigencia
    {
        public int VIGENCIA { get; set; }
        public int ProgramacionIndicadorId { get; set; }
        public double MetaTotalProductoVigencia { get; set; }
        public double MetaTotalProductoPolitica { get; set; }
        public List<Localizacione> Localizaciones { get; set; }
    }

    public class Localizacione
    {
        public int LocalizacionId { get; set; }
        public string Localizacion { get; set; }
        public int PeriodoProyectoId { get; set; }
        public double MetaProductoLocalizacion { get; set; }
        public double MetaProductoPolitica { get; set; }
    }
}
