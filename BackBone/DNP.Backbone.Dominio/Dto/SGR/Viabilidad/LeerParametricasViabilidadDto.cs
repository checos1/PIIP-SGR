using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SGR.Viabilidad
{
    [ExcludeFromCodeCoverage]
    public class LeerParametricasViabilidadDto
    {
        public List<ListaGenericaDto> RegionesSgr { get; set; }
        public List<ListaGenericaDto> CategoriasSgr { get; set; }
        public List<ListaGenericaDto> Sectores { get; set; }
    }
}
