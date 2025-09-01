using System.Diagnostics.CodeAnalysis;


namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    public class VigenciaRegionalizacionDto
    {
        public int? Vigencia{ get; set; }
        public List<MesDto> Mes{ get; set; }
        
    }
}
