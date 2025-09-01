

using System;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    public class DetalleTramiteDto
    {
        public int TramiteId { get; set; }

        public int SectorId { get; set; }

        public string NombreSector { get; set; }

        public int EntidadId { get; set; }

        public string NombreEntidad { get; set; }

        public int TipoTramiteId { get; set; }

        public string TipoTramite { get; set; }

        public string CodigoTipoTramite { get; set; }

        public string Descripcion { get; set; }

        public string DescripcionInstancia { get; set; }

        public Guid InstanciaId { get; set; }

        public int? CodigoDocumental { get; set; }
        public string PDF { get; set; }
    }
}
