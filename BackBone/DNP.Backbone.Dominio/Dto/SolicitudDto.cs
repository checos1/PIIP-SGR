using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SolicitudDto
    {
        public int Idsolicitud { get; set; }

        public string TipoSolicitud { get; set; }

        public string Prioridad { get; set; }

        public string Descripcion { get; set; }

        public string UsuarioResponsable { get; set; }
        
        public int EntidadId { get; set; }


    }
}
