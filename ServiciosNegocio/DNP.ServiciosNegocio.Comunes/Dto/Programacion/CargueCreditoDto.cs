using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Programacion
{
    public class CargueCreditoDto
    {
        public int? Id { get; set; }
        public int EntityTypeCatalogOptionId { get; set; }
        public string CodigoEntidad { get; set; }
        public string Entidad { get; set; }
        public string Codigo { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstadoCredito { get; set; }        
        public string NombreCredito { get; set; }
        public decimal Monto { get; set; }
        public int Vigencia { get; set; }
        public int TipoId { get; set; }
        public string TipoCredito { get; set; }
    }
}
