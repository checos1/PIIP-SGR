
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ProyectoRequisitoDto
    {
        public int Id { get; set; }
        public int TramiteProyectoId { get; set; }
        public int TipoRequisitoId { get; set; }
        public string Numero { get; set; }
        public string NumeroContrato { get; set; }
        public DateTime? Fecha { get; set; }
        public string Descripcion { get; set; }
        public string UnidadEjecutora { get; set; }

        public IEnumerable<TipoRequisitoDto> ListaTiposRequisito { get; set; }

      

    }
}
