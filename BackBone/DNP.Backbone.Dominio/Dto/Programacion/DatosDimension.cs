using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class DatosDimension
    {
        public int DimensionId { get; set; }
        public List<DatosDistribucion> DatosDistribucion { get; set; }

    }
}