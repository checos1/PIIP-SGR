using System.ComponentModel.DataAnnotations;

namespace DNP.Backbone.Dominio.Enums
{
    public enum TipoConfigNotificacion
    {
        [Display(Name = "No Prioritario")]
        NoPrioritario = 1,
        [Display(Name = "Prioritario")]
        Prioritario = 2,
        [Display(Name = "Urgente")]
        Urgente = 3
    }
}
