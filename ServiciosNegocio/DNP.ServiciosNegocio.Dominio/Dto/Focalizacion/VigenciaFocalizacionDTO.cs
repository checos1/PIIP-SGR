using System.Diagnostics.CodeAnalysis;


namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    public class VigenciaFocalizacionDto
    {
        public int? Vigencia{ get; set; }
        public List<FuenteDto> Fuentes{ get; set; }
        
    }
}
