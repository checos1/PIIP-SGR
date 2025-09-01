
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public interface IFuenteFinanciacionPersistencia
    {
        FuenteFinanciacionProyectoDto ObtenerFuenteFinanciacionProyecto(string bpin);
        List<FuenteFinanciacionProyectoDto> ObtenerFuentesFinanciacionProyecto(string bpin);
        ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyectoPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> parametrosGuardar, string usuario);
        string ObtenerPoliticasTransversalesAjustes(string Bpin);
        string GuardarPoliticasTransversalesAjustes(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);
        string ObtenerPoliticasTransversalesCategorias(string Bpin);
        RespuestaGeneralDto EliminarPoliticasProyecto(int proyectoId,int politicaId);
        RespuestaGeneralDto EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId,int categoriaId);
        string GuardarCategoriasPoliticaTransversalesAjustes(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario);
        string ObtenerPoliticasTransversalesResumen(string Bpin);
        string ObtenerPoliticasCategoriasIndicadores(string Bpin);
        string ObtenerCrucePoliticasAjustes(string bpin);
        RespuestaGeneralDto GuardarCrucePoliticasAjustes(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario);
        ResultadoProcedimientoDto ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario);
        string ObtenerPoliticasSolicitudConcepto(string Bpin);
        string FocalizacionSolicitarConceptoDT(ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>> parametrosGuardar, string usuario);
        string ObtenerDireccionesTecnicasPoliticasFocalizacion();
        string ObtenerResumenSolicitudConcepto(string Bpin);
        string ObtenerPreguntasEnvioPoliticaSubDireccion(Guid instanciaid, int proyectoid, string usuarioDNP, Guid nivelid);
        string GuardarPreguntasEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario);
        string GuardarRespuestaEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario);
    }
}
