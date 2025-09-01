using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto
{
    [ExcludeFromCodeCoverage]
    public class FocalizacionProyectoDto
    {
        public string Bpin { get; set; }

        public int? ProyectoId { get; set; }
        public List<PoliticaDto> Politicas { get; set; }
    }
}
