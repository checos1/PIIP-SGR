using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class ProyectoTramiteFuenteDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }

        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public string Operacion { get; set; }
        public decimal ValorTotalNacion { get; set; }
        public decimal ValorTotalPropios { get; set; }
        public int TramiteProyectoId { get; set; }
      

        public List<FuenteFinanciacionDto> ListaFuentes { get; set; }

       
    }
}
