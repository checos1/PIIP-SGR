using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    [ExcludeFromCodeCoverage]
    public class ProyectoInstanciaDto
    {
        public Guid InstanciaProyecto { get; set; }
        public int? EstadoInstanciaid { get; set; }
        public string ObjetoNegocioId { get; set; }
        public Guid? InstanciaPadreId { get; set; }
        public string TipoObjeto { get; set; }
        public Guid Flujoid { get; set; }
    }
}
