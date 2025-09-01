namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Comunes.Dto;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class EntidadProyectoVerificacionOcadPazDto
    {

        public int IdEntidad { get; set; }

        public string NombreEntidad { get; set; }
        public string TipoEntidad { get; set; }

        public List<NegocioVerificacionOcadPazDto> ObjetosNegocio { get; set; }
        public RolAutorizacionDto Rol { get; set; }
    }
}
