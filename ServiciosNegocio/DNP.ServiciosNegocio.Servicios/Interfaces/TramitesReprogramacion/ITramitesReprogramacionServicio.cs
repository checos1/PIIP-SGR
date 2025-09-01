namespace DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion
{
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
    using System;

    public interface ITramitesReprogramacionServicio
    {
        /// <summary>
        ///  Funcion para Obtener los Datos Reprogramacion
        /// </summary>
        /// <param name="InstanciaId"></param>
        /// <param name="ProyectoId"></param>
        /// <param name="TramiteId"></param>
        /// <returns></returns>
        string ObtenerResumenReprogramacionPorVigencia(Guid? InstanciaId, int ProyectoId, int TramiteId);

        /// <summary>
        ///  Funcion para Registrar los Datos Reprogramacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        TramitesResultado GuardarDatosReprogramacion(ParametrosGuardarDto<DatosReprogramacionDto> parametrosGuardar, string usuario);

        /// <summary>
        ///  Funcion para obtener los datos reprogramacion por producto vigencia
        /// </summary>
        /// <param name="InstanciaId"></param>
        /// <param name="ProyectoId"></param>
        /// <param name="TramiteId"></param>
        /// <returns></returns>
        string ObtenerResumenReprogramacionPorProductoVigencia(Guid? InstanciaId, int? ProyectoId, int TramiteId);
    }
}
