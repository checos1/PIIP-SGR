namespace DNP.Backbone.Test.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Backbone.Servicios.Interfaces;
    using Comunes.Dto;
    using Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Programacion;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using Dominio.Dto.AutorizacionNegocio;
    using Dominio.Dto.Inbox;
    using Newtonsoft.Json;

    public class ClienteHttpServiciosMock : IClienteHttpServicios
    {
        public Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, string parametros, object peticion, string usuarioDnp, bool readCustomHttpCodes = false)
        {
            var respuesta = string.Empty;
            switch (uriMetodo)
            {
                case "api/AutorizacionNegocio/ValidarUsuarioEsAdministradorGlobal/":

                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult("true");
                    return Task.FromResult("false");

                case "api/AutorizacionNegocio/ConsultarConfiguraciones/":

                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new List<RolNegocioEntidadDestinoDto>()
                                                                                                         {
                        new RolNegocioEntidadDestinoDto()
                        {
                            Id = Guid.NewGuid(),
                            Sector = new SectorNegocioDto(){ Id = Guid.NewGuid(), Nombre = "Sector 1"},
                            Activo = true,
                            RolNegocioEntidad = new RolNegocioEntidadDto()
                                                {
                                                    Id = Guid.NewGuid(),
                                                    Entidad = new EntidadNegocioDto()
                                                              {
                                                                  Id = Guid.NewGuid(),
                                                                  Nombre = "Entidad 1",
                                                                  TipoEntidad = "Territorial",
                                                                  Sector = new SectorNegocioDto()
                                                                           {
                                                                               Id = Guid.NewGuid(),
                                                                               Nombre = "Sector 1"
                                                                           }
                                                              }
                                                }
                            }
                        }));

                    if (usuarioDnp.Equals("usuarioDnp")) return Task.FromResult(JsonConvert.SerializeObject(new List<RolNegocioEntidadDestinoDto>()
                                                                                                          {
                                                                                                              new RolNegocioEntidadDestinoDto()
                                                                                                              {
                                                                                                                  Id = Guid.NewGuid(),
                                                                                                                  Sector = new SectorNegocioDto(){ Id = Guid.NewGuid(), Nombre = "Sector 2"},
                                                                                                                  Activo = true,
                                                                                                                  RolNegocioEntidad = new RolNegocioEntidadDto()
                                                                                                                                      {
                                                                                                                                          Id = Guid.NewGuid(),
                                                                                                                                          Entidad = new EntidadNegocioDto()
                                                                                                                                                    {
                                                                                                                                                        Id = Guid.NewGuid(),
                                                                                                                                                        Nombre = "Entidad 2",
                                                                                                                                                        TipoEntidad = "Territorial",
                                                                                                                                                        Sector = new SectorNegocioDto()
                                                                                                                                                                 {
                                                                                                                                                                     Id = Guid.NewGuid(),
                                                                                                                                                                     Nombre = "Sector 2"
                                                                                                                                                                 }
                                                                                                                                                    }
                                                                                                                                      }
                                                                                                              }
                                                                                                          }));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/AutorizacionNegocio/ConsultarEntidadesTerritorialesUsuarioLocal/":

                    if (usuarioDnp.Equals("usuarioDnp")) return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadNegocioDto>()
                                                                                                             {
                                                                                                                 new EntidadNegocioDto()
                                                                                                                 {
                                                                                                                     TipoEntidad = "Territorial",
                                                                                                                     Id = Guid.NewGuid(),
                                                                                                                     Nombre = "Entidad x",
                                                                                                                     Sector = new SectorNegocioDto()
                                                                                                                              {
                                                                                                                                  Id = Guid.NewGuid(),
                                                                                                                                  Nombre = "Sector x"
                                                                                                                              }
                                                                                                                 }
                                                                                                             }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/AutorizacionNegocio/ConsultarSectoresUsuarioLocal/":

                    if (usuarioDnp.Equals("usuarioDnp")) return Task.FromResult(JsonConvert.SerializeObject(new List<SectorNegocioDto>()
                                                                                                             {
                                                                                                                 new SectorNegocioDto()
                                                                                                                 {
                                                                                                                     Id = Guid.NewGuid(),
                                                                                                                     Nombre = "Sector 1"
                                                                                                                 }
                                                                                                             }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/AutorizacionNegocio/ConsultarRolesUsuarioLocal/":
                    if (usuarioDnp.Equals("usuarioDnp")) return Task.FromResult(JsonConvert.SerializeObject(new List<RolNegocioDto>()
                                                                                                             {
                                                                                                                 new RolNegocioDto()
                                                                                                                 {
                                                                                                                     Id = Guid.NewGuid(),
                                                                                                                     Nombre = "Rol 1"
                                                                                                                 }
                                                                                                             }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/AutorizacionNegocio/ConsultarEntidadesUsuarioLocal/":
                    if (usuarioDnp.Equals("usuarioDnp")) return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadNegocioDto>()
                                                                                                             {
                                                                                                                 new EntidadNegocioDto()
                                                                                                                 {
                                                                                                                     Id = Guid.NewGuid(),
                                                                                                                     Nombre = "Entidad 1",
                                                                                                                     Sector = new SectorNegocioDto()
                                                                                                                              {
                                                                                                                                  Id = Guid.NewGuid(),
                                                                                                                                  Nombre = "Sector 2"
                                                                                                                              }
                                                                                                                 }
                                                                                                             }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/AutorizacionNegocio/ObtenerRolesPorEntidadTerritorial/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new List<RolNegocioDto>()));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/AutorizacionNegocio/ObtenerSectoresPorEntidadTerritorial/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new List<SectorNegocioDto>()));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/AutorizacionNegocio/ObtenerEntidadesPorSectorTerritorial/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new List<SectorNegocioDto>()));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/AutorizacionNegocio/GuardarConfiguracionRolSector/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = true }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = false }));
                case "api/AutorizacionNegocio/EditarConfiguracionRolSector/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = true }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = false }));
                case "api/AutorizacionNegocio/CambiarEstadoConfiguracionRolSector/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = true }));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(new RespuestaGeneralDto() { Exito = false }));
                case "api/AutorizacionNegocio/ObtenerEntidadesPorRoles/":
                    if (usuarioDnp.Equals("jdelgado")) return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadAutorizacionDto>()));
                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/Autorizacion/ObtenerEntidadesPorRoles/":
                    if (parametros.Contains("bdccf593-0a83-41d9-876d-ceaceb77bf25") && usuarioDnp.Equals("jdelgado"))
                        return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadAutorizacionDto>()
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
                                               }));

                    if (parametros.Contains("bdccf593-0a83-41d9-876d-ceaceb771234") && usuarioDnp.Equals("jdelgado"))
                        return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadAutorizacionDto>()
                                                                           {
                                                                               new EntidadAutorizacionDto()
                                                                               {
                                                                                   IdEntidadMGA = 123,
                                                                                   IdEntidad = Guid. NewGuid(),
                                                                                   NombreEntidad = "Entidad 123",
                                                                                   TipoEntidad = "Territorial",
                                                                                   Roles = new RolAutorizacionDto()
                                                                                           {
                                                                                               IdRol = Guid.NewGuid(),
                                                                                               NombreRol = "Rol 1"
                                                                                           }
                                                                               }
                                                                           }));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/Flujos/ObjetosNegocioConAccionesActivas/":
                    var parametrosInbox = (ParametrosObjetosNegocioInboxDto)peticion;

                    if (usuarioDnp.Equals("jdelgado"))
                    {

                        if (
                            parametrosInbox.IdTipoObjetoNegocio.
                                            Equals(Guid.Parse("bc154cba-50a5-4209-81ce-7c0ff0aec2ce")) ||
                            parametrosInbox.IdTipoObjetoNegocio.
                                            Equals(Guid.Parse("9c5ef8c1-da05-48b9-ba29-00c9efd7a774")))
                            return Task.FromResult(JsonConvert.SerializeObject(new List<NegocioDto>()
                                                                               {
                                                                                   new
                                                                                   NegocioDto()
                                                                                   {
                                                                                       Criticidad = "Alta",
                                                                                       IdEntidad = 636,
                                                                                       IdInstancia = Guid.NewGuid(),
                                                                                       IdObjetoNegocio = Guid.NewGuid().ToString(),
                                                                                       NombreEntidad = "Entidad 636",
                                                                                       NombreObjetoNegocio = "Proyecto"
                                                                                   }
                                                                               }));
                    }

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/Flujo/ObtenerFlujoConAccionesPorIdInstancia/":

                    if (usuarioDnp.Equals("jdelgado") && parametros.Contains("13039628-3b7c-4f03-90d0-cc05af96ca65"))
                        return Task.FromResult(JsonConvert.SerializeObject(CrearFlujoSencillo()));

                    if (usuarioDnp.Equals("jdelgado") && parametros.Contains("18866b32-9c95-4be8-af02-a8f9f2a0e02f"))
                        return Task.FromResult(JsonConvert.SerializeObject(CrearFlujoAnidado()));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/Authorization/ObtenerPermiso":
                    if (usuarioDnp.Equals("jdelgado"))
                        return Task.FromResult(JsonConvert.SerializeObject(new AutorizacionPermisoDto() { Estados = new List<int>() { 1 }, Permiso = true }));

                    return Task.FromResult(JsonConvert.SerializeObject(new AutorizacionPermisoDto() { Estados = new List<int>() { 3 }, Permiso = false }));

                case "api/Flujo/ObtenerAccionPorInstancia/":
                    if (usuarioDnp.Equals("jdelgado") && parametros.Contains("18866b32-9c95-4be8-af02-a8f9f2a0e02f") && parametros.Contains("661f942a-04da-45cb-8325-b7c9c476ac45"))
                        return Task.FromResult(JsonConvert.SerializeObject(CrearAccionPorInstancia(Guid.Parse("18866b32-9c95-4be8-af02-a8f9f2a0e02f"), Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"))));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/Flujo/ObtenerFlujoConAccionesPorIdFlujo/":

                    if (usuarioDnp.Equals("jdelgado") && parametros.Contains("18866b32-9c95-4be8-af02-a8f9f2a0e02f"))
                        return Task.FromResult(JsonConvert.SerializeObject(CrearFlujoSencillo()));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/Flujo/ObtenerInstanciaAnidadaPorIdPadre/":
                    if (usuarioDnp.Equals("jdelgado") && parametros.Contains("18866b32-9c95-4be8-af02-a8f9f2a0e02f"))
                        return Task.FromResult(JsonConvert.SerializeObject(CrearInstancia()));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));

                case "api/CacheEntidadesNegocio/ObtenerEntidadesPorTipoEntidad/":

                    if (usuarioDnp.Equals("jdelgado"))
                        return Task.FromResult(JsonConvert.SerializeObject(new List<EntidadNegocioDto>()
                                               {
                                                   new EntidadNegocioDto()
                                                   {
                                                       TipoEntidad = "Territorial",
                                                       Id = Guid.NewGuid(),
                                                       Nombre = "Entidad 123",
                                                       Sector = new
                                                                SectorNegocioDto()
                                                                {
                                                                    Id = Guid.NewGuid(),
                                                                    Nombre = "Sector 1"
                                                                }
                                                   }
                                               }));

                    return Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/CacheEntidadesNegocio/ObtenerRoles/":

                    return usuarioDnp.Equals("jdelgado") ? Task.FromResult(JsonConvert.SerializeObject(new List<RolNegocioDto>()
                                                                           {
                                                                               new RolNegocioDto()
                                                                               {
                                                                                   Id = Guid.NewGuid(),
                                                                                   Nombre = "Sector 1"
                                                                               }
                                                                           })) : Task.FromResult<string>(JsonConvert.SerializeObject(null));
                case "api/CacheEntidadesNegocio/ObtenerSectores/":

                    return usuarioDnp.Equals("jdelgado") ? Task.FromResult(JsonConvert.SerializeObject(new List<SectorNegocioDto>()
                                                                           {
                                                                               new SectorNegocioDto()
                                                                               {
                                                                                   Id = Guid.NewGuid(),
                                                                                   Nombre = "Sector 1"
                                                                               }
                                                                           })) : Task.FromResult<string>(JsonConvert.SerializeObject(null));
            }
            return Task.FromResult(respuesta);
        }

        private AccionesPorInstanciaDto CrearAccionPorInstancia(Guid idInstancia, Guid idAccion)
        {
            if (idInstancia.Equals(Guid.Parse("18866b32-9c95-4be8-af02-a8f9f2a0e02f")) && idAccion.Equals(Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45")))
                return new AccionesPorInstanciaDto()
                {
                    Id = Guid.NewGuid(),
                    AccionId = Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"),
                    CreadoPor = "jdelgado",
                    ModificadoPor = "jdelgado",
                    EstadoAccionPorInstanciaId = 0,
                    InstanciaId = Guid.Parse("18866b32-9c95-4be8-af02-a8f9f2a0e02f"),
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

            return null;
        }

        private InstanciaDto CrearInstancia()
        {
            return null;
        }

        private static FlujoDto CrearFlujoSencillo()
        {
            return new FlujoDto()
            {
                Id = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                Nombre = "Flujo Sencillo PBI 2320",
                Estado = 1,
                Descripcion = "Flujo Sencillo PBI 2320",
                DireccionIp = "10.1.1.1",
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                IdAplicacion = Guid.Parse("15ac35e7-4a72-439a-87ce-70262e115b84"),
                IdNivel = Guid.Parse("efaa069d-918a-4b31-8d07-75cc1bee4366"),
                Instancia = new InstanciaDto()
                {
                    Id = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                    ListaIdsRoles = new List<Guid>()
                                                                                                               {
                                                                                                                   Guid.NewGuid(),
                                                                                                                   Guid.NewGuid()
                                                                                                               },
                    IdObjeto = Guid.NewGuid(),
                    Aplicacion = "aplicacion",
                    IdUsuario = "jdelgado",
                    CreadoPor = "jdelgado",
                    EntidadDestino = 636,
                    EstadoInstanciaId = 1,
                    FechaCreacion = DateTime.Today,
                    FechaModificacion = DateTime.Today,
                    FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                    ModificadoPor = "jdelgado",
                    ObjetoNegocioId = "1234567890",
                    PadreId = Guid.Empty,
                    RolId = Guid.NewGuid(),
                    TipoObjeto = "Proyecto",
                    TipoObjetoId = Guid.Parse("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                    ListaIdsEntidades = new List<int>() { 636 }
                },
                TipoObjeto = new TipoObjetoDto()
                {
                    Id = Guid.Parse("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                    Nombre = "Proyecto",
                    FechaCreacion = DateTime.Today,
                    FechaModificacion = DateTime.Today,
                    ModificadoPor = "jdelgado",
                    CreadoPor = "jdelgalgado",
                },
                Usuario = "jdelgado",
                Version = 1,
                JsonFlujo = null,
                Acciones = new List<AccionesFlujosDto>()
                                                                                          {
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("e94eb98f-f052-4401-b85d-8d097cc8507d"),
                                                                                                  Nombre = "Inicial",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Inicial,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = null,
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"),
                                                                                                  Nombre = "Transaccional 1",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Transaccional,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("e94eb98f-f052-4401-b85d-8d097cc8507d"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Parse("df1ec10c-2e5e-4e65-9083-6f50bd145ad9"),
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("19de79d5-24e4-46bd-b29e-3969c32a63e3"),
                                                                                                  Nombre = "Transaccional 1",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Notificacion,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Parse("c5ec5369-b5f1-4224-89e5-dd24857a3b13"),
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("c5e0e026-c1f2-4a42-b276-4e29550af0e2"),
                                                                                                  Nombre = "Transaccional 2",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Transaccional,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("19de79d5-24e4-46bd-b29e-3969c32a63e3"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Parse("df1ec10c-2e5e-4e65-9083-6f50bd145ad9"),
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("ba7fd55f-5743-4e52-aef9-9e4ce6296b7c"),
                                                                                                  Nombre = "Fin",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Final,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("c5e0e026-c1f2-4a42-b276-4e29550af0e2"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = null
                                                                                              }
                                                                                          }

            };
        }

        private FlujoDto CrearFlujoAnidado()
        {
            return new FlujoDto()
            {
                Id = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                Nombre = "Flujo Sencillo PBI 2320",
                Estado = 1,
                Descripcion = "Flujo Sencillo PBI 2320",
                DireccionIp = "10.1.1.1",
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                IdAplicacion = Guid.Parse("15ac35e7-4a72-439a-87ce-70262e115b84"),
                IdNivel = Guid.Parse("efaa069d-918a-4b31-8d07-75cc1bee4366"),
                Instancia = new InstanciaDto()
                {
                    Id = Guid.Parse("18866b32-9c95-4be8-af02-a8f9f2a0e02f"),
                    ListaIdsRoles = new List<Guid>()
                                                                                                               {
                                                                                                                   Guid.NewGuid(),
                                                                                                                   Guid.NewGuid()
                                                                                                               },
                    IdObjeto = Guid.NewGuid(),
                    Aplicacion = "aplicacion",
                    IdUsuario = "jdelgado",
                    CreadoPor = "jdelgado",
                    EntidadDestino = 636,
                    EstadoInstanciaId = 1,
                    FechaCreacion = DateTime.Today,
                    FechaModificacion = DateTime.Today,
                    FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                    ModificadoPor = "jdelgado",
                    ObjetoNegocioId = "1234567890",
                    PadreId = Guid.Empty,
                    RolId = Guid.NewGuid(),
                    TipoObjeto = "Proyecto",
                    TipoObjetoId = Guid.Parse("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                    ListaIdsEntidades = new List<int>() { 636 }
                },
                TipoObjeto = new TipoObjetoDto()
                {
                    Id = Guid.Parse("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                    Nombre = "Proyecto",
                    FechaCreacion = DateTime.Today,
                    FechaModificacion = DateTime.Today,
                    ModificadoPor = "jdelgado",
                    CreadoPor = "jdelgalgado",
                },
                Usuario = "jdelgado",
                Version = 1,
                JsonFlujo = null,
                Acciones = new List<AccionesFlujosDto>()
                                                                                          {
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("e94eb98f-f052-4401-b85d-8d097cc8507d"),
                                                                                                  Nombre = "Inicial",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Inicial,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = null,
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"),
                                                                                                  Nombre = "Transaccional 1",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Transaccional,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("e94eb98f-f052-4401-b85d-8d097cc8507d"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Parse("df1ec10c-2e5e-4e65-9083-6f50bd145ad9"),
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("19de79d5-24e4-46bd-b29e-3969c32a63e3"),
                                                                                                  Nombre = "Transaccional 1",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Notificacion,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("661f942a-04da-45cb-8325-b7c9c476ac45"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Parse("c5ec5369-b5f1-4224-89e5-dd24857a3b13"),
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("c5e0e026-c1f2-4a42-b276-4e29550af0e2"),
                                                                                                  Nombre = "Transaccional 2",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Transaccional,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("19de79d5-24e4-46bd-b29e-3969c32a63e3"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Parse("df1ec10c-2e5e-4e65-9083-6f50bd145ad9"),
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = Guid.Parse("8792db04-85c9-4f8a-93f9-214d22597650")
                                                                                              },
                                                                                              new AccionesFlujosDto()
                                                                                              {
                                                                                                  Id = Guid.Parse("ba7fd55f-5743-4e52-aef9-9e4ce6296b7c"),
                                                                                                  Nombre = "Fin",
                                                                                                  FechaCreacion = DateTime.Today,
                                                                                                  FechaModificacion = DateTime.Now,
                                                                                                  IdInstancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65"),
                                                                                                  CreadoPor = "jdelgado",
                                                                                                  ModificadoPor = "jdelgado",
                                                                                                  TipoAccion = TipoAccion.Final,
                                                                                                  PeriodoValidez = 0,
                                                                                                  IdAccionPadre = Guid.Parse("c5e0e026-c1f2-4a42-b276-4e29550af0e2"),
                                                                                                  FlujoId = Guid.Parse("662a97f9-57b0-40d8-8950-0e8019e0f09f"),
                                                                                                  CantidadAcciones = 0,
                                                                                                  TipoParalela = false,
                                                                                                  FechaLimite = null,
                                                                                                  IdFormulario = Guid.Empty,
                                                                                                  IdNotificacion = Guid.Empty,
                                                                                                  IdFlujoAnidado = Guid.Empty,
                                                                                                  EstadoAccionId = null
                                                                                              }
                                                                                          }

            };
        }

        public Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, string parametros, object peticion, string usuarioDnp, bool readCustomHttpCodes = false, IPrincipal principal = null)
        {
            switch (uriMetodo)
            {
                case "api/Flujos/ObtenerFlujoTramiteNivel":
                    return Task.FromResult(JsonConvert.SerializeObject(new List<FlujosProgramacionDto>()
                                               {
                                                   new FlujosProgramacionDto()
                                                   {
                                                       Descripcion ="Descripción",
                                                       Id = Guid.NewGuid(),
                                                       Nombre = "Descripción"
                                                   }
                                               }));
                default:
                    return Task.FromResult(string.Empty);
            }
        }

        public Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, string parametros, object peticion, string usuarioDnp, bool readCustomHttpCodes = false, IPrincipal principal = null, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            switch (uriMetodo)
            {
                case "SGR/Viabilidad/LeerInformacionGeneral":
                    return Task.FromResult(JsonConvert.SerializeObject(new LeerInformacionGeneralViabilidadDto
                    {
                        Id = 582685,
                        Nombre = "Proyecto de prueba",
                        CodigoBPIN = "",
                        Categorias = "",
                        Fase = "Factibilidad",
                        RegionSgrId = 1
                    }));
                case "SGR/Viabilidad/LeerParametricas":
                    return Task.FromResult(JsonConvert.SerializeObject(JsonConvert.SerializeObject(new List<LeerParametricasViabilidadDto>
                    {
                        new LeerParametricasViabilidadDto
                        {
                            RegionesSgr = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Caribe"
                                }
                            },
                            CategoriasSgr = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Servicios"
                                }
                            },
                            Sectores = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Planeación"
                                }
                            }
                        }
                    })));
                case "SGR/Viabilidad/GuardarInformacionBasica":
                    return Task.FromResult(JsonConvert.SerializeObject(new ResultadoProcedimientoDto
                    {
                        Exito = true
                    }));
                default:
                    return Task.FromResult(string.Empty);
            }
        }

        public Task<HttpResponseMessage> PostRequestApiMultiContent(string url, MultipartFormDataContent body, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> RequestApiByteArray(MetodosServiciosWeb metodoServicio, string url, string parametros, object peticion, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            throw new NotImplementedException();
        }

        Task<string> IClienteHttpServicios.ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, object peticion, string usuarioDNP, bool readCustomHttpCodes, IPrincipal principal, bool useJWTAuth, bool useBearerToken, string tokenBearerJWT)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }
    }
}
