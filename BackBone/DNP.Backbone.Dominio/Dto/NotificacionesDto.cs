using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class NotificacionesDto
    {
        public int IdNotificacion { get; set; }

        public string Asunto { get; set; }

        public string Mensaje { get; set; }

        public string Estado { get; set; }

        public string Fecha { get; set; }

        public string UsuarioResponsable { get; set; }

    }
}
