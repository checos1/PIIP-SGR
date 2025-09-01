
namespace DNP.Backbone.Comunes.Enums
{
    using System.Diagnostics.CodeAnalysis;

    public enum TipoCabecerasPeticion
    {

        Authorization,
        Ocp_Apim_Subscription_Key,
        Ocp_Apim_Trace,
        piip_idInstanciaFlujo,
        piip_idAccion,
        piip_idFormulario,
        [SuppressMessage("ReSharper", "InconsistentNaming")] Aeg_sas_key //Se excluye debido a que se convierten estos nombres a los nombres usados en la peticion http
    }
}
