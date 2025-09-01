using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.DelegarViabilidad
{
    public class EjecutorEntidadServicio : IEjecutorEntidadServicio
    {
        private readonly IEntidadEjecutorPersistencia _entidadEjecutorPersistencia;

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="entidadEjecutorPersistencia">entidadEjecutorPersistencia</param>        
        public EjecutorEntidadServicio(IEntidadEjecutorPersistencia entidadEjecutorPersistencia)
        {
            _entidadEjecutorPersistencia = entidadEjecutorPersistencia;
        }

        /// <summary>
        /// Obtiene el listado de ejecutores por la busqueda indicada
        /// </summary>
        /// <param name="nit"></param>
        /// <param name="tipoEntidadId"></param>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        public Task<List<EjecutorEntidadDto>> ConsultarListadoEjecutores(string nit, int? tipoEntidadId, int? entidadId)
        {
            var ejecutorList = new List<EjecutorEntidadDto>();

            ejecutorList = _entidadEjecutorPersistencia.ObtenerListadoEjecutores(nit, tipoEntidadId, entidadId);

            if (ejecutorList.Count == 0) return Task.FromResult<List<EjecutorEntidadDto>>(null);
            return Task.FromResult(ejecutorList);
        }

        public Task<bool> CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId)
        {
            var ejecutorList = false;

            ejecutorList = _entidadEjecutorPersistencia.CrearEjecutorAsociado(proyectoId, ejecutorId, usuario, tipoEjecutorId);

            if (ejecutorList)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }

        public Task<SeccionesEjecutorEntidad> EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario)
        {
            var ejecutorList = new SeccionesEjecutorEntidad();

            ejecutorList = _entidadEjecutorPersistencia.EliminarEjecutorAsociado(EjecutorAsociadoId, usuario);

            return Task.FromResult(ejecutorList);
        }

        public Task<List<EjecutorEntidadAsociado>> ObtenerListadoEjecutoresAsociados(int proyectoId)
        {
            var ejecutorList = new List<EjecutorEntidadAsociado>();

            ejecutorList = _entidadEjecutorPersistencia.ObtenerListadoEjecutoresAsociados(proyectoId);

            if (ejecutorList.Count == 0) return Task.FromResult<List<EjecutorEntidadAsociado>>(null);
            return Task.FromResult(ejecutorList);
        }
    }
}
