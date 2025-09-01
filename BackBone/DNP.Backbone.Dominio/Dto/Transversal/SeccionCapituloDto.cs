using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SeccionCapituloDto
    {
        public int SeccionId { get; set; }
        public int CapituloId { get; set; }
        public int SeccionCapituloId { get; set; }
        public string Macroproceso { get; set; }
        public Guid? Instancia { get; set; }
        public string Seccion { get; set; }
        public string SeccionModificado { get; set; }
        public string Capitulo { get; set; }
        public string CapituloModificado { get; set; }
        public string NombreModificado { get; set; }
        public bool? Modificado { get; set; }
        public string Justificacion { get; set; }
        public string nombreComponente { get; set; }
    }
}
