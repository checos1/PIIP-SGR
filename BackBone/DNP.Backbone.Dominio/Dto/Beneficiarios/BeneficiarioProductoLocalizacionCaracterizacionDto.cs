using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Beneficiarios
{
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
