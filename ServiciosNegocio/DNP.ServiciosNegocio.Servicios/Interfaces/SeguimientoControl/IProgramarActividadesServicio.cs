using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl
{
    public interface IProgramarActividadesServicio
    {
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto);
        Task<DesagregarIndicadoresPoliticasDto> ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto);
        Task<List<CalendarioPeriodoDto>> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto);
        Task<ReponseHttp> EditarProgramarActividad(string usuario, ProgramarActividadesDto ProyectosDto);
        Task<ReponseHttp> ActividadProgramacionSeguimientoPeriodosValores(string usuario, List<VigenciaEntregableDto> parametros);
        Task<ReponseHttp> ActividadReporteSeguimientoPeriodosValores(string usuario, ReporteSeguimiento parametros);
        Task<ReponseHttp> IndicadorPoliticaSeguimientoPeriodosValores(string usuario, ReporteIndicadorPoliticas parametros);
        string ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta);
        string GuardarFocalizacionProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionProgramacionSeguimientoDto> parametrosGuardar, string name);
        string ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid);
        string GuardarCruceProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionCrucePoliticaSeguimientoDto> parametrosGuardar, string name);
        string ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros);
    }
}
