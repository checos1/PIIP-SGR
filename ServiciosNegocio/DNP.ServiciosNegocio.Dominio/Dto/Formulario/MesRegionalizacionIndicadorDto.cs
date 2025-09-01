using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using Formulario;
    [ExcludeFromCodeCoverage]
    public class MesRegionalizacionIndicadorDto
    {
        public int? Mes { get; set; }
        public string NombreMes { get; set; }
        public List<ObjetivoEspecificoProductosIndicadorDto> Objetivos { get; set; }
    }
}
