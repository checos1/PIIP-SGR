namespace DNP.Backbone.Dominio.Dto.Monitoreo
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GrupoEntidadProyectoResumenDto
    {
        public string TipoEntidad { get; set; }
        public ICollection<EntidadProyectoResumenDto> ListaEntidades { get; set; }
    }
}
