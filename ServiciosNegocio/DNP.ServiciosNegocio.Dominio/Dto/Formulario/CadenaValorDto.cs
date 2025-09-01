using System.Collections.Generic;


namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CadenaValorDto
    {
        public string Bpin { get; set; }
        public List<VigenciaCadenaValorDto> Vigencias { get; set; }
    }
}
