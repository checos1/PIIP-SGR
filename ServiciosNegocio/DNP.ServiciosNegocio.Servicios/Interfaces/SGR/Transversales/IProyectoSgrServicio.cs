using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.AvalUso;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTEI;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.OcadPaz;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales
{
    public interface IProyectoSgrServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        string SGR_Proyectos_LeerListas(Guid nivelId, int proyectoId, string nombreLista);

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>string</returns> 
        string SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad);

        /// <summary>
        /// Validar cumplimiento de un proyecto por instancia
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <returns>int</returns> 
        int SGR_Proyectos_CumplimentoFlujoSGR(Guid instanciaId);

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param>
        /// <returns>ResultadoProcedimientoDto</returns> 
        ResultadoProcedimientoDto SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo);

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="user"></param>
        /// <param name="delegado"></param> 
        /// <returns>bool</returns> 
        bool SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string user);

        IEnumerable<ProyectoViabilidadInvolucradosDto> SGR_Proyectos_LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId);
        IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId);

        ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucrados(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario);
        ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradoso(int id);
        string SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId);
        bool SGR_Proyectos_PostAplicarFlujoSGR(AplicarFlujoSGRDto parametros);
        bool SGR_Proyectos_PostDevolverFlujoSGR(DevolverFlujoSGRDto parametros);
        ProyectoCtusDto SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId);
        IEnumerable<EntidadesSolicitarCtusDto> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId);
        ProyectoCtuResultado SGR_Proyectos_GuardarProyectoSolicitarCtus(ParametrosGuardarDto<ProyectoCtusDto> parametrosGuardar, string usuario);
        string SGR_Proyectos_validarTecnicoOcadpaz(Nullable<System.Guid> instanciaId, Nullable<System.Guid> accionId);
        List<ProyectoEntidadVerificacionOcadPazDto> ConsultarProyectosVerificacionOcadPazSgr(ParametrosProyectoVerificacionSgrDto parametros);
        IEnumerable<UsuariosVerificacionOcadPazDto> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId);
        ResultadoProcedimientoDto SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto json, string usuario);
        ResultadoProcedimientoDto SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json, string usuario);
        string SGR_Proyectos_LeerAsignacionUsuarioEncargado(int ProyectoId, Guid instanciaId);

        string SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId);

        ResultadoProcedimientoDto SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto datosAdicionalesCTEIDto, string usuario);

        ResultadoProcedimientoDto SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto datosAvalUsoDto, string usuario);

        string SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId);
        bool SGR_Obtener_Proyectos_TieneInstanciaActiva(String ObjetoNegocioId);
        ResultadoProcedimientoDto SGR_Viabilidad_EliminarOperacionCreditoSGR(int proyectoid, string usuario);
    }
}
