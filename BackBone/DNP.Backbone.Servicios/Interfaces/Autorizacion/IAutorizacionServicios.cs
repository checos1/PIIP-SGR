using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DNP.Backbone.Comunes.Dto;
using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Dominio.Dto;
using System.Security.Principal;
using DNP.Backbone.Dominio.Dto.CentroAyuda;
using UsuarioDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioDto;
using UsuarioPerfilDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilDto;
using UsuarioPerfilProyectoDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilProyectoDto;

namespace DNP.Backbone.Servicios.Interfaces.Autorizacion
{
    public interface IAutorizacionServicios
    {
        Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion,
                                                 string nombreServicio);
        Task<List<EntidadNegocioDto>> ObtenerEntidadesNegocio(string usuarioDnp);
        Task<List<TipoCargaDatosDto>> ObtenerCargaDatosPorTipoYTipoEntidad(string tipoEntidad, string usuarioDnp);

        Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXUsuarioAutenticado(string usuarioDnp);
        Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(string tipoEntidad, string usuarioDnp);

        Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidadBanco(string idEntidad, string usuarioDnp, int resourceGroupId);
        Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidad(string idEntidad, string usuarioDnp);
        Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidadYUsuario(string idEntidad, string usuarioDnp);
        Task<List<DNP.Backbone.Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto>> ObtenerListadoPerfilesXUsuarioTerritorio(string usuarioDnp);

        dynamic ObtenerDatosMongoDb(string id);
        RespuestaGeneralDto GuardarDatosMongoDB(dynamic data, string idSql);
        Task<RespuestaGeneralDto> GuardarDatos(CargaDatosDto cargaDto, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarCargaDatos(int id, string usuarioDnp);

        Task<List<TipoEntidadDto>> ObtenerConfiguracionesRolSector(string usuarioDnp);
        Task<List<RolNegocioDto>> ObtenerRolesPorEntidadTerritorial(string idUsuario, Guid idEntidadTerritorial);
        Task<List<UsuarioDto>> ObtenerUsuarios(string tipoEntidad, string usuarioDnp);
        Task<List<EntidadNegocioDto>> ObtenerEntidadesInviteUsuario(string usuarioDnp, string tipoEntidad, IPrincipal principal);
        Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuario(string usuarioDnp, string tipoEntidad);
        Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuarioXUsuarioAutenticado(string usuarioDnp, string tipoEntidad, string usuarioDnpAutenticado);

        Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal);
        Task<List<EntidadUsuarioDto>> ObtenerUsuariosXEntidad(string tipoEntidad, string usuarioDnp, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal);
        Task<string> ObtenerUsuariosPorEntidadSp(string tipoEntidad, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal);

        Task<string> ObtenerUsuarioPIIPXCorreoDNPAsync(string usuarioTemporal);

        Task<UsuarioDto> ObtenerUsuarioPorId(string idUsuarioDNP, string idUsuario);
        Task<UsuarioDto> ObtenerUsuarioPorIdUsuarioDnp(string idUsuarioDNP);
        Task<bool> CrearUsuarioPIIP(UsuarioPIIPDto usuarioDto, string idUsuarioDnp);
        Task<bool> CrearUsuarioTerritorioPIIP(UsuarioPIIPTerritorioDto usuarioDto, string idUsuarioDnp);

