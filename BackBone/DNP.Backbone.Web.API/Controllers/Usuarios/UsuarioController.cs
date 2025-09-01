using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Utilidades;
using DNP.Backbone.Comunes.Utilidades.AutorizacionAttributes;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Identidad;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using UsuarioDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioDto;
using UsuarioPerfilDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilDto;
using UsuarioPerfilProyectoDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilProyectoDto;

namespace DNP.Backbone.Web.API.Controllers.Usuarios
{
    public class UsuarioController : Base.BackboneBase
    {
        private static readonly string _jwtTokenServicio;

        static UsuarioController()
        {
            var secret = ConfigurationManager.AppSettings["JwtSecret-Servicio"];

            var claims = new List<Claim>();
            claims.Add(new Claim("Tipo", "servicio"));
            claims.Add(new Claim("Nombre-Servicio", "DNP.Backbone.Web.API"));

            var tokenManager = new JwtTokenManager(secret);
            _jwtTokenServicio = tokenManager.EscribirToken(claims, DateTime.UtcNow.AddYears(5));
        }

        private readonly IAutorizacionServicios _autorizacionServicios;
        private readonly IIdentidadServicios _identidadServicios;
        private readonly IFlujoServicios _flujoServicios;

        public UsuarioController(IAutorizacionServicios autorizacionServicios, IIdentidadServicios identidadServicios, IFlujoServicios flujoServicios)
            : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
            _identidadServicios = identidadServicios;
            _flujoServicios = flujoServicios;
        }

