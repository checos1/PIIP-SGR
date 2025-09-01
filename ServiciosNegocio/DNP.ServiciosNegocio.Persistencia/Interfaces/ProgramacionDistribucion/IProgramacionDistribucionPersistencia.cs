using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.ProgramacionDistribucion
{
    public interface IProgramacionDistribucionPersistencia
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
