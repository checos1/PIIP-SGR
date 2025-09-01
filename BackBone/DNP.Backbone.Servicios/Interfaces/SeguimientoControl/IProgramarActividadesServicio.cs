using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IProgramarActividadesServicio
    {
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        Task<List<CalendarioPeriodoDto>> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto);
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto);
        Task<DesagregarIndicadoresPoliticasDto> ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto);
        Task<ReponseHttp> EditarProgramarActividad(string Usuario, ProgramarActividadesDto ProyectosDto);
        Task<ReponseHttp> ActividadProgramacionSeguimientoPeriodosValores(string Usuario, List<VigenciaEntregable> parametros);
        Task<ReponseHttp> ActividadReporteSeguimientoPeriodosValores(string Usuario, ReporteSeguimiento parametros);
        Task<ReponseHttp> IndicadorPoliticaSeguimientoPeriodosValores(string Usuario, ReporteIndicadorPoliticas parametros);
        Task<string> ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta, string usuarioDNP);
        Task<string> GuardarFocalizacionProgramacionSeguimiento(FocalizacionProgramacionSeguimientoDto objFocalizacionProgramacionSeguimientoDto, string usuarioDNP);
        Task<string> ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid, string usuarioDNP);
        Task<string> GuardarCrucePoliticasSeguimiento(FocalizacionCrucePoliticaSeguimientoDto objFocalizacionCrucePoliticaSeguimientoDto, string usuarioDNP);
        Task<string> ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros, string usuarioDNP);
    }
}
