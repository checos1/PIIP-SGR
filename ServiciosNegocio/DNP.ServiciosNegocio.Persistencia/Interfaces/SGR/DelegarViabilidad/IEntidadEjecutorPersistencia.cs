using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad
{
    public interface IEntidadEjecutorPersistencia
    {
        /// <summary>
        /// Permite obtener el listado de ejecutores de acuerdo a la busqueda indicada
        /// </summary>
        /// <param name="nit"></param>
        /// <param name="tipoEntidadId"></param>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        List<EjecutorEntidadDto> ObtenerListadoEjecutores(string nit, int? tipoEntidadId, int? entidadId);

        /// <summary>
        /// Obtener Listado de Ejecutores Asociados
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        List<EjecutorEntidadAsociado> ObtenerListadoEjecutoresAsociados(int proyectoId);

        /// <summary>
        /// Permite guardar el ejecutor asociado
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <param name="ejecutorId"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoEjecutorId"></param>
        /// <returns></returns>
        bool CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId);

        /// <summary>
        /// Permite eliminar a un Ejecutor Asociado
        /// </summary>
        /// <param name="EjecutorAsociadoId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        SeccionesEjecutorEntidad EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario);

    }
}

