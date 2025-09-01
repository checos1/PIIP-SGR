using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl
{
    public class DesagregarEdtServicio: IDesagregarEdtServicio
    {
        private readonly IDesagregarEdtPersistencia _DesagregarEdtPersistencia;

        #region Constructor

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public DesagregarEdtServicio(IDesagregarEdtPersistencia DesagregarEdtPersistencia)
        {
            _DesagregarEdtPersistencia = DesagregarEdtPersistencia;
        }

        #endregion

        public Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
        {
            var listado = _DesagregarEdtPersistencia.ObtenerListadoObjProdNiveles(ProyectosDto);
            if (listado == null || listado.Length < 0) return Task.FromResult<DesagregarEdtNivelesDto>(null);
            var listadoDto = DesagregarEdtNivelesDto.CrearListadoObjProdNiveles(listado);
            return Task.FromResult(listadoDto);
        }

        public Task<ReponseHttp> RegistrarNivel(string usuario, RegistroModel registroNuevo)
        {
            try
            {
                if(registroNuevo.Tipo == "Actividad") _DesagregarEdtPersistencia.RegistrarActividad(usuario, registroNuevo.NivelesNuevos);
                else _DesagregarEdtPersistencia.RegistrarNivel(usuario, registroNuevo.NivelesNuevos);
                
                return Task.FromResult<ReponseHttp>(new ReponseHttp() {Status = true});
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { 
                    Status = false,
                    Message= e.Message
                }) ;
            }
        }

        public Task<ReponseHttp> EliminarNivel(string usuario, RegistroModel registroNuevo)
        {
            try
            {
                _DesagregarEdtPersistencia.EliminarActividad(usuario, registroNuevo.NivelesNuevos);
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public string ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid)
        {
            return _DesagregarEdtPersistencia.ObtenerPreguntasAvanceFinanciero(instancia, proyectoid, bpin, nivelid);
        }

        public string GuardarPreguntasAvanceFinanciero(ParametrosGuardarDto<List<PreguntasReporteAvanceFinancieroDto>> parametrosGuardar, string usuario)
        {
            return _DesagregarEdtPersistencia.GuardarPreguntasAvanceFinanciero(parametrosGuardar, usuario);
        }

        public string ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId)
        {
            return _DesagregarEdtPersistencia.ObtenerAvanceFinanciero(instancia, proyectoid, bpin, vigenciaId, periodoPeriodicidadId);
        }

        public string GuardarAvanceFinanciero(ParametrosGuardarDto<AvanceFinancieroDto> parametrosGuardar, string usuario)
        {
            return _DesagregarEdtPersistencia.GuardarAvanceFinanciero(parametrosGuardar, usuario);
        }
    }
}
