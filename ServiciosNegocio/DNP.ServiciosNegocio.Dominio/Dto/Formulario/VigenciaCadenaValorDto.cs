namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class VigenciaCadenaValorDto
    {
        public int? Vigencia { get; set; }
        public List<MesCadenaValorDto> Mes { get; set; }
    }
}
