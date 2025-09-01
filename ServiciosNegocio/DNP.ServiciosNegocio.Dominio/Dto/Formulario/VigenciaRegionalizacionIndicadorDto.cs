using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;
    [ExcludeFromCodeCoverage]
    public class VigenciaRegionalizacionIndicadorDto
    {
        public int? Vigencia { get; set; }
        public List<MesRegionalizacionIndicadorDto> Mes { get; set; }
    }
}
