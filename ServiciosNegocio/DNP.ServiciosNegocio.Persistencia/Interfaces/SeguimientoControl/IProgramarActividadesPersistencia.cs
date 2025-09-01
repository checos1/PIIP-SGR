using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl
{
    public interface IProgramarActividadesPersistencia
    {
        string ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        string ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto);
        string ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto);
        List<CalendarioPeriodoDto> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto);
        void EditarProgramarActividad(string usuario, ProgramarActividadesDto ProyectosDto);
        void ActividadProgramacionSeguimientoPeriodosValores(string usuario, List<VigenciaEntregableDto> parametros);
        void ActividadReporteSeguimientoPeriodosValores(string usuario, ReporteSeguimiento parametros);
        void IndicadorPoliticaSeguimientoPeriodosValores(string usuario, ReporteIndicadorPoliticas parametros);
        string ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta);
        string GuardarFocalizacionProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionProgramacionSeguimientoDto> parametrosGuardar, string usuario);
        string ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid);
        string GuardarCruceProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionCrucePoliticaSeguimientoDto> parametrosGuardar, string usuario);
        string ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros);
    }
}
