namespace DNP.Backbone.Servicios.Implementaciones.AutorizacionNegocio
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web;
    using Comunes;
    using Comunes.Dto;
    using Comunes.Enums;
    using Comunes.Excepciones;
    using DNP.Autorizacion.Dominio.Dto;
    using DNP.Backbone.Comunes.Extensiones;
    using DNP.Backbone.Comunes.Utilidades;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.CentroAyuda;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using DNP.Backbone.Dominio.Filtros;
    using DNP.Backbone.Persistencia.Interfaces;
    using Dominio.Dto.AutorizacionNegocio;
    using Interfaces;
    using Interfaces.Autorizacion;
    using Interfaces.Cache;
    using Newtonsoft.Json;
    using UsuarioDto = Dominio.Dto.Usuario.UsuarioDto;
    using UsuarioPerfilDto = Dominio.Dto.Usuario.UsuarioPerfilDto;
    using UsuarioPerfilProyectoDto = Dominio.Dto.Usuario.UsuarioPerfilProyectoDto;

    public class AutorizacionServicios : IAutorizacionServicios
    {
        private readonly ICacheEntidadesNegocioServicios _cacheEntidadesNegocioServicios;
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IAutorizacionPersistencia _autorizacionPersistencia;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiAutorizacion"];

        public AutorizacionServicios(ICacheEntidadesNegocioServicios cacheEntidadesNegocioServicios, IClienteHttpServicios clienteHttpServicios, IAutorizacionPersistencia autorizacionPersistencia)
        {
            _cacheEntidadesNegocioServicios = cacheEntidadesNegocioServicios;
            _clienteHttpServicios = clienteHttpServicios;
            _autorizacionPersistencia = autorizacionPersistencia;
        }

        public async Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            ValidarParametros(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
            var respuestaAutorizacion = await ObtenerPermisosUsuario(nombreUsuario, idAplicacion, nombreServicio).ConfigureAwait(false);
            ConstruirRespuestaHttp(respuestaAutorizacion);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ReasonPhrase = BackboneRecursos.UsuarioAutorizado
            };
        }

        public async Task<List<EntidadNegocioDto>> ObtenerEntidadesNegocio(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesNegocio"];

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp));
        }


        /// <summary>
        /// Obtener Configuraciones Rol Sector
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de TipoEntidadDto</returns>
        public async Task<List<TipoEntidadDto>> ObtenerConfiguracionesRolSector(string usuarioDnp)
        {
            var esUsuarioGlobal = await UsuarioEsAdministradorGlobal(usuarioDnp);

            var configuracionesEntidades = new List<TipoEntidadDto>();

            var configuracionTerritorial = new TipoEntidadDto()
            {
                TipoEntidad = TiposEntidad.Territorial.ToString(),
                EntidadesTerritoriales = await ConsultarEntidadesTerritoriales(usuarioDnp, esUsuarioGlobal, TiposEntidad.Territorial.ToString()),
                Configuraciones = await ConsultarConfiguraciones(TiposEntidad.Territorial.ToString(), usuarioDnp)
            };

            configuracionesEntidades.Add(configuracionTerritorial);

            var configuracionNacional = new TipoEntidadDto()
            {
                TipoEntidad = TiposEntidad.Nacional.ToString(),
                Roles = await ConsultarRoles(usuarioDnp, esUsuarioGlobal),
                Sectores = await ConsultarSectores(usuarioDnp, esUsuarioGlobal),
                EntidadesDestino = await ConsultarEntidadesDestino(usuarioDnp, esUsuarioGlobal, TiposEntidad.Nacional.ToString()),
                Configuraciones = await ConsultarConfiguraciones(TiposEntidad.Nacional.ToString(), usuarioDnp)
            };

            configuracionesEntidades.Add(configuracionNacional);

            return configuracionesEntidades;
        }

        /// <summary>
        /// Obtener roles por entidad territorial
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <param name="idEntidadTerritorial"></param>
        /// <returns>una lista de RolNegocioDto</returns>
        public async Task<List<RolNegocioDto>> ObtenerRolesPorEntidadTerritorial(string usuarioDnp, Guid idEntidadTerritorial)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorEntidadTerritorial"];
            var parametros = $"?usuarioDnp={usuarioDnp}&idEntidadTerritorial={idEntidadTerritorial}";

            return JsonConvert.DeserializeObject<List<RolNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener roles por perfil
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<List<RolDto>> ObtenerRolesPorPerfil(Guid idPerfil, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorPerfil"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var request = new ParametrosPerfilDto()
            {
                IdPerfil = idPerfil,
                IdAplicacion = guidBackBone
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
        }

        /// <summary>
        /// Obtener roles por aplicacion
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <param name="roleFiltro"></param>
        /// <returns>Una lista de DuplaDto</returns>
        public async Task<List<RolDto>> ObtenerRoles(string usuarioDnp, string roleFiltro)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorAplicacion"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var request = new ParametrosPerfilDto()
            {
                IdAplicacion = guidBackBone
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            var roles = JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
            if (!string.IsNullOrWhiteSpace(roleFiltro))
                roles = roles.Where(x => x.Nombre.ToLower().Contains(roleFiltro.ToLower())).OrderBy(x => x.Nombre).ToList();

            return roles;
        }

        /// <summary>
        /// Obtener peoyectos por perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de UsuarioPerfilProyectoDto</returns>
        public async Task<List<UsuarioPerfilProyectoDto>> ObtenerProyectosPorPerfil(Guid idUsuarioPerfil, string usuarioDnp)
        {
            // Obtener Ids de proyectos de lo servicio de Autorizacion
            var uriObtenerIdsDeProyectos = ConfigurationManager.AppSettings["uriObtenerIdsDeProyectosPorPerfil"];
            var request = new ParametrosPerfilDto()
            {
                IdUsuarioPerfil = idUsuarioPerfil
            };
            var idsProyectos = JsonConvert.DeserializeObject<List<int>>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriObtenerIdsDeProyectos, string.Empty, request, usuarioDnp, useJWTAuth: false)
            );

            // Obtener Informaciones de los proyectos de lo servicio ServicioNegocios
            var apiServicioNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriObtenerProyectosPorIds = ConfigurationManager.AppSettings["uriObtenerProyectosPorIds"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, apiServicioNegocio, uriObtenerProyectosPorIds, string.Empty, idsProyectos, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<UsuarioPerfilProyectoDto>>(respuesta);
        }

        public async Task<EntidadNegocioDto> ObtenerEntidadPorId(EntidadFiltroDto dto, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadPorId"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, dto, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<EntidadNegocioDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> AsociarProyectosAUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriAsociarProyectosAUsuarioPerfil"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, dto, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<List<SectorNegocioDto>> ObtenerSectoresPorEntidadTerritorial(string usuarioDnp, Guid idEntidadTerritorial)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSectoresPorEntidadTerritorial"];
            var parametros = $"?usuarioDnp={usuarioDnp}&idEntidadTerritorial={idEntidadTerritorial}";

            return JsonConvert.DeserializeObject<List<SectorNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));

        }

        public async Task<List<EntidadNegocioDto>> ObtenerEntidadesPorSectorTerritorial(string usuarioDnp, Guid idEntidadTerritorial, Guid idSector)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesPorSectorTerritorial"];
            var parametros = $"?idUsuarioDnp={usuarioDnp}&idEntidadTerritorial={idEntidadTerritorial}&idSector={idSector}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));

        }

        /// <summary>
        /// Guardar Configuracion Rol Sector
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> GuardarConfiguracionRolSectorAsync(PeticionConfiguracionRolSectorDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarConfiguracionRolSector"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
        }

        /// <summary>
        /// Guardar perfil
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> GuardarPerfil(PerfilDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings[peticion.IdPerfil != null ? "uriEditarPerfilConRoles" : "uriCrearPerfilConRoles"];
            var IdBackbone = ConfigurationManager.AppSettings["GuidPIIPAplicacion"];
            peticion.IdAplicacion = Guid.Parse(IdBackbone);

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Eliminar perfil
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> EliminarPerfil(PerfilDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarPerfilEPerfilRoles"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
        }

        public async Task<RespuestaGeneralDto> EliminarUsuarioPerfil(Guid idUsuarioPerfil, string idUsuarioDnp)
        {
            var uriMetodo = string.Format("{0}/{1}", ConfigurationManager.AppSettings["uriEliminarUsuarioPerfil"], idUsuarioPerfil);

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, idUsuarioPerfil, idUsuarioDnp, true, useJWTAuth: false));
        }

        /// <summary>
        /// Editar Configuracion Rol Sector
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> EditarConfiguracionRolSector(PeticionConfiguracionRolSectorDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEditarConfiguracionRolSector"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Put, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
        }
        /// <summary>
        /// Cambiar Estado Configuracion Rol Sector
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> CambiarEstadoConfiguracionRolSector(PeticionCambioEstadoConfiguracionDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCambiarEstadoConfiguracionRolSector"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Put, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener entidades por lista de roles
        /// </summary>
        /// <param name="idsRoles"></param>
        /// <param name="idUsuario"></param>
        /// <returns>Una lista de EntidadAutorizacionDto</returns>
        public async Task<List<EntidadAutorizacionDto>> ObtenerEntidadesPorListaRoles(List<Guid> idsRoles, string idUsuario)
        {
            var idAplicacion = ConfigurationManager.AppSettings["IdNombreBackbone"];

            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesPorRoles"];

            var listaRoles = idsRoles.Count <= 1 ? idsRoles[0].ToString() : string.Join("&IdsRoles=", idsRoles.ToArray());
            var parametros = $"?idAplicacionDnp={idAplicacion}&IdsRoles={listaRoles}&usuarioDNP={idUsuario}";

            return JsonConvert.DeserializeObject<List<EntidadAutorizacionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuario, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener perfiles por usuario
        /// </summary>
        /// <param name="idUsuarioDnp"></param>
        /// <param name="tipoEntidad"></param>
        /// <returns>Una lista de EntidadePerfilDto</returns>
        public async Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuario(string idUsuarioDnp, string tipoEntidad)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesPorUsuarioEntidad"];
            var parametros = $"?idUsuarioDnp={idUsuarioDnp}&tipoEntidad={tipoEntidad}";

            return JsonConvert.DeserializeObject<List<EntidadPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener perfiles por usuario
        /// </summary>
        /// <param name="idUsuarioDnp"></param>
        /// <param name="tipoEntidad"></param>
        /// <returns>Una lista de EntidadePerfilDto</returns>
        public async Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuarioXUsuarioAutenticado(string idUsuarioDnp, string tipoEntidad, string idUsuarioDnpAutenticado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesPorUsuarioEntidadXUsuarioAutenticado"];
            var parametros = $"?idUsuarioDnp={idUsuarioDnp}&tipoEntidad={tipoEntidad}&idUsuarioDnpAutenticado={idUsuarioDnpAutenticado}";

            return JsonConvert.DeserializeObject<List<EntidadPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnpAutenticado, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener Usuarios por Entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una lista de EntidadUsuarioDto</returns>
        public async Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosPorEntidad"];
            var parametros = $"?tipoEntidad={tipoEntidad}&filtro={filtro}&nombreUsuario={nombreUsuario}&cuentaUsuario={cuentaUsuario}&estado={estado}";

            return JsonConvert.DeserializeObject<List<EntidadUsuarioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, principal: principal, useJWTAuth: false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <param name="nombreUsuario"></param>
        /// <param name="cuentaUsuario"></param>
        /// <param name="estado"></param>
        /// <param name="idUsuarioDnp"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public async Task<string> ObtenerUsuariosPorEntidadSp(string tipoEntidad, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosPorEntidadSp"];
            var parametros = $"?tipoEntidad={tipoEntidad}&filtro={filtro}&nombreUsuario={nombreUsuario}&cuentaUsuario={cuentaUsuario}&estado={estado}";
            var dataReturn = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, principal: principal, useJWTAuth: false));
            return dataReturn;
        }

        /// <summary>
        /// Obtener Usuarios por Entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una lista de EntidadUsuarioDto</returns>
        public async Task<List<EntidadUsuarioDto>> ObtenerUsuariosXEntidad(string tipoEntidad, string usuarioDnp, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosXEntidad"];
            var parametros = $"?usuarioDnp={usuarioDnp}&tipoEntidad={tipoEntidad}&filtro={filtro}&nombreUsuario={nombreUsuario}&cuentaUsuario={cuentaUsuario}&estado={estado}";

            return JsonConvert.DeserializeObject<List<EntidadUsuarioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, principal: principal, useJWTAuth: false));
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorId(string idUsuarioDNP, string idUsuario)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorId"];
            var parametros = string.IsNullOrEmpty(idUsuarioDNP) ? $"{idUsuario}" : $"{idUsuario}?idUsuarioDNP={idUsuarioDNP}";

            return JsonConvert.DeserializeObject<UsuarioDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDNP, useJWTAuth: false));
        }

        /// <summary>
        /// Obtener perfiles por aplicacion
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="perfilFiltro"></param>
        /// <returns>Una lista de PerfilDto</returns>
        public async Task<bool> EditarUsuario(UsuarioDto usuarioDto)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEditarUsuario"];
            await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, usuarioDto, usuarioDto.IdUsuarioDnp, useJWTAuth: false);

            return true;
        }

        public async Task<string> ObtenerUsuarioPIIPXCorreoDNPAsync(string usuarioTemporal)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPIIPXCorreoDNP"];

            var response = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.GetAsync,
                ENDPOINT,
                uriMetodo,
                peticion: null,
                parametros: usuarioTemporal,
                usuarioDnp: usuarioTemporal,
                useJWTAuth: false
            );
            response.TryDeserialize<string>(out var result);

            if (result == null)
            {
                var errores = response.Deserialize<IDictionary<string, string>>();
                throw new BackboneException(errores["Message"]);
            }

            return result;
        }

        public async Task<bool> CrearUsuarioInvitado(InvitarUsuarioDto usuarioDto, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearUsuario"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, ENDPOINT, uriMetodo, null, usuarioDto, idUsuarioDnp, useJWTAuth: false);
            response.TryDeserialize<bool?>(out var result);

            if (result == null)
            {
                var errores = response.Deserialize<IDictionary<string, string>>();
                throw new BackboneException(errores["Message"]);
            }

            return (bool)result;
        }

        public async Task<bool> CrearUsuarioPIIP(UsuarioPIIPDto usuarioDto, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearUsuarioPIIP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, ENDPOINT, uriMetodo, null, usuarioDto, idUsuarioDnp, useJWTAuth: false);
            response.TryDeserialize<bool?>(out var result);

            if (result == null)
            {
                var errores = response.Deserialize<IDictionary<string, string>>();
                throw new BackboneException(errores["Message"]);
            }

            return (bool)result;
        }

        public async Task<bool> CrearUsuarioTerritorioPIIP(UsuarioPIIPTerritorioDto usuarioDto, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearUsuarioTerritorioPIIP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, ENDPOINT, uriMetodo, null, usuarioDto, idUsuarioDnp, useJWTAuth: false);
            response.TryDeserialize<bool?>(out var result);

            if (result == null)
            {
                var errores = response.Deserialize<IDictionary<string, string>>();
                throw new BackboneException(errores["Message"]);
            }

            return (bool)result;
        }

        public async Task<bool> CrearUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiAutorizacion"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearUsuarioPerfil"];

            await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, endPoint, uriMetodo, null, dto, idUsuarioDnp);

            return true;
        }

        public async Task<IEnumerable<PerfilDto>> ObtenerTodosPerfiles(string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTodosPerfiles"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<IEnumerable<PerfilDto>>(respuesta);
        }

        public async Task<IEnumerable<EntidadDto>> ObtenerTodasEntidades(string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTodasEntidades"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<IEnumerable<EntidadDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> SetActivoUsuarioPerfilPorEntidad(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSetActivoUsuarioPerfilPorEntidad"];
            var idUsuarioDnp = dto.UsuarioDnp;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, dto, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> SetActivoUsuarioPerfil(SetActivoUsuarioPerfilDto dto)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSetActivoUsuarioPerfil"];
            var idUsuarioDnp = dto.UsuarioDnp;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, dto, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> SetActivoUsuarioEntidad(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSetActivoUsuarioEntidad"];
            var idUsuarioDnp = dto.UsuarioDnp;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, dto, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<List<PerfilDto>> ObtenerPerfiles(UsuarioLogadoDto dto, string perfilRolFiltro)
        {
            var endPoint = dto.ApiAutorizacion; //ConfigurationManager.AppSettings["ApiAutorizacion"];
            var guidBackBone = dto.GuidPIIPAplicacion; //new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]) ;
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesPorAplicacion"];
            var request = new { IdAplicacion = guidBackBone };

            var perfiles = JsonConvert.DeserializeObject<List<PerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, endPoint, uriMetodo, string.Empty, request, dto.IdUsuario, useJWTAuth: false));
            if (!string.IsNullOrWhiteSpace(perfilRolFiltro))
                perfiles = perfiles.Where(x => x.NombrePerfil.ToLower().Contains(perfilRolFiltro.ToLower()) ||
                    x.Roles.Any(rol => rol.Nombre.ToLower().Contains(perfilRolFiltro.ToLower()))).ToList();

            foreach (var item in perfiles)
            {
                var roles = await ObtenerRolesPorPerfil(item.IdPerfil.Value, dto.IdUsuario);
                item.RolesConcat = String.Join(" - ", roles.Select(x => x.Nombre));
            }

            return perfiles;

        }

        /// <summary>
        /// Guardar rol con o sin opciones
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> GuardarRol(RolDto peticion)
        {
            var uriMetodo = ConfigurationManager.AppSettings[peticion.Agregar ? "uriCrearRolConOpciones" : "uriEditarRolConOpciones"];
            var IdBackbone = ConfigurationManager.AppSettings["GuidPIIPAplicacion"];
            peticion.IdAplicacion = Guid.Parse(IdBackbone);

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, peticion.UsuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Eliminar rol
        /// </summary>
        /// <param name="peticion"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>RespuestaGeneralDto</returns>
        public async Task<RespuestaGeneralDto> EliminarRol(RolDto peticion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarRolAplicacion"];
            var IdBackbone = ConfigurationManager.AppSettings["GuidPIIPAplicacion"];
            peticion.IdAplicacion = Guid.Parse(IdBackbone);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<RolDto>> ObtenerRolesPorOpcionDnp(Guid idOpcionDnp, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorOpcionDnp"];

            var parametros = $"?idOpcionDnp={idOpcionDnp}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
        }

        public async Task<List<OpcionDto>> ObtenerOpciones(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerOpcionesPorAplicacion"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var parametros = $"?idAplicacion={guidBackBone}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<OpcionDto>>(respuesta);
        }

        public async Task<List<OpcionDto>> ObtenerOpcionesDeRol(Guid idRol, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerOpcionesPorRol"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var request = new ParametrosRolDto()
            {
                IdRol = idRol,
                IdAplicacion = guidBackBone
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<OpcionDto>>(respuesta);
        }

        /// <summary>
        /// Obtener Usarios por Entidad
        /// </summary>
        /// <param name="perfilFiltro"></param>
        /// <returns>Uns lista de PerfilDto</returns>
        public async Task<List<PerfilDto>> ObtenerUsuariosPorEntidadAsync(string perfilFiltro)
        {
            //"api/Autorizacion/ObtenerPerfiles
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesPorAplicacion"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidBackboneAplicacion"]);
            var idUsuarioDnp = "TGV0aWNpYTAxOjUyNzQwNjcy";

            var request = new { IdAplicacion = guidBackBone };


            var perfiles = JsonConvert.DeserializeObject<List<PerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, idUsuarioDnp));
            if (!string.IsNullOrWhiteSpace(perfilFiltro))
                perfiles = perfiles.Where(x => x.NombrePerfil.ToLower().Contains(perfilFiltro.ToLower())).ToList();

            foreach (var item in perfiles)
            {
                var roles = await ObtenerRolesPorPerfil(item.IdPerfil.Value, idUsuarioDnp);
                item.RolesConcat = String.Join(" - ", roles.Select(x => x.Nombre));
            }

            return perfiles;

        }



        public async Task<List<UsuarioDto>> ObtenerUsuarios(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarios"];
            return JsonConvert.DeserializeObject<List<UsuarioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<PerfilDto>> ObtenerPerfilesPorAplicacion(string usuarioDnp, string aplicacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesPorAplicacion"];
            var request = new { IdAplicacion = ConfigurationManager.AppSettings[aplicacion.ToLower() == "backbone" ? "GuidPIIPAplicacion" : "GuidAdministracionAplicacion"] };
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);

            var perfiles = JsonConvert.DeserializeObject<List<PerfilDto>>(response);
            return perfiles;
        }

        public async Task<List<PerfilDto>> ObtenerPerfilesAutorizadosPorAplicacion(string idAplicacion, string idPerfil, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPerfilesAutorizadosPorAplicacion"];
            var request = new
            {
                IdAplicacion = idAplicacion,
                IdPerfil = idPerfil,
                nombreUsuario = usuarioDnp
            };
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);

            var perfiles = JsonConvert.DeserializeObject<List<PerfilDto>>(response);
            return perfiles;
        }

        public async Task<List<EntidadNegocioDto>> ObtenerEntidadesInviteUsuario(string usuarioDnp, string tipoEntidad, IPrincipal principal)
        {
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesInviteUsuario"];
            var parametros = $"?tipoEntidad={tipoEntidad}&idAplicacion={guidBackBone}&usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, principal: principal, useJWTAuth: false));
        }

        public async Task<List<SectorNegocioDto>> ObtenerSectoresNegocio(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSectoresNegocio"];

            return JsonConvert.DeserializeObject<List<SectorNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<EntidadNegocioDto>> ObtenerDepartamentos(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDepartamentos"];

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
        }

        public Task<List<EntidadPerfilDto>> ObtenerUsuariosPorEntidad(string tipoEntidad)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidad(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesPorTipoEntidad"];
            var parametros = $"?tipoEntidad={tipoEntidad}";

            return JsonConvert.DeserializeObject<List<EntidadFiltroDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<ResultUnidadResponsableDTO> ObtenerEntidadesPorUnidadesResponsables(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesPorUnidadesResponsables"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<ResultUnidadResponsableDTO>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<ResultSectorDTO> ObtenerSectoresParaEntidades(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSectoresParaEntidades"];

            return JsonConvert.DeserializeObject<ResultSectorDTO>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<EntidadFiltroDto> ObtenerEntidadPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadPorEntidadId"];
            var parametros = $"?idEntidad={idEntidad}";

            return JsonConvert.DeserializeObject<EntidadFiltroDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<EntidadFiltroDto> ObtenerEntidadPorCatalogoOptionId(int entidadCatalogoOptionId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadPorCatalogoOptionId"];
            var parametros = $"?entidadCatalogoOptionId={entidadCatalogoOptionId}";

            return JsonConvert.DeserializeObject<EntidadFiltroDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<EntidadFiltroDto>> ObtenerSubEntidadesPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSubEntidadesPorEntidadId"];
            var parametros = $"?idEntidad={idEntidad}";

            return JsonConvert.DeserializeObject<List<EntidadFiltroDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }


        public async Task<RespuestaGeneralDto> EliminarInflexibilidad(int id, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarInflexibilidad"];
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];

            var parametros = $"?idsInflexibilidades={id.ToString()}";
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Delete, apiPiipCore, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }
        public async Task<RespuestaGeneralDto> EliminarEntidad(Guid idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarEntidad"];
            var peticion = new EntidadFiltroDto() { IdEntidad = idEntidad };
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, usuarioDnp, useJWTAuth: false));

        }


        public async Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos, string usuarioDnp)
        {

            var uriMetodo = ConfigurationManager.AppSettings["uriMantenimientoMatrizFlujo"];
            var apiServicioNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, apiServicioNegocio, uriMetodo, string.Empty, flujos, usuarioDnp, useJWTAuth: false));
            return respuesta;

        }

        /// <summary>
        /// Crear nueva entidad
        /// </summary>
        /// <param name="entidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> GuardarEntidad(EntidadNegocioDto entidadDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings[entidadDto.Id != Guid.Empty ? "uriEditarEntidad" : "uriGuardarEntidad"];

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, entidadDto, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }

        /// <summary>
        /// Crear nueva entidad
        /// </summary>
        /// <param name="entidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> ActualizarUnidadResponsable(EntidadNegocioDto entidadDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarUnidadResponsable"];

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, entidadDto, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }

        /// <summary>
        /// Crear / Editar Inflexibilidad
        /// </summary>
        /// <param name="inflexibilidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> GuardarInflexibilidad(InflexibilidadDto inflexibilidadDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearActualizar"];
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, apiPiipCore, uriMetodo, string.Empty, inflexibilidadDto, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }

        /// <summary>
        /// Crear Pagos Inflexibilidad
        /// </summary>
        /// <param name="inflexibilidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> GuardarInflexibilidadPagos(List<InflexibilidadPagosDto> lista, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearPagos"];
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, apiPiipCore, uriMetodo, string.Empty, lista, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }

        /// <summary>
        /// Obtener lista CRType
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de CRtype</returns>
        public async Task<List<CrTypeDto>> ObtenerCRType(string usuarioDnp)
        {

            // Obtener Informaciones de los proyectos de lo servicio ServicioNegocios
            var apiServicioNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriObtenerCRType = ConfigurationManager.AppSettings["uriObtenerCRType"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, apiServicioNegocio, uriObtenerCRType, string.Empty, null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<CrTypeDto>>(respuesta);
        }

        /// <summary>
        /// Obtener lista Fase
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de Fase</returns>
        public async Task<List<FaseDto>> ObtenerFase(string usuarioDnp)
        {

            // Obtener Informaciones de los proyectos de lo servicio ServicioNegocios
            var apiServicioNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriObtenerFase = ConfigurationManager.AppSettings["uriObtenerFase"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, apiServicioNegocio, uriObtenerFase, string.Empty, null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<FaseDto>>(respuesta);
        }

        /// <summary>
        /// Obtener lista de la Inflexibilidad
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de Inflexibilidad</returns>
        public async Task<List<InflexibilidadDto>> ObtenerInflexibilidadPorEntidadId(Guid idEntidad, InflexibilidadFiltroDto filtro, string usuarioDnp)
        {
            // Obtener Informaciones de los proyectos de lo servicio PiipCore
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriObtenerInflexibilidadPorEntidadId = ConfigurationManager.AppSettings["uriObtenerInflexibilidadPorEntidadId"];
            var parametros = $"?idEntidad={idEntidad}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, apiPiipCore, uriObtenerInflexibilidadPorEntidadId, parametros, null, usuarioDnp, useJWTAuth: false);

            List<InflexibilidadDto> inflexibilidades = JsonConvert.DeserializeObject<List<InflexibilidadDto>>(respuesta);
            FiltrarInflexibilidad(ref inflexibilidades, filtro);

            return inflexibilidades;
        }

        private void FiltrarInflexibilidad(ref List<InflexibilidadDto> inflexibilidades, InflexibilidadFiltroDto inflexibilidadFiltroDto)
        {
            if (inflexibilidades != null && inflexibilidades.Count > 0)
            {
                if (!string.IsNullOrEmpty(inflexibilidadFiltroDto.NombreInflexibilidad))
                {
                    inflexibilidades = inflexibilidades.Where(p => !string.IsNullOrEmpty(p.NombreInflexibilidad) && p.NombreInflexibilidad.ToLower().Contains(inflexibilidadFiltroDto.NombreInflexibilidad.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(inflexibilidadFiltroDto.Estado))
                {
                    inflexibilidades = inflexibilidades.Where(p => !string.IsNullOrEmpty(p.Estado) && p.Estado.Contains(inflexibilidadFiltroDto.Estado)).ToList();
                }
                if (inflexibilidadFiltroDto.AnioInicio.HasValue)
                {
                    inflexibilidades = inflexibilidades.Where(p => p.FechaInicio.Year == inflexibilidadFiltroDto.AnioInicio).ToList();
                }
                if (inflexibilidadFiltroDto.AnioFin.HasValue)
                {
                    inflexibilidades = inflexibilidades.Where(p => p.FechaFin.Year == inflexibilidadFiltroDto.AnioFin).ToList();
                }
                if (inflexibilidadFiltroDto.ValorPagado.HasValue)
                {
                    inflexibilidades = inflexibilidades.Where(p => p.ValorPagado == inflexibilidadFiltroDto.ValorPagado).ToList();
                }
                if (inflexibilidadFiltroDto.ValorTotal.HasValue)
                {
                    inflexibilidades = inflexibilidades.Where(p => p.ValorTotal == inflexibilidadFiltroDto.ValorTotal).ToList();
                }
            }
        }


        /// <summary>
        /// Obtener lista de la Inflexibilidad
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de Inflexibilidad</returns>
        public async Task<List<InflexibilidadPagosDto>> ObtenerInflexibilidadPagos(int idInflexibilidad, string usuarioDnp)
        {
            // Obtener Informaciones de los proyectos de lo servicio PiipCore
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriObtenerInflexibilidadPagos = ConfigurationManager.AppSettings["uriObtenerInflexibilidadPagos"];
            var parametros = $"?idInflexibilidad={idInflexibilidad}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, apiPiipCore, uriObtenerInflexibilidadPagos, parametros, null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<InflexibilidadPagosDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> ActualizarIdArchivoInflexibilidadPagos(InflexibilidadPagosDto pago, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriActulizarIdArchivoInflexibilidadPagos"];
            var apiPiipCore = ConfigurationManager.AppSettings["ApiPiipCore"];
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, apiPiipCore, uriMetodo, string.Empty, pago, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }

        /// <summary>
        /// Obtener lista Flujos
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de Flujos</returns>
        public async Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId, string usuarioDnp)
        {
            // Obtener Informaciones de los proyectos de lo servicio ServicioNegocios
            var apiServicioNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriObtenerMatrizFlujo = ConfigurationManager.AppSettings["uriObtenerMatrizFlujo"];
            var parametros = $"?entidadResponsableId={entidadResponsableId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, apiServicioNegocio, uriObtenerMatrizFlujo, parametros, null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<MatrizEntidadDestinoAccionDto>>(respuesta);
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorIdUsuarioDnp(string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorIdUsuarioDnp"];
            var parametros = $"?idUsuarioDnp={idUsuarioDNP}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<UsuarioDto>(respuesta);
        }

        public async Task<PermisosEntidadDto> ObtenerPermisosPorEntidad(string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPermisosPorEntidad"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, string.Empty, null, idUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<PermisosEntidadDto>(respuesta);
        }

        public async Task<List<RolDto>> ObtenerRolesPorUsuario(UsuarioDto dto)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorUsuario"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, dto, dto.IdUsuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
        }

        /// <summary>
        /// Obtener tipos cargas datos + cargas datos por tipoentidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<TipoCargaDatosDto>> ObtenerCargaDatosPorTipoYTipoEntidad(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCargaDatosPorTipoYTipoEntidad"];

            var parametros = $"?tipoEntidad={tipoEntidad}";
            var respuesta = JsonConvert.DeserializeObject<List<TipoCargaDatosDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }


        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXUsuarioAutenticado(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoEntidadesXUsuarioAutenticado"];

            var parametros = $"?usuarioDnp={usuarioDnp}";

            var respuesta = JsonConvert.DeserializeObject<List<ListadoEntidadDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado"];

            var parametros = $"?tipoEntidad={tipoEntidad}&usuarioDnp={usuarioDnp}";

            var respuesta = JsonConvert.DeserializeObject<List<ListadoEntidadDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidadBanco(string idEntidad, string usuarioDnp, int resourceGroupId)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoPerfilesXEntidadBanco"];

            var parametros = $"?idEntidad={idEntidad}&usuarioDnp={usuarioDnp}&resourceGroupId={resourceGroupId}";

            var respuesta = JsonConvert.DeserializeObject<List<ListadoPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidad(string idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoPerfilesXEntidad"];

            var parametros = $"?idEntidad={idEntidad}&usuarioDnp={usuarioDnp}";

            var respuesta = JsonConvert.DeserializeObject<List<ListadoPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidadYUsuario(string idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoPerfilesXEntidadYUsuario"];

            var parametros = $"?idEntidad={idEntidad}&usuarioDnp={usuarioDnp}";

            var respuesta = JsonConvert.DeserializeObject<List<ListadoPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Obtener listado entidades relacionadas al usuario autenticado
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<List<DNP.Backbone.Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto>> ObtenerListadoPerfilesXUsuarioTerritorio(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListadoPerfilesXUsuarioTerritorio"];

            var parametros = $"?&usuarioDnp={usuarioDnp}";

            var respuesta = JsonConvert.DeserializeObject<List<DNP.Backbone.Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        /// <summary>
        /// Guardar datos MongoDB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public RespuestaGeneralDto GuardarDatosMongoDB(dynamic data, string idSql)
        {
            return _autorizacionPersistencia.GuardarDatosMongoDB(data, idSql);
        }

        /// <summary>
        /// Obtener datos MongoDB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic ObtenerDatosMongoDb(string id)
        {
            return _autorizacionPersistencia.ObtenerDatosMongoDb(id);
        }

        /// <summary>
        /// Crear nueva carga de datos
        /// </summary>
        /// <param name="entidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> GuardarDatos(CargaDatosDto cargaDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarCargaDatos"];

            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, cargaDto, usuarioDnp));

            return respuesta;
        }

        /// <summary>
        /// Eliminar carga de datos
        /// </summary>
        /// <param name="entidadDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> EliminarCargaDatos(int id, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarCargaDatos"];
            var peticion = new CargaDatosDto() { Id = id };
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, peticion, usuarioDnp, useJWTAuth: false));

            return respuesta;
        }
        public async Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidadYUsuario(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesPorTipoEntidadYUsuario"];
            var parametros = $"?tipoEntidad={tipoEntidad}&usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<EntidadFiltroDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<EntidadNegocioDto>> ObtenerEntidadesConRoleVisualizador(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerEntidadesConRoleVisualizador"];
            var parametros = $"?idUsuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: true));
        }

        /// <summary>
        /// Obtener Usuarios por Nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Una lista de Usuarios</returns>
        public async Task<List<UsuarioAuthDto>> ObtenerUsuariosPorNombre(string nombre, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosPorNombre"];
            var parametros = $"?nombre={nombre}";

            return JsonConvert.DeserializeObject<List<UsuarioAuthDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, useJWTAuth: false));
        }

        #region MÉTODOS PRIVADOS

        private async Task<ICollection<RolNegocioEntidadDestinoDto>> ConsultarConfiguraciones(string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarConfiguraciones"];
            var parametros = $"?tipoEntidad={tipoEntidad}";

            return JsonConvert.DeserializeObject<List<RolNegocioEntidadDestinoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        private async Task<ICollection<EntidadNegocioDto>> ConsultarEntidadesTerritoriales(string usuarioDnp, bool esUsuarioGlobal, string tipoEntidad)
        {
            if (esUsuarioGlobal)
                return await _cacheEntidadesNegocioServicios.ConsultarEntidadesPorTipoEntidad(usuarioDnp, tipoEntidad);

            return await ConsultarEntidadesTerritorialesUsuarioLocal(usuarioDnp);
        }

        private async Task<ICollection<EntidadNegocioDto>> ConsultarEntidadesTerritorialesUsuarioLocal(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarEntidadesTerritorialesUsuarioLocal"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        private async Task<ICollection<RolNegocioDto>> ConsultarRoles(string usuarioDnp, bool esUsuarioGlobal)
        {
            if (esUsuarioGlobal)
                return await _cacheEntidadesNegocioServicios.ConsultarRoles(usuarioDnp);

            return await ConsultarRolesUsuarioLocal(usuarioDnp);
        }

        private async Task<ICollection<RolNegocioDto>> ConsultarRolesUsuarioLocal(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarRolesUsuarioLocal"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<RolNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        private async Task<ICollection<SectorNegocioDto>> ConsultarSectores(string usuarioDnp, bool esUsuarioGlobal)
        {
            if (esUsuarioGlobal)
                return await _cacheEntidadesNegocioServicios.ConsultarSectores(usuarioDnp);

            return await ConsultarSectoresUsuarioLocal(usuarioDnp);
        }

        private async Task<ICollection<SectorNegocioDto>> ConsultarSectoresUsuarioLocal(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarSectoresUsuarioLocal"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<SectorNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        private async Task<ICollection<EntidadNegocioDto>> ConsultarEntidadesDestino(string usuarioDnp, bool esUsuarioGlobal, string tipoEntidad)
        {
            if (esUsuarioGlobal)
                return await _cacheEntidadesNegocioServicios.ConsultarEntidadesPorTipoEntidad(usuarioDnp, tipoEntidad);

            return await ConsultarEntidadesUsuarioLocal(usuarioDnp);
        }

        private async Task<ICollection<EntidadNegocioDto>> ConsultarEntidadesUsuarioLocal(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarEntidadesUsuarioLocal"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        private static void ConstruirRespuestaHttp(AutorizacionPermisoDto respuestaAutorizacion)
        {
            var mensaje = string.Empty;
            var valido = false;

            var tienePermiso = respuestaAutorizacion.Permiso;
            var estados = respuestaAutorizacion.Estados;

            if (estados.Contains((int)EstadoAutorizacion.UsuarioNoExiste))
                mensaje = BackboneRecursos.CredencialesInvalidas;
            else if (estados.Contains((int)EstadoAutorizacion.AplicacionNoExiste))
                mensaje = BackboneRecursos.AplicacionNoExiste;
            else if (estados.Contains((int)EstadoAutorizacion.OpcionNoExiste))
                mensaje = BackboneRecursos.ServicioNoExiste;
            else if (estados.Contains((int)EstadoAutorizacion.UsuarioSinPermisosParaLaAplicacionUOpcion))
                mensaje = BackboneRecursos.UsuarioNoTienePermisos;
            else if (estados.Contains((int)EstadoAutorizacion.AutenticacionNoValidaDeLaAplicacionCliente))
                mensaje = BackboneRecursos.AplicacionNoExiste;
            else if (estados.Contains((int)EstadoAutorizacion.ErrorIndefinido))
                mensaje = BackboneRecursos.ErrorIndefinido;
            else if (!tienePermiso)
                mensaje = BackboneRecursos.UsuarioNoTienePermisos;
            else
                valido = true;

            if (!valido)
                throw new BackboneResponseException(HttpStatusCode.Unauthorized, mensaje);

        }

        private void ValidarParametros(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            var mensaje = string.Empty;
            var valido = false;

            if (string.IsNullOrEmpty(nombreUsuario))
                mensaje = string.Format(BackboneRecursos.ParametroNoRecibido, "nombreUsuario");
            else if (string.IsNullOrEmpty(hashUsuario))
                mensaje = string.Format(BackboneRecursos.ParametroNoRecibido, "hashUsuario");
            else if (string.IsNullOrEmpty(idAplicacion))
                mensaje = string.Format(BackboneRecursos.ParametroNoRecibido, "idAplicacion");
            else if (string.IsNullOrEmpty(nombreServicio))
                mensaje = string.Format(BackboneRecursos.ParametroNoRecibido, "nombreServicio");
            else
                valido = true;

            if (!valido)
                throw new BackboneResponseException(HttpStatusCode.BadRequest, mensaje);
        }

        private async Task<AutorizacionPermisoDto> ObtenerPermisosUsuario(string nombreUsuario, string idAplicacion,
                                                                  string nombreServicio)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPermiso"];
            var parametros = $"?idAplicacion={idAplicacion}&idOpcion={nombreServicio}";

            return JsonConvert.DeserializeObject<AutorizacionPermisoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, nombreUsuario, useJWTAuth: false));
        }

        private async Task<bool> UsuarioEsAdministradorGlobal(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarUsuarioEsAdministradorGlobal"];
            var parametros = $"?usuarioDnp={usuarioDnp}";

            return Convert.ToBoolean(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        //public Task<List<UsuarioDto>> ObtenerUsuarios(string tipoEntidad)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region Adherencia

        public async Task<List<AdherenciaDto>> ObtenerAdherenciasPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerAdherenciasPorId"];
            var parametros = $"?idEntidad={idEntidad}";

            return JsonConvert.DeserializeObject<List<AdherenciaDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<RespuestaGeneralDto> GuardarAdherencia(AdherenciaDto adherenciaDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings[adherenciaDto.AdherenciaId != 0 ? "uriEditarAdherencia" : "uriCrearAdherencia"];

                var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, adherenciaDto, usuarioDnp, useJWTAuth: false));

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> EliminarAdherencia(int idAdherencia, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriEliminarAdherencia"];
                AdherenciaDto adherencia = new AdherenciaDto()
                {
                    AdherenciaId = idAdherencia
                };
                return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, adherencia, usuarioDnp, useJWTAuth: false));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Adherencia

        #region Delegado

        public async Task<List<DelegadoDto>> ObtenerDelegadosPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDelegadosPorId"];
            var parametros = $"?idEntidad={idEntidad}";

            return JsonConvert.DeserializeObject<List<DelegadoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<RespuestaGeneralDto> GuardarDelegado(DelegadoDto delegadoDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings[delegadoDto.DelegadoId != 0 ? "uriEditarDelegado" : "uriCrearDelegado"];

                var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, delegadoDto, usuarioDnp, useJWTAuth: false));

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> EliminarDelegado(int idDelegado, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriEliminarDelegado"];
                DelegadoDto adherencia = new DelegadoDto()
                {
                    DelegadoId = idDelegado
                };
                return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, adherencia, usuarioDnp, useJWTAuth: false));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Delegado

        /// <summary>
        /// Obtener roles por Id Usuario Perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<List<RolDto>> ObtenerRolesPorIdUsuarioPerfil(Guid idUsuarioPerfil, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorIdUsuarioPerfil"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var request = new ParametrosPerfilDto()
            {
                IdUsuarioPerfil = idUsuarioPerfil,
                IdAplicacion = guidBackBone
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
        }

        /// <summary>
        /// Obtener roles por Id Usuario Perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<List<RespuestaUsuariosConfiguracionDto>> ObtenerUsuariosPorNombreIdentificacion(ParametrosUsuariosConfiguracionDto filtro, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosPorNombreIdentificacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, filtro, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RespuestaUsuariosConfiguracionDto>>(respuesta);
        }

        /// <summary>
        /// Obtener roles por Id Usuario Perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<bool> GuardarSectoresPorUsuarioEntidad(List<RespuestaSectoresUsuarioConfiguracionDto> data, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarSectoresPorUsuarioEntidad"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, data, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        /// <summary>
        /// Obtener roles por Id Usuario Perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<List<RespuestaSectoresUsuarioConfiguracionDto>> ObtenerSectoresPorUsuarioEntidad(ParametrosSectoresUsuarioConfiguracionDto filtro, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSectoresPorUsuarioEntidad"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, filtro, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RespuestaSectoresUsuarioConfiguracionDto>>(respuesta);
        }

        /// <summary>
        /// Obtener roles por Id Usuario Perfil
        /// </summary>
        /// <param name="idUsuarioPerfil"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>Una lista de RolDto</returns>
        public async Task<List<RolDto>> ObtenerRolesPorIdsUsuarioPerfil(List<Guid> idsUsuarioPerfil, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolesPorIdsUsuarioPerfil"];
            var guidBackBone = new Guid(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]);

            var request = new ParametrosPerfilDto()
            {
                IdsUsuarioPerfil = idsUsuarioPerfil,
                IdAplicacion = guidBackBone
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<RolDto>>(respuesta);
        }

        /// <summary>
        /// metodo que obtiene los usuarios de las subdirecciones tecnicas a partir del entityTypeCatalogOptionId
        /// </summary>
        /// <param name="entityTypeCatalogOptionId"></param>
        /// <returns>lista de UsuarioDto</returns>
        public async Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosPorSubDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto)
        {
            int entityTypeCatalogOptionId = Convert.ToInt32(peticionObtenerProyecto.IdFiltro);
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosPorSubDireccionTecnica"];
            var parametros = $"?entityTypeCatalogOptionId={entityTypeCatalogOptionId}";

            List<UsuarioAnalistaConceptoDto> lst = JsonConvert.DeserializeObject<List<UsuarioAnalistaConceptoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, peticionObtenerProyecto.IdUsuario, useJWTAuth: false));
            return lst;
        }

        public async Task<IEnumerable<OpcionDto>> ObtenerOpcionesConFiltro(string idAplicacion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerOpcionesConFiltro"];
            var parametros = $"?idAplicacion={idAplicacion}";
            IEnumerable<OpcionDto> lst = JsonConvert.DeserializeObject<IEnumerable<OpcionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return lst;
        }

        /// <summary>
        /// metodo que obtiene los usuarios de las subdirecciones tecnicas  por rol RValidadorPoliticaTransversal
        /// </summary>
        /// <param name="entityTypeCatalogOptionId"></param>
        /// <returns>lista de UsuarioDto</returns>
        public async Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosRValidadorPoliticaTransversal(ProyectoParametrosDto peticionObtenerProyecto)
        {
            int entityTypeCatalogOptionId = Convert.ToInt32(peticionObtenerProyecto.IdFiltro);
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosRValidadorPoliticaTransversal"];
            var parametros = $"?entityTypeCatalogOptionId={entityTypeCatalogOptionId}";

            List<UsuarioAnalistaConceptoDto> lst = JsonConvert.DeserializeObject<List<UsuarioAnalistaConceptoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, peticionObtenerProyecto.IdUsuario, useJWTAuth: false));
            return lst;
        }

        public async Task<Dominio.Dto.AutorizacionNegocio.UsuarioCuentaDto> ObtenerCuentaUsuario(string nomeCuenta, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCuentaUsuario"];
            var parametros = $"?nomeCuenta={nomeCuenta}";
            var rta = JsonConvert.DeserializeObject<Dominio.Dto.AutorizacionNegocio.UsuarioCuentaDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        public async Task<Dominio.Dto.AutorizacionNegocio.UsuarioDto> ObtenerUsuarioPorCorreoDNP(string correo, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorCorreoDNP"];
            var parametros = $"?correo={correo}";
            var rta = JsonConvert.DeserializeObject<Dominio.Dto.AutorizacionNegocio.UsuarioDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        public async Task<List<EntidadFiltroDto>> ObtenerListaEntidad(string usuarioDnp,string objetoNegocioId)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListaEntidad"];
            var parametros = $"?usuarioDnp={usuarioDnp}&objetoNegocioId={objetoNegocioId}";

            List<EntidadFiltroDto> lst = JsonConvert.DeserializeObject<List<EntidadFiltroDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return lst;
        }
        public async Task<string> validarPermisoInactivarUsuario(string usuarioDnp, string usuarioDnpEliminar)
        {
            var uriMetodo = ConfigurationManager.AppSettings["urivalidarPermisoInactivarUsuario"];
            var rta = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, $"?usuarioDnp=" + usuarioDnp + "&usuarioDnpEliminar=" + usuarioDnpEliminar, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        public async Task<IEnumerable<EntidadFiltroConsolaDto>> ObtnerEntidadesPorSector(int sectorId, string tipoEntidad, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtnerEntidadesPorSector"];
            var parametros = $"?sectorId={sectorId}&tipoEntidad={tipoEntidad}";

            IEnumerable<EntidadFiltroConsolaDto> lst = JsonConvert.DeserializeObject<IEnumerable<EntidadFiltroConsolaDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return lst;
        }

        /// <summary>
        /// funcion que se encarga de traer el Encabezado Listado Reporte
        /// </summary>
        /// <returns></returns>
        public async Task<string> ObtenerEncabezadoListadoReportesPIIP(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerEncabezadoListadoReportesPIIP"];
            var rta = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        /// <summary>
        /// funcion que se encarga de traer el listado de ReportesPIIP
        /// </summary>
        /// <returns></returns>
        public async Task<string> ObtenerListadoReportesPIIP(string usuarioDnp, string idRoles)
        {
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerListadoReportesPIIP"];
            var parametros = $"?idRoles={idRoles}";
            var rta = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        /// <summary>
        /// funcion que se encarga de Obtener los FiltrosReportesPIIP
        /// </summary>
        /// <returns></returns>
        public async Task<string> ObtenerFiltrosReportesPIIP(Guid idReporte, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerFiltrosReportesPIIP"] + $"?idReporte={idReporte}" + $"&idUsuarioDnp={usuarioDnp}";
            var rta = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false));
            return rta;
        }

        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        public async Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDnp, string idEntidades)
        {
            try
            {
                var parametros = $"?usuarioDnp={usuarioDnp}";
                var uriMetodo = ConfigurationManager.AppSettings["urlObtenerDatosReportePIIP"] + $"?idReporte={idReporte}" + $"&filtros={filtros}" + $"&idEntidades={idEntidades}";
                var rta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, null, usuarioDnp, useJWTAuth: false);
                return rta;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        /// <summary>
        /// Método que obtiene los usuarios por rol y entidad
        /// </summary>
        /// <param name="entidadRol"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns>lista de UsuarioDto</returns>
        public async Task<List<AutorizacionUsuarioEntidadRolDto>> ObtenerUsuariosBasicosPorRolEntidad(EntidadRolDto entidadRol, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosBasicosPorRolEntidad"];
            List<AutorizacionUsuarioEntidadRolDto> lst = JsonConvert.DeserializeObject<List<AutorizacionUsuarioEntidadRolDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, entidadRol, usuarioDnp, useJWTAuth: false));
            return lst;
        }
    }
}
