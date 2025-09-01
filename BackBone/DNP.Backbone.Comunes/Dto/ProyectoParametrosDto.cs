namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;    
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProyectoParametrosDto
    {
        public string Aplicacion { get; set; }
        public List<Guid> ListaIdsRoles { get; set; }
        public string IdUsuario { get; set; }
        public Guid IdObjeto { get; set; }
        public Guid? InstanciaId { get; set; }
        public string IdFiltro { get; set; }
    }

    public class ListMatrizEntidadDestinoDto
    {
        public List<MatrizEntidadParametrosDto> ListMatrizEntidad { get; set; }
        public string IdUsuario { get; set; }
    }

    public class MatrizEntidadParametrosDto
    {
        public int EntidadResponsableId { get; set; }
        public int ResourceGroupId { get; set; }
        public List<EntidadDestinoIdDto> ListEntidadDestinoId { get; set; }
        public List<SectorIdDto> ListSectorId { get; set; }
    }

    public class EntidadDestinoIdDto
    {
        public int EntidadDestinoId { get; set; }
    }

    public class SectorIdDto
    {
        public int SectorId { get; set; }
    }

    public class ListaMatrizEntidadUnidadDto
    {
        public List<MatrizEntidadUnidadDto> MatrizEntidadUnidad { get; set; }
        public Guid TipoFlujo { get; set; }
    }
    public class MatrizEntidadUnidadDto
    {
        public int Id { get; set; }
        public int CRTypeId { get; set; }
        public int EntidadResponsableId { get; set; }
        public string EntidadResponsable { get; set; }
        public int SectorId { get; set; }
        public string Sector { get; set; }
        public Guid RolId { get; set; }
        public string Rol { get; set; }
        public int EntidadDestinoId { get; set; }
        public string EntidadDestinoAccion { get; set; }
        public int Estado { get; set; } //0-Actual , 1-Adicionado, 2-Eliminado

    }

    public class ConfiguracionUnidadMatrizDTO
    {
        public List<MatrizEntidadUnidadDto> Respuesta { get; set; }
        public int TipoFlujo { get; set; }
        public int EntidadResponsableId { get; set; }
        public Guid FlowId { get; set; }
    }

}
