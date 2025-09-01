using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto
{
    [ExcludeFromCodeCoverage]
    public class DimensionDto
    {
        public int? DimensionId{ get; set; }
        public string Dimension{ get; set; }
         public int? FocalizacionProyectoId { get; set; }


    }
}
