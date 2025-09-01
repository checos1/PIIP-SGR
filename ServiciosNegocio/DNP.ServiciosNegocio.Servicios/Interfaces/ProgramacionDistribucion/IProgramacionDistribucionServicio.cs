using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.ProgramacionDistribucion
{
    public interface IProgramacionDistribucionServicio
    {
        /// <summary>
        ///  Funcion para Obtener los Datos Reprogramacion
        /// </summary>
        /// <param name="EntidadDestinoId"></param>
        /// <param name="TramiteId"></param>
        /// <returns></returns>
        string ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId);

        /// <summary>
        ///  Funcion para Registrar los Datos Reprogramacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario);
        string ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid);
        string ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId);
        TramitesResultado GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario);
    }
}
