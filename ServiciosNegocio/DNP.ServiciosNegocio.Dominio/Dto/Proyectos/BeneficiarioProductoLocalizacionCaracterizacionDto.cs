using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class BeneficiarioProductoLocalizacionCaracterizacionDto
    {
        public int ProyectoId { get; set; }
        public int ProductoId { get; set; }
        public int LocalizacionId { get; set; }
        public int Vigencia { get; set; }
        public int ProductoLocalizacionProgramacionId { get; set; }
        public List<DetalleCaracteristicas> DetalleCaracteristicas { get; set; }
    }

    public class DetalleCaracteristicas
    {
        public int CharacteristicTypeId { get; set; }
        public int ValorCaracteristica { get; set; }
        public string FuenteInformacion { get; set; }
    }
}
