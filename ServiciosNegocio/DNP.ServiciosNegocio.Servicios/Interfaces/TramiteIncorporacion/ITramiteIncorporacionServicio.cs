namespace DNP.ServiciosNegocio.Servicios.Interfaces.TramiteIncorporacion
{
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;

    public interface ITramiteIncorporacionServicio
    {
        /// <summary>
        ///  Funcion para Obtener los Datos Incorporacion
        /// </summary>
        /// <param name="TramiteId"></param>
        /// <returns></returns>
        string ObtenerDatosIncorporacion(int TramiteId);

        /// <summary>
        ///  Funcion para Registrar los Datos Incorporacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        RespuestaGeneralDto GuardarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario);

        /// <summary>
        ///  Funcion para eliminar los Datos Incorporacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>

        RespuestaGeneralDto EiliminarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario);

    }
}
