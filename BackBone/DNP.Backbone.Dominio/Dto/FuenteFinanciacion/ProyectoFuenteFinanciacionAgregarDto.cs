namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    using System.Collections.Generic;

    public class ProyectoFuenteFinanciacionAgregarDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int? CR { get; set; }

        public List<FuenteFinanciacionAgregarDto> FuentesFinanciacionAgregar { get; set; }
    }
}
