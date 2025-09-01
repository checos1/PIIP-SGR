using Microsoft.SharePoint.Client.Publishing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversales
{
    [ExcludeFromCodeCoverage]
    public class DocumentosDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public string Origen { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodoId { get; set; }
        public string Periodo { get; set; }
        public string OrigenCompleto { get; set; }
        public string ProcesoOrigen { get; set; }
        public string CargadoEn { get; set; }
        public int? Ficha { get; set; }
        public int? Tramite { get; set; }
        public Nullable<DateTime> FechaRegistroSolicitud { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string FechaCreacion { get; set; }
        public string DocumentoDatos { get; set; }
        public string Descripcion { get; set; }
        public string UrlArchivo { get; set; }
        public bool Seleccionado { get; set; }
        public string idArchivoBlob { get; set; }
        public string ContenType { get; set; }
        public string NumeroProceso { get; set; }
    }
}
