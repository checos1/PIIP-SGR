namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GrupoEntidadProyectoVerificacionOcadPazDto
    {
        public string TipoEntidad { get; set; }
        public ICollection<EntidadProyectoVerificacionOcadPazDto> ListaEntidades { get; set; }
    }
}
