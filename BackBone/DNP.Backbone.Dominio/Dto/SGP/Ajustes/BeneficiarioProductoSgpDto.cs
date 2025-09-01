using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGP.Ajustes
{
  public   class BeneficiarioProductoSgpDto
    {
        public int ProyectoId { get; set; }
        public int ProductoId { get; set; }
        public int InterventionLocalizationTypeId { get; set; }
        public int PersonasBeneficiaros { get; set; }
        public bool EsAcumulable { get; set; }
        public List<DetalleLocalizacionSgp> ListaDetalleLocalizacion { get; set; }


    }

    public class DetalleLocalizacionSgp
    {
        public int Id { get; set; }

        public string justificacion { get; set; }
    }
}
