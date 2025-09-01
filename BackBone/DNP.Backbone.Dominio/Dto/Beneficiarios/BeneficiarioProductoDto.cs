using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Beneficiarios
{
    public class BeneficiarioProductoDto
    {
        public int ProyectoId { get; set; }
        public int ProductoId { get; set; }
        public int InterventionLocalizationTypeId { get; set; }
        public int PersonasBeneficiaros { get; set; }
        public bool EsAcumulable { get; set; }
        public List<DetalleLocalizacion> ListaDetalleLocalizacion { get; set; }
    }

    public class DetalleLocalizacion
    {
        public int Id { get; set; }
    }
}
