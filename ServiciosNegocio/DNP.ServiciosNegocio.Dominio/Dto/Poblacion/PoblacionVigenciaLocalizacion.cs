using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Poblacion
{
    [ExcludeFromCodeCoverage]
    public class PoblacionVigenciaLocalizacion
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string NombreRegion { get; set; }
        public string CodigoRegion { get; set; }
        public int? DepartamentoId { get; set; }
        public string NombreDepartamento { get; set; }
        public string CodigoDepto { get; set; }
        public int? MunicipioId { get; set; }
        public string NombreMunicipio { get; set; }
        public string CodigoMunicipio { get; set; }
        public int? AgrupacionId { get; set; }
        public string NombreAgrupacion { get; set; }
        public string CodigoAgrupacion { get; set; }
        public decimal? CantidadLocalizada { get; set; }
    }
}
