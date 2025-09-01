using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Beneficiarios
{
    public class BeneficiarioProductoLocalizacionDto
    {
        public int ProyectoId { get; set; }
        public int ProductoId { get; set; }
        public int LocalizacionId { get; set; }
        public List<DetalleVigencias> DetalleVigencias { get; set; }
    }

    public class DetalleVigencias
    {
        public int PeriodoProyectoId { get; set; }
        public int ValorActual { get; set; }
    }
}
