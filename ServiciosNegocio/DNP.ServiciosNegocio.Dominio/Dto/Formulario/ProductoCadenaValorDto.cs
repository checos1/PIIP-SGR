using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class ProductoCadenaValorDto
    {
        public int? Id { get; set; }
        public int? CatalogoProductoId { get; set; }

        public string Nombre { get; set; }
        public int? TipoMedidaId { get; set; }
        public decimal? Cantidad { get; set; }

        public List<ActividadCadenaValorDto> Actividad{ get; set; }
    }
}