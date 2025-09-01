using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System;
    using System.Collections.Generic;
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
