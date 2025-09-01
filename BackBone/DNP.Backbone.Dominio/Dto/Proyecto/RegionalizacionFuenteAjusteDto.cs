using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class RegionalizacionFuenteAjusteDto
    {
        public string Bpin { get; set; }
        public int ProductoId { get; set; }
        public int FuenteId { get; set; }
        public int LocalizacionId { get; set; }
        public string Vigencia { get; set; }
        public int PeriodoProyectoId { get; set; }
        public double TotalFuene { get; set; }
        public double TotalRegionalizadoFuente { get; set; }
        public double TotalCostoProducto { get; set; }
        public double TotalRegionalizadoProducto { get; set; }
        public double EnAjuste { get; set; }
        public double MetaEnAjuste { get; set; }
    }
}
