using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class ObjetivoEspecificoCadenaValorDto
    {
        public int? Id { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductoCadenaValorDto> Productos { get; set; }
    }
}