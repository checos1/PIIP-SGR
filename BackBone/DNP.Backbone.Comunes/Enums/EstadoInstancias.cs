using System.ComponentModel;

namespace DNP.Backbone.Comunes.Enums
{
    public enum EstadoInstancias
    {
        [Description("Activo")]
        Activo = 1,
        [Description("Anulado por alcance")]
        AnuladoPorAlcance = 2,
        [Description("Completado")]
        Completado = 3,
        [Description("Pausado")]
        Pausado = 4,
        [Description("Cancelado")]
        Cancelado = 5,
        [Description("Anulado")]
        Anulado = 6
    }
}
