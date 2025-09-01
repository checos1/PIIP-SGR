using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class FuenteFinancianciacionDto
    {
        public int Id { get; set; }
        public int? TipoGastoId { get; set; }
        public int? SubPrograma { get; set; }
        public List<FuenteDto> Fuente { get; set; }
        public int? ProyectoId { get; set; }
    }
}
