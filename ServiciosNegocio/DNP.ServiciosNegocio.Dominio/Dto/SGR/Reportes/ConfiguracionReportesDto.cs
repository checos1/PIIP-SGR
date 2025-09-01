using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Reportes
{
    public class ConfiguracionReportesDto
    {
        public string NombreRdl { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> TipoDocumentoId { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string Coleccion { get; set; }
        public string NombreDocumento { get; set; }
        public string NombrePaso {  get; set; }
    }
}
