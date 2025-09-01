using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class AjustesUbicacionDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<MGAUbicacionDto> Localizacion { get; set; }
        public List<NuevaUbicacionDto> NuevaLocalizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class MGAUbicacionDto
    {
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class NuevaUbicacionDto
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
    }
}
