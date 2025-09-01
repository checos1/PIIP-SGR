using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public interface IFuenteFinanciacionServicios
    {
        ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyecto(ParametrosConsultaDto parametrosConsulta);
        ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyectoPreview();
        ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoFuenteFinanciacionDto contenido);
        void Guardar(ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        //FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId);
        string ObtenerPoliticasTransversalesAjustes(string Bpin);
        string GuardarPoliticasTransversalesAjustes(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);
        string ObtenerPoliticasTransversalesCategorias(string Bpin);
        RespuestaGeneralDto EliminarPoliticasProyecto(int proyectoId, int politicaId);
        RespuestaGeneralDto EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId);
        string GuardarCategoriasPoliticaTransversalesAjustes(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario);
        string ObtenerPoliticasTransversalesResumen(string Bpin);
        string ObtenerPoliticasCategoriasIndicadores(string Bpin);
        string ObtenerCrucePoliticasAjustes(string bPIN);
        RespuestaGeneralDto GuardarCrucePoliticasAjustes(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string name);
        ResultadoProcedimientoDto ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario);
        string ObtenerPoliticasSolicitudConcepto(string Bpin);
        string FocalizacionSolicitarConceptoDT(ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>> parametrosGuardar, string name);
        string ObtenerDireccionesTecnicasPoliticasFocalizacion();
        string ObtenerResumenSolicitudConcepto(string Bpin);
        string ObtenerPreguntasEnvioPoliticaSubDireccion(Guid instanciaid, int proyectoid, string usuarioDNP, Guid nivelid);
        string GuardarPreguntasEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string name);
        string GuardarRespuestaEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string name);
    }
}