        /// <summary>
        /// Obtener datos de registro de usuario
        /// </summary>
        /// <param name="usuarioGuid">Guid del Usuario DNP</param>
        [HttpGet]
        [Route("api/Usuario/ObtenerDatosVisualizacion")]
        public async Task<IHttpActionResult> ObtenerDatosVisualizacion(string usuarioGuid = null, string usuarioDnp = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuarioDnp) && string.IsNullOrWhiteSpace(usuarioGuid))
                    return BadRequest();

                var usuario = !string.IsNullOrWhiteSpace(usuarioGuid) ?
                    await _autorizacionServicios.ObtenerUsuarioPorId(usuarioDnp, usuarioGuid) :
                    await _autorizacionServicios.ObtenerUsuarioPorIdUsuarioDnp(usuarioDnp);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [HttpGet]
        [Route("api/Usuario/ObtenerUsuarioPorIdUsuarioDnp")]
        public async Task<IHttpActionResult> ObtenerUsuarioPorIdUsuarioDnp(string usuarioDnp = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuarioDnp))
                    return BadRequest();

                var usuario = await _autorizacionServicios.ObtenerUsuarioPorIdUsuarioDnp(usuarioDnp);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        /// <summary>
        /// Obtener los usuarios filtrados por tipo de entidad
        /// </summary>
        /// <param name="tipoEntidad">Tipo de entidad filtrada</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuarios")]
        public async Task<IHttpActionResult> ObtenerUsuarios()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuarios(string.Empty, UsuarioLogadoDto.IdUsuario));
            return Ok(result);

            //return Ok();
        }

        /// <summary>
        /// Obtener lista de las entidades, filtradas por tipo de la entidad
        /// </summary>
        /// <param name="tipoEntidad">Tipo de entidad filtrada</param>
        [HttpGet]
        [Route("api/Usuario/ObtenerEntidadesInviteUsuario")]
        public async Task<IHttpActionResult> ObtenerEntidadesInviteUsuario(string tipoEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadesInviteUsuario(UsuarioLogadoDto.IdUsuario, tipoEntidad, RequestContext.Principal));
            return Ok(result);
        }

        /// <summary>
        /// Enviar una invitación a un usuario para acceder a los sistemas de Administración y / o Backbone
        /// </summary>
        [HttpPost]
        [Route("api/Usuario/InvitarUsuario")]
        public async Task<IHttpActionResult> InvitarUsuario(InvitarUsuarioDto dto)
        {
            try
            {
                if(dto.InvitacionValida)
                    await _identidadServicios.InvitarUsuario(dto, UsuarioLogadoDto.IdUsuario, esUsuarioBackbone: dto.TieneModuloBackbone);

                var result = await _autorizacionServicios.CrearUsuarioInvitado(dto, UsuarioLogadoDto.IdUsuario);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
        }


        /// <summary>
        /// Enviar una invitación a un usuario para acceder a los sistemas de Administración y / o Backbone
        /// </summary>
        [HttpPost]
        [Route("api/Usuario/RegistrarUsuarioPIIP")]
        public async Task<IHttpActionResult> RegistrarUsuarioPIIP(UsuarioPIIPDto dto)
        {
            try
            {
                //if (dto.RegistroValido)
                //    await _identidadServicios.RegistrarUsuarioPIIP(dto, UsuarioLogadoDto.IdUsuario, esUsuarioBackbone: dto.TieneModuloBackbone);

                var result = await _autorizacionServicios.CrearUsuarioPIIP(dto, UsuarioLogadoDto.IdUsuario);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
        }

        /// <summary>
        /// Enviar una invitación a un usuario para acceder a los sistemas de Administración y / o Backbone
        /// </summary>
        [HttpPost]
        [Route("api/Usuario/RegistrarUsuarioTerritorioPIIP")]
        public async Task<IHttpActionResult> RegistrarUsuarioPIIP(UsuarioPIIPTerritorioDto dto)
        {
            try
            {
                //if (dto.RegistroValido)
                //    await _identidadServicios.RegistrarUsuarioPIIP(dto, UsuarioLogadoDto.IdUsuario, esUsuarioBackbone: dto.TieneModuloBackbone);

                var result = await _autorizacionServicios.CrearUsuarioTerritorioPIIP(dto, UsuarioLogadoDto.IdUsuario);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
        }

        /// <summary>
        /// Crear la UsuarioPerfil.
        /// </summary>
        /// <returns>Elimina Aplicacion.</returns>
        [Route("api/Usuario/CrearUsuarioPerfil")]
        [HttpPost]
        //[MenuAuthorize("Usuarios")]
        //[OpcionAuthorize("Usuarios:AsignarPerfiles")]
        public async Task<IHttpActionResult> CrearUsuarioPerfil([FromBody] UsuarioPerfilDto dto)
        {
            var entidad = await Task.Run(() => _autorizacionServicios.ObtenerEntidadPorEntidadId(dto.IdEntidad, UsuarioLogadoDto.IdUsuario));
            if (entidad == null || !entidad.EntityTypeCatalogOptionId.HasValue)
            {
                return Ok(new { Exitoso = false, Message = "Entidade não possuí EntityCatalogOptionId e portanto não será dado as permissões as instancias." });
            }

            var result = await _autorizacionServicios.CrearUsuarioPerfil(dto, UsuarioLogadoDto.IdUsuario);

            if (result)
            {
                var rolesDto = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorPerfil(dto.IdPerfil, UsuarioLogadoDto.IdUsuario));
                var roles = rolesDto.Select(r => r.IdRol).ToList();

                await _flujoServicios.RegistrarPermisosInstancias(new Comunes.Dto.ParametrosObjetosNegocioDto
                {
                    IdsRoles = roles,
                    IdUsuarioDNP = UsuarioLogadoDto.IdUsuario,
                    EntidadId = entidad.EntityTypeCatalogOptionId,
                    UsuarioDNP = dto.UsuarioDNP
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de perfiles
        /// </summary>
        /// <returns>Objeto de entidad</returns>
        [Route("api/Usuario/ObtenerTodosPerfiles")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTodosPerfiles()
        {
            var result = await Task.Run(async () => await _autorizacionServicios.ObtenerTodosPerfiles(UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de entidades
        /// </summary>
        /// <returns>Objeto de entidad</returns>
        [Route("api/Usuario/ObtenerTodasEntidades")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTodasEntidades()
        {
            var result = await Task.Run(async () => await _autorizacionServicios.ObtenerTodasEntidades(UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        /// <summary>
        /// Obtener los perfiles asignados ao usuario agrupados por entidad
        /// </summary>
        /// <param name="usuarioDnp">Usuario DNP</param>
        /// <param name="tipoEntidad">Tipo de entidad filtrada</param>
        /// <returns>Una lista de PerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerPerfiles/{perfilFiltro?}")]
        public async Task<IHttpActionResult> ObtenerPerfiles(string perfilFiltro = "")
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerPerfiles(UsuarioLogadoDto, perfilFiltro));
            return Ok(result);
        }

        /// <summary>
        /// Obtener los perfiles asignados ao usuario agrupados por aplicacion
        /// </summary>
        /// <param name="usuarioDnp"></param>
        /// <param name="aplicacion"></param>
        /// <returns>Una lista de PerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerPerfilesPorAplicacion")]
        public async Task<IHttpActionResult> ObtenerPerfilesPorAplicacion(string usuarioDnp, string aplicacion)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerPerfilesPorAplicacion(usuarioDnp, aplicacion));
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <param name="idPerfil"></param>
        /// <param name="aplicacion"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerPerfilesAutorizadosPorAplicacion")]
        public async Task<IHttpActionResult> ObtenerPerfilesAutorizadosPorAplicacion()
        {
            string idAplicacion = UsuarioLogadoDto.GuidPIIPAplicacion.ToString();
            string idPerfil = UsuarioLogadoDto.GuidPIIPAplicacion.ToString();
            string usuarioDnp = UsuarioLogadoDto.IdUsuario;
            var result = await Task.Run(() => _autorizacionServicios.ObtenerPerfilesAutorizadosPorAplicacion(idAplicacion, idPerfil, usuarioDnp));
            return Ok(result);
        }

        /// <summary>
        /// obtener perfiles de usuario por tipo de entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <returns>Una Lista de EntidadPerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerPerfilesUsuario")]
        public async Task<IHttpActionResult> ObtenerPerfilesUsuario(string tipoEntidad, string idUsuario = null)
        {
            try
            {
                var idUsuarioDNP = UsuarioLogadoDto.IdUsuario;
                if (idUsuario != null)
                {
                    var usuario = await Task.Run(() => _autorizacionServicios.ObtenerUsuarioPorId(idUsuarioDNP, idUsuario));
                    idUsuarioDNP = usuario.IdUsuarioDnp;
                }

                var result = await Task.Run(() => _autorizacionServicios.ObtenerPerfilesPorUsuario(idUsuarioDNP, tipoEntidad));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtener perfiles de usuario por tipo de entidad por entidad relacionada al usuario autenticado
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="idUsuario"></param>
        /// <returns>Una Lista de EntidadPerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerPerfilesUsuarioXUsuarioAutenticado")]
        public async Task<IHttpActionResult> ObtenerPerfilesUsuarioXUsuarioAutenticado(string tipoEntidad, string idUsuario = null)
        {
            try
            {
                var idUsuarioDNPAutenticado = UsuarioLogadoDto.IdUsuario;
                var idUsuarioDNP = UsuarioLogadoDto.IdUsuario;
                if (idUsuario != null)
                {
                    var usuario = await Task.Run(() => _autorizacionServicios.ObtenerUsuarioPorId(idUsuarioDNP, idUsuario));
                    idUsuarioDNP = usuario.IdUsuarioDnp;
                }

                var result = await Task.Run(() => _autorizacionServicios.ObtenerPerfilesPorUsuarioXUsuarioAutenticado(idUsuarioDNP, tipoEntidad, idUsuarioDNPAutenticado));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtener perfiles de usuario por entidad relacionada al usuario seleccionado
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <param name="idUsuario"></param>
        /// <returns>Una Lista de EntidadPerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerListadoPerfilesXEntidadYUsuario")]
        public async Task<IHttpActionResult> ObtenerListadoPerfilesXEntidadYUsuario(string idEntidad, string idUsuario = null)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario));
            return Ok(result);
        }

        /// <summary>
        /// obtener perfiles relacionados al usuario seleccionado
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>Una Lista de UsuarioPerfilDto</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerListadoPerfilesXUsuarioTerritorio")]
        public async Task<IHttpActionResult> ObtenerListadoPerfilesXUsuarioTerritorio(string idUsuario)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoPerfilesXUsuarioTerritorio(idUsuario));
            return Ok(result);
        }

        /// <summary>
        /// Obtener usuarios por entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una Lista de Usario Entidad</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosPorEntidad")]
        [MenuAuthorize("Usuarios")]
        public async Task<IHttpActionResult> ObtenerUsuariosPorEntidad(String tipoEntidad, String filtro, String filtroExtra)
        {
            try
            {
                /// <!--NOTA: Donde filtroExtra es un objeto anonimo { NomberUsuario: String, CuentaUsuario: String, Estado: bool }-->
                var tipoAnonimo = new { NombreUsuario = String.Empty, CuentaUsuario = String.Empty, Estado = (bool?)null };

                // deserializar
                var tipoAnonimoFiltros = JsonConvert.DeserializeAnonymousType(filtroExtra, tipoAnonimo);

                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosPorEntidad(tipoEntidad, filtro, tipoAnonimoFiltros.NombreUsuario, tipoAnonimoFiltros.CuentaUsuario, tipoAnonimoFiltros.Estado, UsuarioLogadoDto.IdUsuario, RequestContext.Principal));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener usuarios por entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una Lista de Usario Entidad</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosPorEntidadSp")]
        [MenuAuthorize("Usuarios")]
        public async Task<IHttpActionResult> ObtenerUsuariosPorEntidadSp(String tipoEntidad, String filtro, String filtroExtra)
        {
            try
            {
                /// <!--NOTA: Donde filtroExtra es un objeto anonimo { NomberUsuario: String, CuentaUsuario: String, Estado: bool }-->
                var tipoAnonimo = new { NombreUsuario = String.Empty, CuentaUsuario = String.Empty, Estado = (bool?)null };

                // deserializar
                var tipoAnonimoFiltros = JsonConvert.DeserializeAnonymousType(filtroExtra, tipoAnonimo);

                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosPorEntidadSp(tipoEntidad, filtro, tipoAnonimoFiltros.NombreUsuario, tipoAnonimoFiltros.CuentaUsuario, tipoAnonimoFiltros.Estado, UsuarioLogadoDto.IdUsuario, RequestContext.Principal));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener usuarios por entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una Lista de Usario Entidad</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosXEntidad")]
        [MenuAuthorize("Usuarios")]
        public async Task<IHttpActionResult> ObtenerUsuariosXEntidad(String tipoEntidad, String filtro, String filtroExtra)
        {
            return await ObtenerUsuariosXEntidadComun(tipoEntidad, filtro, filtroExtra);
        }

        private async Task<IHttpActionResult> ObtenerUsuariosXEntidadComun(String tipoEntidad, String filtro, String filtroExtra)
        {
            try
            {
                /// <!--NOTA: Donde filtroExtra es un objeto anonimo { NomberUsuario: String, CuentaUsuario: String, Estado: bool }-->
                var tipoAnonimo = new { NombreUsuario = String.Empty, CuentaUsuario = String.Empty, Estado = (bool?)null };

                // deserializar
                var tipoAnonimoFiltros = JsonConvert.DeserializeAnonymousType(filtroExtra, tipoAnonimo);

                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosXEntidad(tipoEntidad, UsuarioLogadoDto.IdUsuario, filtroExtra, tipoAnonimoFiltros.NombreUsuario, tipoAnonimoFiltros.CuentaUsuario, tipoAnonimoFiltros.Estado, UsuarioLogadoDto.IdUsuario, RequestContext.Principal));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }

        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosXEntidadTerritorio")]
        [MenuAuthorize("Entidades")]
        public async Task<IHttpActionResult> ObtenerUsuariosXEntidadTerritorio(String tipoEntidad, String filtro, String filtroExtra)
        {
            return await ObtenerUsuariosXEntidadComun(tipoEntidad, filtro, filtroExtra);
        }

        [HttpPost]
        [Route("api/Usuario/EditarUsuario")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Usuarios:Editar")]
        public async Task<IHttpActionResult> EditarUsuario(UsuarioDto usuarioDto)
        {
            return await EditarUsuarioComun(usuarioDto);
        }

        private async Task<IHttpActionResult> EditarUsuarioComun(UsuarioDto usuarioDto)
        {
            usuarioDto.IdUsuarioDnp = UsuarioLogadoDto.IdUsuario;

            try
            {
                var result = await Task.Run(() => _autorizacionServicios.EditarUsuario(usuarioDto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Usuario/EditarUsuarioTerritorio")]
        [MenuAuthorize("Entidades")]
        [OpcionAuthorize("Usuarios:Editar")]
        public async Task<IHttpActionResult> EditarUsuarioTerritorio(UsuarioDto usuarioDto)
        {
            return await EditarUsuarioComun(usuarioDto);
        }

        [HttpPost]
        [Route("api/Usuario/SetActivoUsuarioPerfilPorEntidad")]
        public async Task<IHttpActionResult> SetActivoUsuarioPerfil(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.SetActivoUsuarioPerfilPorEntidad(dto));
                
                //se retira el llamado al registro de permisos Instancias ya que se deja por defecto con todos permisos por instancia 16022023

                //List<Guid> roles = new List<Guid>();
                //var perfilesUsuario = await Task.Run(() => _autorizacionServicios.ObtenerPerfilesPorUsuario(dto.IdUsuarioDnp, dto.TipoEntidad));
                //var idsUsuarioPerfil = perfilesUsuario.SelectMany(p => p.Perfiles.Select(pe => pe.IdUsuarioPerfil)).ToList();
                //var rolesDto = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorIdsUsuarioPerfil(idsUsuarioPerfil, UsuarioLogadoDto.IdUsuario));
                //if (rolesDto != null && rolesDto.Any())
                //{
                //    roles = rolesDto.Select(r => r.IdRol).ToList();

                //    var entidad = await Task.Run(() => _autorizacionServicios.ObtenerEntidadPorEntidadId(dto.IdEntidad, UsuarioLogadoDto.IdUsuario));

                //if (dto.Activo)
                //{
                //    await _flujoServicios.RegistrarPermisosInstancias(new Comunes.Dto.ParametrosObjetosNegocioDto
                //    {
                //        IdsRoles = roles,
                //        IdUsuarioDNP = UsuarioLogadoDto.IdUsuario,
                //        EntidadId = entidad.EntityTypeCatalogOptionId,
                //        UsuarioDNP = dto.IdUsuarioDnp
                //    });
                //}
                //else
                //{
                //    await Task.Run(() => _flujoServicios.EliminarInstanciasPermiso(new Comunes.Dto.ParametrosObjetosNegocioDto
                //    {
                //        EntidadId = entidad?.EntityTypeCatalogOptionId,
                //        IdsRoles = roles,
                //        UsuarioDNP = dto.IdUsuarioDnp,
                //        IdUsuarioDNP = UsuarioLogadoDto.IdUsuario,
                //    }));
                //}
                //}

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Usuario/SetActivoUsuarioPerfil")]
        public async Task<IHttpActionResult> SetActivoUsuarioPerfil(SetActivoUsuarioPerfilDto dto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.SetActivoUsuarioPerfil(dto));

                var rolesDto = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorIdUsuarioPerfil(dto.IdUsuarioPerfil, UsuarioLogadoDto.IdUsuario));
                var roles = rolesDto.Select(r => r.IdRol).ToList();

                var entidad = await Task.Run(() => _autorizacionServicios.ObtenerEntidadPorEntidadId(dto.IdEntidad, UsuarioLogadoDto.IdUsuario));

                if (dto.Activo)
                {
                    await _flujoServicios.RegistrarPermisosInstancias(new Comunes.Dto.ParametrosObjetosNegocioDto
                    {
                        IdsRoles = roles,
                        IdUsuarioDNP = UsuarioLogadoDto.IdUsuario,
                        EntidadId = entidad.EntityTypeCatalogOptionId,
                        UsuarioDNP = dto.UsuarioDnp
                    });
                }
                else
                {
                    await Task.Run(() => _flujoServicios.EliminarInstanciasPermiso(new Comunes.Dto.ParametrosObjetosNegocioDto
                    {
                        EntidadId = entidad?.EntityTypeCatalogOptionId,
                        IdsRoles = roles,
                        UsuarioDNP = dto.UsuarioDnp,
                        IdUsuarioDNP = UsuarioLogadoDto.IdUsuario
                    }));
                }

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Usuario/SetActivoUsuarioEntidad")]
        public async Task<IHttpActionResult> SetActivoUsuarioEntidad(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.SetActivoUsuarioEntidad(dto));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Usuario/ObtenerExcelPerfiles")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcelPerfiles(string perfilFiltro)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var perfiles = await Task.Run(() => _autorizacionServicios.ObtenerPerfiles(UsuarioLogadoDto, perfilFiltro));

                var _result = new ExcelDto
                {
                    Mensaje = "No hay ningún resultado",
                    Reporte = "Información del perfil",
                    Columnas = new List<string> { "Perfil", "Roles" },
                    Data = perfiles.Select(x => new { x.NombrePerfil, x.RolesConcat }).ToList()
                };
                //                result.Content = ExcelUtilidades.ObtenerExcellPerfiles(_result);
                result.Content = ExcelUtilidades.ObtenerExcellComum(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return result;


            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Usuario/ObtenerExcelRoles")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcelRoles(string rolFiltro)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var perfiles = await Task.Run(() => _autorizacionServicios.ObtenerRoles(UsuarioLogadoDto.IdUsuario, rolFiltro));

                var _result = new ExcelDto
                {
                    Mensaje = "No hay ningún resultado",
                    Reporte = "Información del rol",
                    Columnas = new List<string> { "Rol" },
                    Data = perfiles.Select(x => new { x.Nombre, x.OpcionesConcat }).ToList()
                };

                result.Content = ExcelUtilidades.ObtenerExcellComum(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return result;


            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener la lista de roles para un perfil de usuario
        /// </summary>
        /// <param name="idPerfil">Guid del perfil del usuario</param>
        /// <param name="usuarioDnp">Guid del Usuario DNP</param>
        /// <returns>Una lista de roles</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerRolesDePerfil")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Roles:Crear|Roles:Editar")]
        public async Task<IHttpActionResult> ObtenerRolesDePerfil(Guid idPerfil, string usuarioDnp)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorPerfil(idPerfil, usuarioDnp));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Guardar Perfil
        /// </summary>
        /// <param name="perfilDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Usuario/GuardarPerfil")]
        public async Task<IHttpActionResult> GuardarPerfil(PerfilDto perfilDto)
        {
            try
            {
                return Ok(await Task.Run(() => _autorizacionServicios.GuardarPerfil(perfilDto)));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Perfil
        /// </summary>
        /// <param name="perfilDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Usuario/EliminarPerfil")]
        public async Task<IHttpActionResult> EliminarPerfil(PerfilDto perfilDto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.EliminarPerfil(perfilDto));
                return Ok(result.Exito);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Rol
        /// </summary>
        /// <param name="rolDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Usuario/EliminarUsuarioPerfil")]
        public async Task<IHttpActionResult> EliminarUsuarioPerfil(UsuarioDto usuarioDto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.EliminarUsuarioPerfil(usuarioDto.IdUsuarioPerfil, UsuarioLogadoDto.IdUsuario));
                if (!result.Exito)
                {
                    return Content(HttpStatusCode.Conflict, result);
                }

                var rolesDto = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorIdUsuarioPerfil(usuarioDto.IdUsuarioPerfil, UsuarioLogadoDto.IdUsuario));
                var roles = rolesDto.Select(r => r.IdRol).ToList();

                var entidad = await Task.Run(() => _autorizacionServicios.ObtenerEntidadPorEntidadId(usuarioDto.IdEntidad, UsuarioLogadoDto.IdUsuario));

                await Task.Run(() => _flujoServicios.EliminarInstanciasPermiso(new Comunes.Dto.ParametrosObjetosNegocioDto
                {
                    EntidadId = entidad?.EntityTypeCatalogOptionId,
                    IdsRoles = roles,
                    UsuarioDNP = usuarioDto.IdUsuarioDnp,
                    IdUsuarioDNP = UsuarioLogadoDto.IdUsuario
                }));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Guardar rol
        /// </summary>
        /// <returns>RespuestaGeneralDto</returns>
        [HttpPost]
        [Route("api/Usuario/GuardarRol")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Roles:Crear|Roles:Editar")]
        public async Task<IHttpActionResult> GuardarRol(RolDto rolDto)
        {
            try
            {
                return Ok(await Task.Run(() => _autorizacionServicios.GuardarRol(rolDto)));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Rol
        /// </summary>
        /// <returns>RespuestaGeneralDto</returns>
        [HttpPost]
        [Route("api/Usuario/EliminarRol")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Roles:Eliminar")]
        public async Task<IHttpActionResult> EliminarRol(RolDto rolDto)
        {
            try
            {
                return Ok(await Task.Run(() => _autorizacionServicios.EliminarRol(rolDto, UsuarioLogadoDto.IdUsuario)));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener roles
        /// </summary>
        /// <param name="roleFiltro"></param>
        /// <returns>Una lista de duplaDto</returns>
        [HttpGet]
        [Route("api/Usuario/obtenerRoles/{roleFiltro?}")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Roles:Crear|Roles:Editar")]
        public async Task<IHttpActionResult> ObtenerRoles(string roleFiltro)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerRoles(UsuarioLogadoDto.IdUsuario, roleFiltro));
            return Ok(result);
        }

        /// <summary>
        /// Obtener opciones Backbone
        /// </summary>
        /// <returns>Una lista de opciones del Backbone</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerOpciones")]
        public async Task<IHttpActionResult> ObtenerOpciones()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerOpciones(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        /// <summary>
        /// Obtener la lista de opciones para un rol de perfil
        /// </summary>
        /// <param name="idRol">Guid del rol</param>
        /// <param name="usuarioDnp">Guid del Usuario DNP</param>
        /// <returns>Una lista de opciones</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerOpcionesDeRol")]
        [MenuAuthorize("Usuarios")]
        [OpcionAuthorize("Roles:Crear|Roles:Editar")]
        public async Task<IHttpActionResult> ObtenerOpcionesDeRol(Guid idRol, string usuarioDnp)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerOpcionesDeRol(idRol, usuarioDnp));
            return Ok(result);
        }

        /// <summary>
        /// Obtener la lista de proyectos para un perfil de usuario
        /// </summary>
        /// <param name="idUsuarioPerfil">Guid del perfil del usuario</param>
        /// <param name="usuarioDnp">Guid del Usuario DNP</param>
        /// <returns>Una lista de proyectos</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerProyectosDePerfil")]
        public async Task<IHttpActionResult> ObtenerProyectosDePerfil(Guid idUsuarioPerfil, string usuarioDnp)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerProyectosPorPerfil(idUsuarioPerfil, usuarioDnp));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Asocia un proyecto ao Perfil del Usuario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>RespuestaGeneralDto</returns>
        [HttpPost]
        [Route("api/Usuario/AsociarProyectosAUsuarioPerfil")]
        public async Task<IHttpActionResult> AsociarProyectosAUsuarioPerfil([FromBody] UsuarioPerfilDto dto)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.AsociarProyectosAUsuarioPerfil(dto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Consulta los permisos del modulo Backbone de un usuario, agrupadas por entidad
        /// </summary>
        /// <returns>Permisos del usuario</returns>        
        [Route("api/Usuario/ObtenerPermisosPorEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPermisosPorEntidad()
        {
            var idUsuarioDnp = UsuarioLogadoDto.IdUsuario;

            var autorizacionPermisos = await Task.Run(() =>
                _autorizacionServicios.ObtenerPermisosPorEntidad(idUsuarioDnp));

            return Ok(autorizacionPermisos);
        }

        /// <summary>
        /// Obtener roles especificando el IdUsuarioDnp de un usuario
        /// </summary>
        /// <param name="usuario">Objeto usuario con propiedad IdUsuarioDnp diligenciada</param>
        /// <returns>Lista de roles del usuario</returns>    
        [Route("api/Usuario/ObtenerRolesPorUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerRolesPorUsuario(UsuarioDto dto)
        {
            dto.IdUsuarioDnp = UsuarioLogadoDto.IdUsuario;

            var autorizacionPermisos = await Task.Run(() =>
                _autorizacionServicios.ObtenerRolesPorUsuario(dto));

            return Ok(autorizacionPermisos);
        }

        /// <summary>
        /// Obtiene un objeto de entidad completo a partir de un id de rol
        /// </summary>
        /// <param name="idsRoles">Id del rol a consultar</param>
        /// <returns>Objeto de entidad</returns>
        [Route("api/Usuario/ObtenerEntidadesPorRoles")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEntidadesPorRoles([FromUri] List<Guid> idsRoles)
        {
            var idUsuarioDnp = UsuarioLogadoDto.IdUsuario;

            var autorizacionPermisos = await Task.Run(() =>
                _autorizacionServicios.ObtenerEntidadesPorListaRoles(idsRoles, idUsuarioDnp));

            return Ok(autorizacionPermisos);
        }


        private async Task<object> ObtieneDatosUsuarioAsync(string userObjectId)
        {
            string urlApiIdentidad = ConfigurationManager.AppSettings["ApiIdentidad"];

            try
            {
                string resultado = string.Empty;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlApiIdentidad);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    HttpClientAsignarAutorizacionJWT(client, UsuarioLogadoDto.IdUsuario);

                    var response = client.GetAsync("api/Identidad/obtenerUsuarioPorId?id=" + userObjectId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resultado = await response.Content.ReadAsStringAsync();
                    }
                }

                return JsonConvert.DeserializeObject(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// Obtener usuarios por entidad
        /// </summary>
        /// <param name="tipoEntidad"></param>
        /// <param name="filtro"></param>
        /// <returns>Una Lista de Usario Entidad</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosPorEntidadPdf")]
        public async Task<IHttpActionResult> ObtenerUsuariosPorEntidadPdf(string tipoEntidad, string filtro)
        {
            try
            {
                // deserializar
                var tipoAnonimo = new { NombreUsuario = String.Empty, CuentaUsuario = String.Empty, Estado = (bool?)null };
                var tipoAnonimoFiltros = JsonConvert.DeserializeAnonymousType(filtro, tipoAnonimo);

                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosPorEntidadSp(tipoEntidad, filtro, tipoAnonimoFiltros.NombreUsuario, tipoAnonimoFiltros.CuentaUsuario, tipoAnonimoFiltros.Estado, UsuarioLogadoDto.IdUsuario, RequestContext.Principal));

                string responseBody = result.ToString();
                var respuestaJs = JsonConvert.DeserializeObject(responseBody);
                List<UsuarioReportesDto> listResult = new List<UsuarioReportesDto>();
                var UsuariosTipoAnonimo = new
                {
                    CabezaSector = String.Empty,
                    AgrupadorNombreEntidad = String.Empty,
                    NombreEntidad = String.Empty,
                    NombreUsuario = String.Empty,
                    TipoIdentificacion = String.Empty,
                    Identificacion = String.Empty,
                    Correo = String.Empty,
                    Perfil = String.Empty,
                    Activo = String.Empty,
                    ActivoUsuarioPerfil = String.Empty,
                };
                var objResult = JsonConvert.DeserializeObject<List<UsuarioReportesDto>>(respuestaJs.ToString());
                return Ok(objResult);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para generar Usuarios en excel
        /// </summary>
        /// <param name="tipoEntidad">Contiene informacion de tipo de entidad</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Usuario/ObtenerUsuariosExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerUsuariosExcel(string tipoEntidad, string filtro)
        {
            try
            {
                // deserializar
                var tipoAnonimo = new { NombreUsuario = String.Empty, CuentaUsuario = String.Empty, Estado = (bool?)null };
                var tipoAnonimoFiltros = JsonConvert.DeserializeAnonymousType(filtro, tipoAnonimo);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var _result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosPorEntidadSp(tipoEntidad, filtro, tipoAnonimoFiltros.NombreUsuario, tipoAnonimoFiltros.CuentaUsuario, tipoAnonimoFiltros.Estado, UsuarioLogadoDto.IdUsuario, RequestContext.Principal));
                string responseBody = _result.ToString();
                var respuestaJs = JsonConvert.DeserializeObject(responseBody);
                List<UsuarioReportesDto> listResult = new List<UsuarioReportesDto>();
                var UsuariosTipoAnonimo = new
                {
                    CabezaSector = String.Empty,
                    AgrupadorNombreEntidad = String.Empty,
                    NombreEntidad = String.Empty,
                    NombreUsuario = String.Empty,
                    TipoIdentificacion = String.Empty,
                    Identificacion = String.Empty,
                    Correo = String.Empty,
                    Perfil = String.Empty,
                    Activo = String.Empty,
                    ActivoUsuarioPerfil = String.Empty,
                };
                var objResult = JsonConvert.DeserializeObject<List<UsuarioReportesDto>>(respuestaJs.ToString());
                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcellUsuarios(objResult);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Usuario/cambiarClaveUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> cambiarClaveUsuario(CredencialUsuarioDto credencialUsuario)
        {
            try
            {
                var result = await Task.Run(() => _identidadServicios.CambiarClaveUsuario(credencialUsuario, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener usuarios por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Una Lista de Usario</returns>
        [HttpGet]
        [Route("api/Usuario/ObtenerUsuariosPorNombre")]
        [MenuAuthorize("Usuarios")]
        public async Task<IHttpActionResult> ObtenerUsuariosPorNombre([FromUri] string nombre)
        {
            try
            {
                var result = await _autorizacionServicios.ObtenerUsuariosPorNombre(nombre, UsuarioLogadoDto.IdUsuario);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        private void HttpClientAsignarAutorizacionJWT(HttpClient client, string usuarioDnp)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _jwtTokenServicio);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization-Type", "JWT");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Application-User", usuarioDnp);
        }

        [Route("api/Usuario/ObtenerUsuarioDominio")]
        [HttpGet]
        public async Task<object> ObtenerUsuarioDominio(string usuarioDnp)
        {
            try
            {
                var result = await Task.Run(() => _identidadServicios.ObtenerUsuarioDominio(usuarioDnp));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Rol
        /// </summary>
        /// <param name="rolDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Autorizacion/ObtenerUsuariosPorNombreIdentificacion")]
        public async Task<IHttpActionResult> ObtenerUsuariosPorNombreIdentificacion(ParametrosUsuariosConfiguracionDto filtro)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuariosPorNombreIdentificacion(filtro, UsuarioLogadoDto.IdUsuario));
                
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Rol
        /// </summary>
        /// <param name="rolDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Autorizacion/GuardarSectoresPorUsuarioEntidad")]
        public async Task<IHttpActionResult> GuardarSectoresPorUsuarioEntidad(List<RespuestaSectoresUsuarioConfiguracionDto> data)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.GuardarSectoresPorUsuarioEntidad(data, UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar Rol
        /// </summary>
        /// <param name="rolDto"></param>
        /// <returns>una respuesta general</returns>
        [HttpPost]
        [Route("api/Autorizacion/ObtenerSectoresPorUsuarioEntidad")]
        public async Task<IHttpActionResult> ObtenerSectoresPorUsuarioEntidad(ParametrosSectoresUsuarioConfiguracionDto filtro)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerSectoresPorUsuarioEntidad(filtro, UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Identidad/cambiarContrasenaSTS")]
        [HttpPost]
        public async Task<IHttpActionResult> CambiarContrasenaSTS(UsuarioSTSDto usuarioSTS)
        {
            var result = await Task.Run(() => _identidadServicios.CambiarContrasenaSTS(usuarioSTS, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/enviarCorreoInvitacionSTS")]
        [HttpPost]
        public async Task<IHttpActionResult> EnviarCorreoInvitacionSTS(NotificarInvitacionUsuarioSTSDto notificacion)
        {
            var result = await Task.Run(() => _identidadServicios.EnviarCorreoInvitacionSTS(notificacion, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/registrarUsuarioAPPSTS")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarUsuarioAPPSTS(UsuarioSTSDto usuarioSTS)
        {
            var result = await Task.Run(() => _identidadServicios.RegistrarUsuarioAPPSTS(usuarioSTS, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/registrarUsuarioSTS")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarUsuarioSTS(UsuarioSTSDto usuarioSTS)
        {
            var result = await Task.Run(() => _identidadServicios.RegistrarUsuarioSTS(usuarioSTS, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/ValidarContrasenaActualSTS")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarContrasenaActualSTS(UsuarioSTSDto usuarioSTS)
        {
            var result = await Task.Run(() => _identidadServicios.ValidarContrasenaActualSTS(usuarioSTS, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/verificarExistenciaUsuarioSTSAplicacion")]
        [HttpGet]
        public async Task<IHttpActionResult> VerificarExistenciaUsuarioSTSAplicacion(string pAplicacion, string pTD, string pNumeroDocumento)
        {
             var result = await Task.Run(() => _identidadServicios.apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(pAplicacion, pTD, pNumeroDocumento, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/obtenerAplicacionesExistenciaUsuarioSTS")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAplicacionesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento)
        {
          
            var result = await Task.Run(() => _identidadServicios.apiObtenerAplicacionesExistenciaUsuarioSTS(pAplicacion, pTD, pNumeroDocumento, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/obtenerAplicacionesConfiablesExistenciaUsuarioSTS")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAplicacionesConfiablesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento)
        {
            
            var result = await Task.Run(() => _identidadServicios.apiObtenerAplicacionesConfiablesExistenciaUsuarioSTS(pAplicacion, pTD, pNumeroDocumento, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Identidad/obtenerUsuarioPorId")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuarioPorId(string id)
        {
            var idUsuarioDnp = UsuarioLogadoDto.IdUsuario;
            var result = await Task.Run(() => _identidadServicios.ObtenerUsuarioPorId(id, idUsuarioDnp));
            return Ok(result);
        }

        [Route("api/Usuario/validarPermisoInactivarUsuario")]
        [HttpGet]
        public async Task<IHttpActionResult> validarPermisoInactivarUsuario(string usuarioDnp, string usuarioDnpEliminar)
        {
            var result = await Task.Run(() => _autorizacionServicios.validarPermisoInactivarUsuario(usuarioDnp, usuarioDnpEliminar));
            return Ok(result);
        }
    }
}