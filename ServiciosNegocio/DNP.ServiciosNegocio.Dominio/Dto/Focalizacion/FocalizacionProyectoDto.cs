using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class FocalizacionProyectoDto
    {
        public string Bpin { get; set; }
        public List<VigenciaFocalizacionDto> Vigencias { get; set; }
    }
}
