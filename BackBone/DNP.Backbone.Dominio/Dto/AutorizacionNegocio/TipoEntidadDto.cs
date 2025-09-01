using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TipoEntidadDto
    {
        public TipoEntidadDto()
        {
            EntidadesTerritoriales = new HashSet<EntidadNegocioDto>();
            Roles = new HashSet<RolNegocioDto>();
            Sectores = new HashSet<SectorNegocioDto>();
            EntidadesDestino = new HashSet<EntidadNegocioDto>();
            Configuraciones = new HashSet<RolNegocioEntidadDestinoDto>();
        }

        public string TipoEntidad { get; set; }
        public ICollection<EntidadNegocioDto> EntidadesTerritoriales { get; set; }
        public ICollection<RolNegocioDto> Roles { get; set; }
        public ICollection<SectorNegocioDto> Sectores { get; set; }
        public ICollection<EntidadNegocioDto> EntidadesDestino { get; set; }
        public ICollection<RolNegocioEntidadDestinoDto> Configuraciones { get; set; }
    }
}
