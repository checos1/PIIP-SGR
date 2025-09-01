using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class ProyectoTramiteFuenteDto
    {
        public int Id { get; set; }

        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public string Operacion { get; set; }
        public decimal ValorTotalNacion { get; set; }
        public decimal ValorTotalPropios { get; set; }
        public int TramiteProyectoId { get; set; }
        public int ProyectoId { get; set; }



        public List<FuenteFinanciacionDto> ListaFuentes { get; set; }
    }
}
