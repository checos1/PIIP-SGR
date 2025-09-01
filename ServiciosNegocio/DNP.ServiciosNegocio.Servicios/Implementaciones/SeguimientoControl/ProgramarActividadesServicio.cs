using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
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
    public class ProgramarActividadesServicio: IProgramarActividadesServicio
    {
        private readonly IProgramarActividadesPersistencia _ProgramarActividadesPersistencia;

        #region Constructor

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public ProgramarActividadesServicio(IProgramarActividadesPersistencia ProgramarActividadesPersistencia)
        {
            _ProgramarActividadesPersistencia = ProgramarActividadesPersistencia;
        }

        #endregion

        public Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
        {
            var listado = _ProgramarActividadesPersistencia.ObtenerListadoObjProdNiveles(ProyectosDto);
            if (listado == null || listado.Length < 0) return Task.FromResult<DesagregarEdtNivelesDto>(null);
            var listadoDto = DesagregarEdtNivelesDto.CrearListadoObjProdNiveles(listado);
            return Task.FromResult(listadoDto);
        }

        public Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto)
        {
            var listado = _ProgramarActividadesPersistencia.ObtenerListadoObjProdNivelesXReporte(ProyectosDto);
            if (listado == null || listado.Length < 0) return Task.FromResult<DesagregarEdtNivelesDto>(null);
            var periodos = _ProgramarActividadesPersistencia.ObtenerCalendarioPeriodo(ProyectosDto);
            var listadoDto = DesagregarEdtNivelesDto.CrearListadoObjProdNivelesXReporte(listado, periodos);
            return Task.FromResult(listadoDto);
        }

        public Task<DesagregarIndicadoresPoliticasDto> ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto)
        {
            var listado = _ProgramarActividadesPersistencia.ObtenerIndicadoresPoliticas(ProyectosDto);
            if (listado == null || listado.Length < 0) return Task.FromResult<DesagregarIndicadoresPoliticasDto>(null);
            var periodos = _ProgramarActividadesPersistencia.ObtenerCalendarioPeriodo(ProyectosDto);
            var listadoDto = DesagregarIndicadoresPoliticasDto.CrearListadoObtenerIndicadoresPoliticas(listado, periodos);
            return Task.FromResult(listadoDto);
        }

        public Task<List<CalendarioPeriodoDto>> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto)
        {
            var listado = _ProgramarActividadesPersistencia.ObtenerCalendarioPeriodo(ProyectosDto);
            if (listado == null || listado.Count < 0) return Task.FromResult<List<CalendarioPeriodoDto>>(null);
            return Task.FromResult(listado);
        }

        public Task<ReponseHttp> EditarProgramarActividad(string usuario, ProgramarActividadesDto ProyectosDto)
        {
            try
            {
                _ProgramarActividadesPersistencia.EditarProgramarActividad(usuario, ProyectosDto);
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

        public Task<ReponseHttp> ActividadProgramacionSeguimientoPeriodosValores(string usuario, List<VigenciaEntregableDto> parametros)
        {
            try
            {
                _ProgramarActividadesPersistencia.ActividadProgramacionSeguimientoPeriodosValores(usuario, parametros);
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

        public Task<ReponseHttp> ActividadReporteSeguimientoPeriodosValores(string usuario, ReporteSeguimiento parametros)
        {
            try
            {
                _ProgramarActividadesPersistencia.ActividadReporteSeguimientoPeriodosValores(usuario, parametros);
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

        public Task<ReponseHttp> IndicadorPoliticaSeguimientoPeriodosValores(string usuario, ReporteIndicadorPoliticas parametros)
        {
            try
            {
                _ProgramarActividadesPersistencia.IndicadorPoliticaSeguimientoPeriodosValores(usuario, parametros);
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

        public string ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta)
        {
            return _ProgramarActividadesPersistencia.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta);

        }

        public string GuardarFocalizacionProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionProgramacionSeguimientoDto> parametrosGuardar, string usuario)
        {
            return _ProgramarActividadesPersistencia.GuardarFocalizacionProgramacionSeguimiento(parametrosGuardar, usuario);
        }

        public string ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid)
        {
            return _ProgramarActividadesPersistencia.ObtenerCruceProgramacionSeguimiento(instanciaid, proyectoid);

        }

        public string GuardarCruceProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionCrucePoliticaSeguimientoDto> parametrosGuardar, string usuario)
        {
            return _ProgramarActividadesPersistencia.GuardarCruceProgramacionSeguimiento(parametrosGuardar, usuario);
        }

        public string ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros)
        {
            return _ProgramarActividadesPersistencia.ObtenerFocalizacionProgramacionSeguimientoDetalle(parametros);
        }

    }
}
