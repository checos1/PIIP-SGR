using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RegistroAuditoriaDto
    {
        public string Id { get; set; }

        public string Ip { get; set; }

        public string EntidadOrigen { get; set; }

        public string Aplicacion { get; set; }

        public string Usuario { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string TipoEvento { get; set; }

        public string TipoMensaje { get; set; }

        public string ContenidoMensaje { get; set; }

        public string CodigoHash { get; set; }
    }
}
