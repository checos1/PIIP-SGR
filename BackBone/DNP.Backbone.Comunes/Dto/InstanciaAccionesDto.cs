using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class InstanciaAccionesDto
    {
        public string IdInstancia { get; set; }
        public string IdObjeto { get; set; }
        public string IdObjetoNegocio { get; set; }
        public Guid IdTipoObjeto { get; set; }
        public int? IdEntidad { get; set; }
        public Guid IdAccion { get; set; }
        public string EstadoAccion { get; set; }
        public Guid IdRol { get; set; }
        public int PeriodoValidez { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaLimite { get; set; }
        public string EstadoInstancia { get; set; }
    }
}