        Task<bool> CrearUsuarioInvitado(InvitarUsuarioDto usuarioDto, string idUsuarioDnp);
        Task<bool> CrearUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp);
        Task<IEnumerable<PerfilDto>> ObtenerTodosPerfiles(string idUsuarioDnp);
        Task<IEnumerable<EntidadDto>> ObtenerTodasEntidades(string idUsuarioDnp);
        Task<bool> EditarUsuario(UsuarioDto usuarioDto);
        Task<RespuestaGeneralDto> SetActivoUsuarioPerfilPorEntidad(SetActivoUsuarioPerfilPorEntidadDto dto);
        Task<RespuestaGeneralDto> SetActivoUsuarioPerfil(SetActivoUsuarioPerfilDto dto);
        Task<RespuestaGeneralDto> SetActivoUsuarioEntidad(SetActivoUsuarioPerfilPorEntidadDto dto);
        Task<List<RolDto>> ObtenerRolesPorPerfil(Guid idPerfil, string usuarioDnp);
        Task<List<RolDto>> ObtenerRoles(string usuarioDnp, string roleFiltro);
        Task<List<OpcionDto>> ObtenerOpciones(string usuarioDnp);
        Task<List<OpcionDto>> ObtenerOpcionesDeRol(Guid idRol, string usuarioDnp);
        Task<List<UsuarioPerfilProyectoDto>> ObtenerProyectosPorPerfil(Guid idUsuarioPerfil, string usuarioDnp);
        Task<EntidadNegocioDto> ObtenerEntidadPorId(EntidadFiltroDto dto, string idUsuarioDnp);
        Task<EntidadFiltroDto> ObtenerEntidadPorCatalogoOptionId(int entidadCatalogoOptionId, string usuarioDnp);
        Task<RespuestaGeneralDto> AsociarProyectosAUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp);
        Task<List<SectorNegocioDto>> ObtenerSectoresPorEntidadTerritorial(string idUsuario, Guid idEntidadTerritorial);
        Task<List<EntidadNegocioDto>> ObtenerEntidadesPorSectorTerritorial(string idUsuario, Guid idEntidadTerritorial, Guid idSector);
        Task<RespuestaGeneralDto> GuardarPerfil(PerfilDto perfilDto);

        Task<RespuestaGeneralDto> EliminarPerfil(PerfilDto perfilDto);
        Task<RespuestaGeneralDto> EliminarUsuarioPerfil(Guid idUsuarioPerfil, string idUsuarioDnp);
        Task<RespuestaGeneralDto> GuardarConfiguracionRolSectorAsync(PeticionConfiguracionRolSectorDto peticion);
        Task<RespuestaGeneralDto> EditarConfiguracionRolSector(PeticionConfiguracionRolSectorDto peticion);
        Task<RespuestaGeneralDto> CambiarEstadoConfiguracionRolSector(PeticionCambioEstadoConfiguracionDto peticion);
        Task<List<EntidadAutorizacionDto>> ObtenerEntidadesPorListaRoles(List<Guid> datosConsultaListaIdsRoles, string idUsuario);
        Task<List<PerfilDto>> ObtenerPerfiles(UsuarioLogadoDto dto, string perfilFiltro);

        //Task<List<RolDto>> ObtenerRolesPorAplicacion(string roleFiltro);

        Task<RespuestaGeneralDto> GuardarRol(RolDto rolDto);

        Task<RespuestaGeneralDto> EliminarRol(RolDto rolDto, string usuarioDnp);

        Task<List<RolDto>> ObtenerRolesPorOpcionDnp(Guid idOpcionDnp, string usuarioDnp);

        Task<List<PerfilDto>> ObtenerPerfilesPorAplicacion(string usuarioDnp, string aplicacion);

        Task<List<PerfilDto>> ObtenerPerfilesAutorizadosPorAplicacion(String idAplicacion, string idPerfil, string usuarioDnp);

        Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidad(string aplicacion, string usuarioDnp);
        Task<ResultUnidadResponsableDTO> ObtenerEntidadesPorUnidadesResponsables(string usuarioDnp);
        Task<ResultSectorDTO> ObtenerSectoresParaEntidades(string usuarioDnp);

        Task<EntidadFiltroDto> ObtenerEntidadPorEntidadId(Guid idEntidad, string usuarioDnp);
        Task<List<SectorNegocioDto>> ObtenerSectoresNegocio(string usuarioDnp);
        Task<List<EntidadNegocioDto>> ObtenerDepartamentos(string usuaquiDnp);

        Task<List<EntidadFiltroDto>> ObtenerSubEntidadesPorEntidadId(Guid idEntidad, string usuarioDnp);

        Task<RespuestaGeneralDto> GuardarEntidad(EntidadNegocioDto entidadDto, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarUnidadResponsable(EntidadNegocioDto entidadDto, string usuarioDnp);
        //Task<RespuestaGeneralDto> GuardarCargaDatos(CargaDatosDto cargaDto, string usuarioDnp); 

        Task<RespuestaGeneralDto> GuardarInflexibilidad(InflexibilidadDto inflexibilidadDto, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarInflexibilidadPagos(List<InflexibilidadPagosDto> lista, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarInflexibilidad(int id, string usuarioDnp);

        Task<RespuestaGeneralDto> EliminarEntidad(Guid idEntidad, string usuarioDnp);
        Task<List<CrTypeDto>> ObtenerCRType(string usuarioDnp);
        Task<List<FaseDto>> ObtenerFase(string usuarioDnp);
        Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId, string usuarioDnp);

        #region Adherencia

        Task<List<AdherenciaDto>> ObtenerAdherenciasPorEntidadId(Guid idEntidad, string usuarioDnp);

        Task<RespuestaGeneralDto> GuardarAdherencia(AdherenciaDto entidadDto, string usuarioDnp);

        Task<RespuestaGeneralDto> EliminarAdherencia(int idAdherencia, string usuarioDnp);

        #endregion Adherencia

        #region Delegado

        Task<List<DelegadoDto>> ObtenerDelegadosPorEntidadId(Guid idEntidad, string usuarioDnp);

        Task<RespuestaGeneralDto> GuardarDelegado(DelegadoDto delegadoDto, string usuarioDnp);

        Task<RespuestaGeneralDto> EliminarDelegado(int idDelegado, string usuarioDnp);

        #endregion Delegado

        Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos, string usuarioDnp);

        Task<List<InflexibilidadDto>> ObtenerInflexibilidadPorEntidadId(Guid idEntidad, InflexibilidadFiltroDto filtro, string usuarioDnp);
        Task<List<InflexibilidadPagosDto>> ObtenerInflexibilidadPagos(int idInflexibilidad, string usuarioDnp);

        Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidadYUsuario(string tipoEntidad, string usuarioDnp);

        Task<PermisosEntidadDto> ObtenerPermisosPorEntidad(string idUsuarioDnp);

        Task<List<RolDto>> ObtenerRolesPorUsuario(UsuarioDto dto);

        Task<List<EntidadNegocioDto>> ObtenerEntidadesConRoleVisualizador(string usuarioDnp);

        Task<List<RolDto>> ObtenerRolesPorIdUsuarioPerfil(Guid idUsuarioPerfil, string usuarioDnp);
        Task<List<RespuestaUsuariosConfiguracionDto>> ObtenerUsuariosPorNombreIdentificacion(ParametrosUsuariosConfiguracionDto filtro, string usuarioDnp);
        Task<bool> GuardarSectoresPorUsuarioEntidad(List<RespuestaSectoresUsuarioConfiguracionDto> data, string usuarioDnp);
        Task<List<RespuestaSectoresUsuarioConfiguracionDto>> ObtenerSectoresPorUsuarioEntidad(ParametrosSectoresUsuarioConfiguracionDto filtro, string usuarioDnp);
        Task<List<RolDto>> ObtenerRolesPorIdsUsuarioPerfil(List<Guid> idsUsuarioPerfil, string usuarioDnp);

        Task<List<UsuarioAuthDto>> ObtenerUsuariosPorNombre(string nombre, string usuarioLogado);

        Task<RespuestaGeneralDto> ActualizarIdArchivoInflexibilidadPagos(InflexibilidadPagosDto pago, string usuarioDnp);
        Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosPorSubDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto);
        Task<IEnumerable<OpcionDto>> ObtenerOpcionesConFiltro(string idAplicacion, string idUsuarioDnp);
        Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosRValidadorPoliticaTransversal(ProyectoParametrosDto peticionObtenerProyecto);

        Task<Dominio.Dto.AutorizacionNegocio.UsuarioCuentaDto> ObtenerCuentaUsuario(string nomeCuenta, string usuarioDnp);

        Task<Dominio.Dto.AutorizacionNegocio.UsuarioDto> ObtenerUsuarioPorCorreoDNP(string correo, string usuarioDnp);

        Task<List<EntidadFiltroDto>> ObtenerListaEntidad(string usuarioDnp, string objetoNegocioId);

        Task<string> validarPermisoInactivarUsuario(string usuarioDnp, string usuarioDnpEliminar);

        Task<IEnumerable<EntidadFiltroConsolaDto>> ObtnerEntidadesPorSector(int SectorId, string tipoEntidad, string usuarioDnp);

        Task<string> ObtenerEncabezadoListadoReportesPIIP(string usuarioDnp);

        Task<string> ObtenerListadoReportesPIIP(string usuarioDnp, string idRoles);

        Task<string> ObtenerFiltrosReportesPIIP(Guid idReporte, string usuarioDnp);

        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDnp, string idEntidades);
        Task<List<AutorizacionUsuarioEntidadRolDto>> ObtenerUsuariosBasicosPorRolEntidad(EntidadRolDto entidadRol, string usuarioDnp);
    }
}
