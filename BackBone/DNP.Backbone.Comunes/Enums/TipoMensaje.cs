using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Enums
{
   [SuppressMessage("ReSharper", "InconsistentNaming")]
   public enum TipoMensaje
    {
        ADVERTENCIA,
        DEPURACION,
        ERROR,
        EXCEPCION,
        INFORMACION,
        CONSULTA,
        CREACION,
        ELIMINACION,
        MODIFICACION,
        SEGUIMIENTO
    }
}
