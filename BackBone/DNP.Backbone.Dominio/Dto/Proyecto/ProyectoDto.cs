namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    using DNP.Backbone.Dominio.Dto.Inbox;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ProyectoDto
    {
        public string Mensaje { get; set; }
        public string[] ColumnasVisibles { get; set; }
        public List<GrupoEntidadProyectoDto> GruposEntidades { get; set; }
    }
}
