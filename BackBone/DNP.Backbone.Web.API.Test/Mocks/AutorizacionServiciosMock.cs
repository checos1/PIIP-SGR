namespace DNP.Backbone.Web.API.Test.Mocks
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using DNP.Autorizacion.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
    using DNP.Backbone.Dominio.Dto.CentroAyuda;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using UsuarioDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioDto;
    using UsuarioPerfilDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilDto;
    using UsuarioPerfilProyectoDto = DNP.Backbone.Dominio.Dto.Usuario.UsuarioPerfilProyectoDto;

    public class AutorizacionServiciosMock : IAutorizacionServicios
    {
        public Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                throw new BackboneResponseException(HttpStatusCode.BadRequest, string.Format(Resources.ParametroNoRecibido, "nombreUsuario"));

            if (string.IsNullOrEmpty(hashUsuario))
                throw new BackboneResponseException(HttpStatusCode.BadRequest, string.Format(Resources.ParametroNoRecibido, "hashUsuario"));

            if (string.IsNullOrEmpty(idAplicacion))
                throw new BackboneResponseException(HttpStatusCode.BadRequest, string.Format(Resources.ParametroNoRecibido, "idAplicacion"));

            if (string.IsNullOrEmpty(nombreServicio))
                throw new BackboneResponseException(HttpStatusCode.BadRequest, string.Format(Resources.ParametroNoRecibido, "nombreServicio"));

            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            });

        }
        public Task<List<TipoEntidadDto>> ObtenerConfiguracionesRolSector(string usuarioDnp)
        {
            return usuarioDnp.Equals("jdelgado")
                ? Task.FromResult(new List<TipoEntidadDto>() { new TipoEntidadDto() })
                : Task.FromResult<List<TipoEntidadDto>>(null);
        }
        public Task<List<RolNegocioDto>> ObtenerRolesPorEntidadTerritorial(string idUsuario, Guid idEntidadTerritorial)
        {
            if (idUsuario.Equals("jdelgado") && idEntidadTerritorial != Guid.Empty)
                return Task.FromResult(new List<RolNegocioDto>() { new RolNegocioDto() });

            return Task.FromResult<List<RolNegocioDto>>(null);
        }
        public Task<List<RolDto>> ObtenerRolesPorPerfil(Guid perfilId, string usuarioDnp)
        {
            if (usuarioDnp.Equals("jdelgado") && perfilId != Guid.Empty)
                return Task.FromResult(new List<RolDto>() { new RolDto() });

            return Task.FromResult<List<RolDto>>(null);
        }
        public Task<List<UsuarioPerfilProyectoDto>> ObtenerProyectosPorPerfil(Guid perfilId, string usuarioDnp)
        {
            if (usuarioDnp.Equals("jdelgado") && perfilId != Guid.Empty)
                return Task.FromResult(new List<UsuarioPerfilProyectoDto>() { new UsuarioPerfilProyectoDto() });

            return Task.FromResult<List<UsuarioPerfilProyectoDto>>(null);
        }
        public Task<List<CrTypeDto>> ObtenerCRType(string usuarioDnp)
        {
            return Task.FromResult<List<CrTypeDto>>(null);
        }
        public Task<List<FaseDto>> ObtenerFase(string usuarioDnp)
        {
            return Task.FromResult<List<FaseDto>>(null);
        }
        public Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId, string usuarioDnp)
        {
            return Task.FromResult<List<MatrizEntidadDestinoAccionDto>>(null);
        }

        public Task<List<SectorNegocioDto>> ObtenerSectoresPorEntidadTerritorial(string idUsuario, Guid idEntidadTerritorial)
        {
            if (idUsuario.Equals("jdelgado") && idEntidadTerritorial != Guid.Empty)
                return Task.FromResult(new List<SectorNegocioDto>() { new SectorNegocioDto() });

            return Task.FromResult<List<SectorNegocioDto>>(null);
        }
        public Task<List<EntidadNegocioDto>> ObtenerEntidadesPorSectorTerritorial(string idUsuario, Guid idEntidadTerritorial, Guid idSector)
        {
            if (idUsuario.Equals("jdelgado") && idEntidadTerritorial != Guid.Empty && idSector != Guid.Empty)
                return Task.FromResult(new List<EntidadNegocioDto>() { new EntidadNegocioDto() });

            return Task.FromResult<List<EntidadNegocioDto>>(null);
        }
        public Task<RespuestaGeneralDto> EliminarPerfil(PerfilDto peticion)
        {
            return peticion.UsuarioDnp != null && peticion.IdPerfil != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }
        public Task<List<RolDto>> ObtenerRoles(string usuarioDnp, string roleFiltro)
        {
            return string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult<List<RolDto>>(null)
                : Task.FromResult(new List<RolDto>());
        }
        public Task<RespuestaGeneralDto> GuardarPerfil(PerfilDto peticion)
        {
            return peticion.UsuarioDnp != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }
        public Task<RespuestaGeneralDto> GuardarConfiguracionRolSectorAsync(PeticionConfiguracionRolSectorDto peticion)
        {
            return peticion != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult<RespuestaGeneralDto>(null);
        }
        public Task<RespuestaGeneralDto> EditarConfiguracionRolSector(PeticionConfiguracionRolSectorDto peticion)
        {
            return peticion != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult<RespuestaGeneralDto>(null);
        }
        public Task<RespuestaGeneralDto> CambiarEstadoConfiguracionRolSector(PeticionCambioEstadoConfiguracionDto peticion)
        {
            return peticion != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult<RespuestaGeneralDto>(null);
        }
        public Task<List<EntidadAutorizacionDto>> ObtenerEntidadesPorListaRoles(List<Guid> datosConsultaListaIdsRoles, string idUsuario)
        {
            if (datosConsultaListaIdsRoles.Count > 0 && idUsuario.Equals("jdelgado"))
                return Task.FromResult(new List<EntidadAutorizacionDto>()
                                       {
                                           new EntidadAutorizacionDto()
                                           {
                                               IdEntidadMGA = 636,
                                               IdEntidad = Guid. NewGuid(),
                                               NombreEntidad = "Entidad 636",
                                               TipoEntidad = "Territorial",
                                               Roles = new RolAutorizacionDto()
                                               {
                                                    IdRol = Guid.NewGuid(),
                                                    NombreRol = "Rol 1"
                                               }
                                           }
                                       });

            return Task.FromResult<List<EntidadAutorizacionDto>>(null);
        }
        public Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuario(string usuarioDnp, string tipoEntidad)
        {
            if (usuarioDnp.Equals("jdelgado") && !string.IsNullOrEmpty(tipoEntidad))
                return Task.FromResult(new List<EntidadPerfilDto>() { });

            return Task.FromResult<List<EntidadPerfilDto>>(null);
        }
        public Task<List<PerfilDto>> ObtenerPerfiles(UsuarioLogadoDto dto, string perfilFiltro)
        {
            if (dto != null)
                return Task.FromResult(new List<PerfilDto>()
            {
                new PerfilDto()
                {
                    IdPerfil = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    NombrePerfil = "ADM 01",
                    RolesConcat = "Rol Servicios",
                }
            });
            return Task.FromResult<List<PerfilDto>>(null);

        }
        public Task<List<UsuarioDto>> ObtenerUsuarios(string tipoEntidad)
        {
            return Task.FromResult(new List<UsuarioDto>());
        }
        public Task<List<PerfilDto>> ObtenerPerfilesPorAplicacion(string usuarioDNP, string aplicacion)
        {
            return Task.FromResult(new List<PerfilDto>()
            {
                new PerfilDto()
                {
                    IdPerfil = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    NombrePerfil = "ADM 01",
                    RolesConcat = "Rol Servicios",
                }
            });
        }
        public Task<List<PerfilDto>> ObtenerPerfilesAutorizadosPorAplicacion(String idAplicacion, string idPerfil, string usuarioDnp)
        {
            return Task.FromResult(new List<PerfilDto>()
            {
                new PerfilDto()
                {
                    IdPerfil = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    NombrePerfil = "ADM 01",
                    RolesConcat = "Rol Servicios",
                }
            });
        }

        public Task<RespuestaGeneralDto> GuardarRol(RolDto rolDto)
        {
            return null != rolDto.UsuarioDnp
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }
        public Task<RespuestaGeneralDto> EliminarRol(RolDto rolDto, string usuarioDnp)
        {
            return rolDto.UsuarioDnp != null && rolDto.IdRol != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }
        public Task<List<EntidadNegocioDto>> ObtenerEntidadesInviteUsuario(string usuarioDnp, string tipoEntidad)
        {
            return Task.FromResult(new List<EntidadNegocioDto>()
            {
                new EntidadNegocioDto()
                {
                    Id = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Nombre = "ADM 01"
                }
            });
        }
        public Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro)
        {
            return Task.FromResult(new List<EntidadUsuarioDto>()
            {
                new EntidadUsuarioDto()
                {
                    IdEntidad = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Entidad = "test"
                }
            });
        }
        public Task<string> ObtenerUsuariosPorEntidadSp(string tipoEntidad, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<List<EntidadUsuarioDto>> ObtenerUsuariosXEntidad(string tipoEntidad, string usuarioDnp, string filtro, String nombreUsuario, String cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {                                                            
            List<EntidadUsuarioDto> lista = new List<EntidadUsuarioDto>();
            EntidadUsuarioDto data = new EntidadUsuarioDto();

            data.IdEntidad = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C");
            data.Entidad = "test";

            lista.Add(data);

            if (string.IsNullOrEmpty(tipoEntidad) || string.IsNullOrEmpty(usuarioDnp))
            {
                return null;
            } else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidad(string tipoEntidad, string usuarioDnp)
        {
            return Task.FromResult(new List<EntidadFiltroDto>()
            {
                new EntidadFiltroDto()
                {
                    IdEntidad = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Entidad = "test"
                }
            });
        }

        public Task<ResultUnidadResponsableDTO> ObtenerEntidadesPorUnidadesResponsables(string usuarioDnp)
        {
            return Task.FromResult(new ResultUnidadResponsableDTO());
        }
        public Task<List<EntidadFiltroDto>> ObtenerSubEntidadesPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            return Task.FromResult(new List<EntidadFiltroDto>()
            {
                new EntidadFiltroDto()
                {
                    IdEntidad = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Entidad = "test"
                }
            });
        }
        public Task<bool> EditarUsuario(UsuarioDto usuarioDto)
        {
            return Task.FromResult(true);
        }
        public Task<bool> SetActivoUsuarioPerfil(SetActivoUsuarioPerfilDto setActivoUsuarioPerfilDto)
        {
            return Task.FromResult(true);
        }
        public Task<bool> CrearUsuarioInvitado(InvitarUsuarioDto usuarioDto, string idUsuarioDnp)
        {
            return Task.FromResult(true);
        }
        public Task<bool> CrearUsuarioPIIP(UsuarioPIIPDto usuarioDto, string idUsuarioDnp)
        {
            return Task.FromResult(true);
        }

        public Task<RespuestaGeneralDto> EliminarUsuarioPerfil(Guid idUsuarioPerfil)
        {
            return idUsuarioPerfil != null
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult<RespuestaGeneralDto>(null);
        }

        public Task<RespuestaGeneralDto> GuardarEntidad(EntidadNegocioDto entidadDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }
        public Task<RespuestaGeneralDto> GuardarCargaDatos(CargaDatosDto cargaDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<RespuestaGeneralDto> GuardarInflexibilidad(InflexibilidadDto inflexibilidadDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<RespuestaGeneralDto> GuardarInflexibilidadPagos(List<InflexibilidadPagosDto> lista, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<RespuestaGeneralDto> EliminarEntidad(Guid idEntidad, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }
        public Task<RespuestaGeneralDto> EliminarInflexibilidad(int id, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<EntidadFiltroDto> ObtenerEntidadPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            return Task.FromResult(new EntidadFiltroDto());
        }

        public Task<List<SectorNegocioDto>> ObtenerSectoresNegocio(string usuarioDnp)
        {
            return Task.FromResult(new List<SectorNegocioDto>());
        }

        public Task<UsuarioDto> ObtenerUsuarioPorId(string idUsuarioDNP, string idUsuario)
        {
            return Task.FromResult(new UsuarioDto());
        }

        public Task<RespuestaGeneralDto> SetActivoUsuarioPerfilPorEntidad(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            return dto != null
               ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
               : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        Task<RespuestaGeneralDto> IAutorizacionServicios.SetActivoUsuarioPerfil(SetActivoUsuarioPerfilDto dto)
        {
            return dto != null
               ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
               : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        public Task<List<EntidadNegocioDto>> ObtenerDepartamentos(string usuaquiDnp)
        {
            return Task.FromResult(new List<EntidadNegocioDto>());
        }

        public Task<List<AdherenciaDto>> ObtenerAdherenciasPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<AdherenciaDto>() { new AdherenciaDto() });

            return Task.FromResult<List<AdherenciaDto>>(null);
        }

        public Task<RespuestaGeneralDto> GuardarAdherencia(AdherenciaDto entidadDto, string usuarioDnp)
        {
            return !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        public Task<RespuestaGeneralDto> EliminarAdherencia(int idAdherencia, string usuarioDnp)
        {
            return !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        public Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<bool> CrearUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<List<EntidadFiltroDto>> ObtenerEntidadesPorTipoEntidadYUsuario(string tipoEntidad, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PerfilDto>> ObtenerTodosPerfiles(string idUsuarioDnp)
        {
            return Task.FromResult<IEnumerable<PerfilDto>>(null);
        }

        public Task<IEnumerable<EntidadDto>> ObtenerTodasEntidades(string idUsuarioDnp)
        {
            return Task.FromResult<IEnumerable<EntidadDto>>(null);
        }

        public Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXUsuarioAutenticado(string usuarioDnp)
        {
            List<ListadoEntidadDto> lista = new List<ListadoEntidadDto>();
            ListadoEntidadDto data = new ListadoEntidadDto();

            data.Id = Guid.NewGuid();
            data.Nombre = "test";
            data.PadreId = new Guid();

            lista.Add(data);

            if (string.IsNullOrEmpty(usuarioDnp))
            {
                return null;
            }
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<ListadoEntidadDto>> ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(string tipoEntidad, string usuarioDnp)
        {
            List<ListadoEntidadDto> lista = new List<ListadoEntidadDto>();
            ListadoEntidadDto data = new ListadoEntidadDto();

            data.Id = Guid.NewGuid();
            data.Nombre = "test";
            data.PadreId = new Guid();

            lista.Add(data);

            if (string.IsNullOrEmpty(tipoEntidad) || string.IsNullOrEmpty(usuarioDnp))
            {
                return null;
            }
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidad(string idEntidad, string usuarioDnp)
        {
            List<ListadoPerfilDto> lista = new List<ListadoPerfilDto>();
            ListadoPerfilDto data = new ListadoPerfilDto();

            data.IdPerfil = Guid.NewGuid();
            data.NombrePerfil = "test";

            lista.Add(data);

            if (string.IsNullOrEmpty(idEntidad) || string.IsNullOrEmpty(usuarioDnp))
            {
                return null;
            }
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<ListadoPerfilDto>> ObtenerListadoPerfilesXEntidadYUsuario(string idEntidad, string usuarioDnp)
        {
            List<ListadoPerfilDto> lista = new List<ListadoPerfilDto>();
            ListadoPerfilDto data = new ListadoPerfilDto();

            data.IdPerfil = Guid.NewGuid();
            data.NombrePerfil = "test";

            lista.Add(data);

            if (string.IsNullOrEmpty(idEntidad) || string.IsNullOrEmpty(usuarioDnp))
            {
                return null;
            }
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<EntidadPerfilDto>> ObtenerPerfilesPorUsuarioXUsuarioAutenticado(string usuarioDnp, string tipoEntidad, string usuarioDnpAutenticado)
        {
            List<EntidadPerfilDto> lista = new List<EntidadPerfilDto>();
            EntidadPerfilDto data = new EntidadPerfilDto();

            data.IdEntidad = Guid.NewGuid();
            data.NombreCompleto = "test";
            data.AgrupadorEntidad = "Agricultura y desarrollo rural";
            data.TipoEntidad = "Nacional";

            lista.Add(data);

            if (string.IsNullOrEmpty(tipoEntidad) || string.IsNullOrEmpty(usuarioDnp) || string.IsNullOrEmpty(usuarioDnpAutenticado))
            {
                return null;
            }
            else
            {
                return Task.FromResult(lista);
            }
        }


        public Task<List<EntidadNegocioDto>> ObtenerEntidadesNegocio(string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<EntidadNegocioDto>() { new EntidadNegocioDto() });

            return Task.FromResult<List<EntidadNegocioDto>>(null);
        }

        public Task<List<UsuarioDto>> ObtenerUsuarios(string tipoEntidad, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<UsuarioDto>() { new UsuarioDto() });

            return Task.FromResult<List<UsuarioDto>>(null);
        }

        #region Delegados

        public Task<List<DelegadoDto>> ObtenerDelegadosPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<DelegadoDto>() { new DelegadoDto() });

            return Task.FromResult<List<DelegadoDto>>(null);
        }

        public Task<RespuestaGeneralDto> GuardarDelegado(DelegadoDto delegadoDto, string usuarioDnp)
        {
            return !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        public Task<RespuestaGeneralDto> EliminarDelegado(int idDelegado, string usuarioDnp)
        {
            return !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
                : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }

        public Task<EntidadNegocioDto> ObtenerEntidadPorId(EntidadFiltroDto dto, string idUsuarioDnp)
        {
            return Task.FromResult(new EntidadNegocioDto());
        }

        public Task<RespuestaGeneralDto> AsociarProyectosAUsuarioPerfil(UsuarioPerfilDto dto, string idUsuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        #endregion Delegados

        public Task<List<OpcionDto>> ObtenerOpcionesDeRol(Guid idRol, string usuarioDnp)
        {
            return Task.FromResult(new List<OpcionDto>()
            {
                new OpcionDto()
                {
                    IdOpcion = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Nombre = "ADM 01"
                }
            });
        }

        public Task<List<OpcionDto>> ObtenerOpciones(string usuarioDnp)
        {
            return Task.FromResult(new List<OpcionDto>()
            {
                new OpcionDto()
                {
                    IdOpcion = Guid.Parse("6321417B-E783-4BB6-9F5D-5429524EE52C"),
                    Nombre = "ADM 01"
                }
            });
        }

        public Task<List<InflexibilidadDto>> ObtenerInflexibilidadPorEntidadId(Guid idEntidad, string usuarioDnp)
        {
            return Task.FromResult(new List<InflexibilidadDto>());
        }
        public Task<List<InflexibilidadPagosDto>> ObtenerInflexibilidadPagos(int idInflexibilidad, string usuarioDnp)
        {
            return Task.FromResult(new List<InflexibilidadPagosDto>());
        }

        public Task<List<TipoCargaDatosDto>> ObtenerCargaDatosPorTipoYTipoEntidad(string tipoEntidad, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<TipoCargaDatosDto>() { new TipoCargaDatosDto() });

            return Task.FromResult<List<TipoCargaDatosDto>>(null);
        }
        public Task<List<AyudaTemaListaItemDto>> ObtenerListaTemas(AyudaTemaFiltroDto dto, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(usuarioDnp))
                return Task.FromResult(new List<AyudaTemaListaItemDto>() { new AyudaTemaListaItemDto() });

            return Task.FromResult<List<AyudaTemaListaItemDto>>(null);
        }

        public RespuestaGeneralDto GuardarDatosMongoDB(dynamic data, string idSql)
        {
            return new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" };
        }

        public dynamic ObtenerDatosMongoDb(string id)
        {
            return new { };
        }

        public Task<RespuestaGeneralDto> GuardarDatos(CargaDatosDto cargaDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }
        public Task<RespuestaGeneralDto> EliminarCargaDatos(int id, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }
        public Task<UsuarioDto> ObtenerUsuarioPorIdUsuarioDnp(string idUsuarioDNP)
        {
            return Task.FromResult(new UsuarioDto());
        }

        public Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro, IPrincipal principal)
        {
            if (!string.IsNullOrEmpty(tipoEntidad))
                return Task.FromResult(new List<EntidadUsuarioDto>() { new EntidadUsuarioDto() });

            return Task.FromResult<List<EntidadUsuarioDto>>(null);
        }

        public Task<PermisosEntidadDto> ObtenerPermisosPorEntidad(string idUsuarioDnp)
        {
            return Task.FromResult(new PermisosEntidadDto());
        }

        public Task<List<RolDto>> ObtenerRolesPorUsuario(UsuarioDto dto)
        {
            if (dto != null)
                return Task.FromResult(new List<RolDto>() { new RolDto() });

            return Task.FromResult<List<RolDto>>(null);
        }

        public Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro, string idUsuarioDnp, IPrincipal principal)
        {
            if (!string.IsNullOrEmpty(tipoEntidad))
                return Task.FromResult(new List<EntidadUsuarioDto>() { new EntidadUsuarioDto() });

            return Task.FromResult<List<EntidadUsuarioDto>>(null);
        }

        public Task<RespuestaGeneralDto> EliminarUsuarioPerfil(Guid idUsuarioPerfil, string idUsuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<List<EntidadUsuarioDto>> ObtenerUsuariosPorEntidad(string tipoEntidad, string filtro, string nombreUsuario, string cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {
            if (!string.IsNullOrEmpty(tipoEntidad))
                return Task.FromResult(new List<EntidadUsuarioDto>() { new EntidadUsuarioDto() });

            return Task.FromResult<List<EntidadUsuarioDto>>(null);
        }

        public Task<List<EntidadNegocioDto>> ObtenerEntidadesConRoleVisualizador(string usuarioDnp)
        {
            return Task.FromResult(new List<EntidadNegocioDto>());
        }


        public Task<List<EntidadNegocioDto>> ObtenerEntidadesInviteUsuario(string usuarioDnp, string tipoEntidad, IPrincipal principal)
        {
            if (!string.IsNullOrEmpty(tipoEntidad))
                return Task.FromResult(new List<EntidadNegocioDto>() { new EntidadNegocioDto() });

            return Task.FromResult<List<EntidadNegocioDto>>(null);
        }

        public Task<List<InflexibilidadDto>> ObtenerInflexibilidadPorEntidadId(Guid idEntidad, InflexibilidadFiltroDto filtro, string usuarioDnp)
        {
            if (idEntidad == Guid.Empty || idEntidad == null)
                return Task.FromResult(new List<InflexibilidadDto>() { new InflexibilidadDto() });

            return Task.FromResult<List<InflexibilidadDto>>(null);
        }

        public Task<List<RolDto>> ObtenerRolesPorIdUsuarioPerfil(Guid idUsuarioPerfil, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<RolDto>> ObtenerRolesPorIdsUsuarioPerfil(List<Guid> idsUsuarioPerfil, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioAuthDto>> ObtenerUsuariosPorNombre(string nombre, string usuarioLogado)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarIdArchivoInflexibilidadPagos(InflexibilidadPagosDto pago, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosPorSubDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto)
        {
            throw new NotImplementedException();
        }

        public Task<EntidadFiltroDto> ObtenerEntidadPorCatalogoOptionId(int entidadCatalogoOptionId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<RolDto>> ObtenerRolesPorOpcionDnp(Guid idOpcionDnp, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerUsuarioPIIPXCorreoDNPAsync(string usuarioDnp)
        {
            return Task.FromResult(usuarioDnp);
        }

        public Task<RespuestaGeneralDto> ActualizarUnidadResponsable(EntidadNegocioDto entidadDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" });
        }

        public Task<ResultSectorDTO> ObtenerSectoresParaEntidades(string usuarioDnp)
        {
            return Task.FromResult(new ResultSectorDTO());
        }

        public Task<List<RespuestaUsuariosConfiguracionDto>> ObtenerUsuariosPorNombreIdentificacion(ParametrosUsuariosConfiguracionDto filtro, string usuarioDnp)
        {
            return Task.FromResult(new List<RespuestaUsuariosConfiguracionDto>());
        }

        public Task<List<RespuestaSectoresUsuarioConfiguracionDto>> ObtenerSectoresPorUsuarioEntidad(ParametrosSectoresUsuarioConfiguracionDto filtro, string usuarioDnp)
        {
            return Task.FromResult(new List<RespuestaSectoresUsuarioConfiguracionDto>());
        }

        public Task<bool> GuardarSectoresPorUsuarioEntidad(List<RespuestaSectoresUsuarioConfiguracionDto> data, string usuarioDnp)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<OpcionDto>> ObtenerOpcionesConFiltro(string idAplicacion, string usuarioDnp)
        {
            return Task.FromResult((IEnumerable<OpcionDto>)new List<OpcionDto>());
        }

        public Task<List<UsuarioAnalistaConceptoDto>> ObtenerUsuariosRValidadorPoliticaTransversal(ProyectoParametrosDto peticionObtenerProyecto)
        {
            return Task.FromResult((List<UsuarioAnalistaConceptoDto>)new List<UsuarioAnalistaConceptoDto>());
        }

        public Task<Dominio.Dto.AutorizacionNegocio.UsuarioCuentaDto> ObtenerCuentaUsuario(string nomeCuenta, string usuarioDnp)
        {
            return Task.FromResult(new Dominio.Dto.AutorizacionNegocio.UsuarioCuentaDto());
        }

        public Task<Dominio.Dto.AutorizacionNegocio.UsuarioDto> ObtenerUsuarioPorCorreoDNP(string correo, string usuarioDnp)
        {
            return Task.FromResult(new Dominio.Dto.AutorizacionNegocio.UsuarioDto());
        }

        public Task<List<EntidadFiltroDto>> ObtenerListaEntidad(string usuarioDnp, string objetoNegocioId)
        {
            return Task.FromResult((List<EntidadFiltroDto>)new List<EntidadFiltroDto>());
        }

        public Task<string> validarPermisoInactivarUsuario(string usuarioDnp, string usuarioDnpEliminar)
        {
            return Task.FromResult("OK");
        }

        public Task<IEnumerable<EntidadFiltroConsolaDto>> ObtnerEntidadesPorSector(int sectorId, string tipoEntidad, string usuarioDnp)
        {
            IEnumerable<EntidadFiltroConsolaDto> entidaddes = new List<EntidadFiltroConsolaDto>();
            return Task.FromResult(entidaddes);
        }

        public Task<string> ObtenerUsuariosPorEntidadSp(string tipoEntidad, string filtro, string nombreUsuario, string cuentaUsuario, bool? estado, string idUsuarioDnp, IPrincipal principal)
        {
            return Task.FromResult("OK");

        }

        public Task<string> ObtenerEncabezadoListadoReportesPIIP(string usuarioDnp)
        {
            return Task.FromResult("OK");
        }

        public Task<string> ObtenerListadoReportesPIIP(string usuarioDnp, string idRoles)
        {
            return Task.FromResult("OK");
        }

        public Task<string> ObtenerFiltrosReportesPIIP(Guid idReporte, string usuarioDnp)
        {
            return Task.FromResult("OK");
        }

        public Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDnp, string listaEntidades)
        {
            return Task.FromResult("OK");
        }


        public Task<List<AutorizacionUsuarioEntidadRolDto>> ObtenerUsuariosBasicosPorRolEntidad(EntidadRolDto entidadRol, string usuarioDnp)
        {
            return Task.FromResult(new List<AutorizacionUsuarioEntidadRolDto>());
        }

        Task<List<ListadoPerfilDto>> IAutorizacionServicios.ObtenerListadoPerfilesXEntidadBanco(string idEntidad, string usuarioDnp, int resourceGroupId)
        {
            return Task.FromResult(new List<ListadoPerfilDto>()
            {
                new ListadoPerfilDto()
            });
        }

        Task<List<Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto>> IAutorizacionServicios.ObtenerListadoPerfilesXUsuarioTerritorio(string usuarioDnp)
        {
            return Task.FromResult(new List<Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto>()
            {
                new Dominio.Dto.AutorizacionNegocio.UsuarioPerfilDto()
            });
        }

        Task<bool> IAutorizacionServicios.CrearUsuarioTerritorioPIIP(UsuarioPIIPTerritorioDto usuarioDto, string idUsuarioDnp)
        {
            return Task.FromResult(true);
        }

        Task<RespuestaGeneralDto> IAutorizacionServicios.SetActivoUsuarioEntidad(SetActivoUsuarioPerfilPorEntidadDto dto)
        {
            return dto != null
               ? Task.FromResult(new RespuestaGeneralDto() { Exito = true, Mensaje = "Exito" })
               : Task.FromResult(new RespuestaGeneralDto() { Exito = false, Mensaje = "Error" });
        }
    }
}
