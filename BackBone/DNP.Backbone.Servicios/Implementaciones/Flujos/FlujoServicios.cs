using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
//using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Flujos;
using DNP.Backbone.Dominio.Dto.Monitoreo;
using DNP.Backbone.Dominio.Dto.Programacion;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Flujos.Dominio.Dto.Flujos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Flujos
{
    public class FlujoServicios : IFlujoServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private IServiciosNegocioServicios _serviciosNegocioServicios;

        public FlujoServicios(IClienteHttpServicios clienteHttpServicios, IAutorizacionServicios autorizacionServicios, IServiciosNegocioServicios serviciosNegocioServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
            _autorizacionServicios = autorizacionServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        public async Task<FlujoMenuContextualDto> ObtenerFlujoPorInstanciaTarea(string usuarioDnp, Guid idInstancia)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerFlujoConAccionesPorIdInstancia"];
            var parametros = $"?idInstancia={idInstancia}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros,
                    null, usuarioDnp, useJWTAuth: false);

            var flujo = JsonConvert.DeserializeObject<FlujoDto>(respuesta);

            return flujo != null ? CrearFlujoMenu(flujo, string.Empty, usuarioDnp) : null;
        }

        public async Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivas(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObjetosNegocioAccionesActivas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasTotales(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObjetosNegocioAccionesTotales"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<List<ResultadoValidarProyectoItemDto>> ValidarProyectosConInstanciasActivas(ValidarProyectosDto dto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriValidarProyectosConInstanciasActivas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, dto, dto.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ResultadoValidarProyectoItemDto>>(respuesta);
        }

        public async Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriTramites"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteDto>>(respuesta);
        }

        public async Task<List<TramiteDto>> ObtenerTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriTramitesProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteDto>>(respuesta);
        }

        public async Task<List<NegocioDto>> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriProyectosTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriAlertasConfig"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaAlertasConfigDto, instanciaAlertasConfigDto.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<AlertasConfigDto>>(respuesta);
        }

        public async Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriMapColumnas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, mapColumnasFiltroDto, mapColumnasFiltroDto.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<MapColumnasDto>>(respuesta);
        }

        public async Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriAlertasConfigCrearActualizar"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, alertasConfigFiltroDto, alertasConfigFiltroDto.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<AlertasConfigDto>(respuesta);
        }

        public async Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriAlertasConfigEliminar"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, alertasConfigFiltroDto, alertasConfigFiltroDto.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<AlertasConfigDto>(respuesta);
        }

        public async Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto alertasGeneradasFiltro)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriAlertasGeneradas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, alertasGeneradasFiltro, alertasGeneradasFiltro.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ICollection<AlertasGeneradasDto>>(respuesta);
        }

        public async Task<List<InfoFinancieroProyectoDto>> ObtenerInfoFinancieroProyectos(InfoFinancieroProyectoFiltroDto infoFinancieroProyectoFiltro)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerInfoFinancieroProyecto"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, infoFinancieroProyectoFiltro, infoFinancieroProyectoFiltro.ParametrosDto?.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<InfoFinancieroProyectoDto>>(respuesta);
        }

        public async Task<List<TramiteDto>> ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriConsolaTramites"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteDto>>(respuesta);
        }

        public async Task<List<NegocioDto>> ObtenerProyectosTramiteConsola(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriProyectosTramiteConsola"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<InstanciaDto> ObtenerInstanciaPorId(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerInstanciaPorId"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<InstanciaDto>(respuesta);
        }

        #region MÉTODOS PRIVADOS

        private FlujoMenuContextualDto CrearFlujoMenu(FlujoDto flujo, string ordenVisualizacionPadre, string usuarioDnp)
        {
            var flujoMenu = new FlujoMenuContextualDto
            {
                Id = flujo.Id,
                Nombre = flujo.Nombre,
                Descripcion = flujo.Descripcion,
                FechaHoraCreacion = flujo?.Instancia?.FechaCreacion,
                Usuario = flujo?.Instancia?.IdUsuario,
                RolId = flujo?.Instancia?.RolId,
                IdEntidad = flujo?.Instancia?.EntidadDestino,
                Entidad = ObtenerEntidad(flujo.Instancia.EntidadDestino.Value, usuarioDnp),
                NumeroTramite = flujo?.NumeroTramite,
                Acciones = AsignarOrdenVisualizacion(flujo, ordenVisualizacionPadre, usuarioDnp)
                
            };
            if (flujo.Instancia != null)
                flujoMenu.IdInstancia = flujo.Instancia.Id;

            return flujoMenu;
        }

        private string ObtenerEntidad(int entidadDestino, string usuarioDnp)
        {
            string entidad = string.Empty;
            if (entidadDestino > 0)
            {
                var result = _autorizacionServicios.ObtenerEntidadPorCatalogoOptionId(entidadDestino, usuarioDnp);
                entidad = result.Result.Entidad;
            }
            return entidad;
        }

        private AccionesFlujosMenuContextualDto CrearAccionMenu(AccionesFlujosDto accion)
        {
            var accionMenu = new AccionesFlujosMenuContextualDto()
            {
                Id = accion.Id,
                Nombre = accion.Nombre,
                TipoAccion = accion.TipoAccion,
                Descripcion = accion.Descripcion,
                IdFormulario = accion.IdFormulario,
                RolId = accion.RolId,
                Ventana = accion.Ventana,
                IdNivel = accion.IdNivel,
                DescripcionNivel = accion.DescripcionNivel,
                Usuarios = accion.Usuarios,
                RequiereInfoNivelAnterior = accion.RequiereInfoNivelAnterior,
                VisualizarCumple = accion.VisualizarCumple,
                EstadoAccionPorInstanciaId = accion.EstadoAccionPorInstanciaId
            };

            var permisoEntidad = accion.Usuarios.FirstOrDefault(x => x.IdEntidad > 0);
            if(permisoEntidad != null)
            {
                accionMenu.IdEntidad = permisoEntidad.IdEntidad;
                accionMenu.Entidad = ObtenerEntidad(permisoEntidad.IdEntidad, permisoEntidad.Usuario);
            }

            if (accion.AccionPorInstancia == null)
            {
                accionMenu.Estado = EstadoAccionMenuContextual.PorDefinir.ToString();
            }
            else
            {
                accionMenu.AccionInstanciaId = accion.AccionPorInstancia.Id;
                accionMenu.Estado = ObtenerEstado(accion.AccionPorInstancia.EstadoAccionPorInstanciaId);
                accionMenu.FechaCreacion = accion.AccionPorInstancia.FechaCreacion;
                accionMenu.FechaModificacion = accion.AccionPorInstancia.FechaModificacion;
                accionMenu.ModificadoPor = accion.AccionPorInstancia.ModificadoPor;
                accionMenu.CreadoPor = accion.AccionPorInstancia.CreadoPor;
                Dominio.Dto.Usuario.UsuarioDto usuario = new Dominio.Dto.Usuario.UsuarioDto();
                if (!string.IsNullOrEmpty(accion.AccionPorInstancia.ModificadoPor))
                {
                    usuario = _autorizacionServicios.ObtenerUsuarioPorIdUsuarioDnp(accion.AccionPorInstancia.ModificadoPor).Result;
                }
                else
                {
                    usuario = _autorizacionServicios.ObtenerUsuarioPorIdUsuarioDnp(accion.AccionPorInstancia.CreadoPor).Result;
                }
                if (usuario != null)
                {
                    accionMenu.NombreUsuario = usuario.Nombre;
                    accionMenu.Cedula = usuario.Identificacion;
                }
            }

            return accionMenu;
        }


        private List<AccionesFlujosMenuContextualDto> AsignarOrdenVisualizacion(FlujoDto flujo, string ordenPadre, string usuarioDnp)
        {
            var accionesFlujo = flujo.Acciones;
            var listaAccionesMenu = new List<AccionesFlujosMenuContextualDto>();
            var ordenVisualizacion = 0;
            var esAccionFinal = false;

            var accion = accionesFlujo.Find(x => x.TipoAccion == TipoAccion.Inicial);

            while (!esAccionFinal)
            {
                // ReSharper disable once PossibleNullReferenceException
                if (accion.TipoAccion != TipoAccion.Enrutamiento)
                    accion = accionesFlujo.Find(x => x.IdAccionPadre == accion.Id && x.TipoAccion != TipoAccion.RamasParalelas);

                if ((accion == null) || (accion.TipoAccion == TipoAccion.Enrutamiento))
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (accion != null)
                    {
                        //Agregando enrutamiento a las acciones
                        var accionMenuEnruta = CrearAccionMenu(accion);
                        ordenVisualizacion++;
                        accionMenuEnruta.OrdenVisualizacion = ConstruirOrden(ordenPadre, ordenVisualizacion);
                        accionMenuEnruta.Roles = ObtenerRolesPorAccionAsync(accion.Id, usuarioDnp).Result;
                        listaAccionesMenu.Add(accionMenuEnruta);
                        //
                        accion = ObtenerAccionEnrrutada(flujo, accionesFlujo, accion, usuarioDnp);
                    }
                }

                if ((accion == null) || (accion.TipoAccion == TipoAccion.Final))
                {
                    esAccionFinal = true;
                    continue;
                }

                if (flujo.Instancia != null)
                    accion.AccionPorInstancia = ConsultarAccionPorInstancia(flujo.Instancia.Id, accion.Id, usuarioDnp);

                if (((accion.IdFormulario == Guid.Empty && accion.Ventana == null) || (accion.TipoAccion != TipoAccion.Transaccional)) &&
                    (accion.TipoAccion != TipoAccion.Anidada) && (accion.TipoAccion != TipoAccion.Paralelas)) continue;

                var accionMenu = CrearAccionMenu(accion);
                ordenVisualizacion++;
                accionMenu.VisualizaEnviarSubpaso = accion.VisualizaEnviarSubpaso;
                accionMenu.SubPasos = accion.SubPasos;
                accionMenu.Roles = ObtenerRolesPorAccionAsync(accion.Id, usuarioDnp).Result;
                accionMenu.OrdenVisualizacion = ConstruirOrden(ordenPadre, ordenVisualizacion);

                switch (accion.TipoAccion)
                {
                    case TipoAccion.Anidada:
                        if ((accion.FlujoAnidado != null) || (accion.IdFlujoAnidado != Guid.Empty))
                        {
                            var flujoAnidado = ConsultarFlujo(accion.IdFlujoAnidado, usuarioDnp);
                            accion.FlujoAnidado = flujoAnidado;

                            if (accion.AccionPorInstancia != null)
                            {
                                accion.FlujoAnidado.Instancia = ConsultarInstanciaAnidada(
                                    accion.AccionPorInstancia.Id, accion.FlujoAnidado.Id, usuarioDnp);
                            }
                            accionMenu.FlujoAnidado = CrearFlujoMenu(accion.FlujoAnidado,
                                accionMenu.OrdenVisualizacion,
                                usuarioDnp);
                        }

                        break;
                    case TipoAccion.Paralelas:
                        if (flujo.Instancia != null)
                        {
                            var listaAccionesParalelas = CrearAccionesParalelasMenu(flujo.Acciones, accion,
                                flujo.Instancia.Id, accionMenu.OrdenVisualizacion, usuarioDnp);

                            if (listaAccionesParalelas.Count > 0)
                                accionMenu.AccionesParalelas = listaAccionesParalelas;

                        }

                        break;

                }
                listaAccionesMenu.Add(accionMenu);
            }

            return listaAccionesMenu;
        }

        private async Task<List<RolAutorizacionDto>> ObtenerRolesPorAccionAsync(Guid id, string usuarioDnp)
        {
            List<RolAutorizacionDto> roles = new List<RolAutorizacionDto>();
            var rolesPorOpcionDnp = await _autorizacionServicios.ObtenerRolesPorOpcionDnp(id, usuarioDnp);
            foreach (var item in rolesPorOpcionDnp)
            {
                roles.Add(new RolAutorizacionDto() { IdRol = item.IdRol, NombreRol = item.Nombre });
            }
            return roles;
        }

        private AccionesFlujosDto ObtenerAccionEnrrutada(FlujoDto flujo, IEnumerable<AccionesFlujosDto> accionesFlujo, AccionesFlujosDto accion, string usuarioDnp)
        {
            var listaAcciones = accionesFlujo.Where(af => af.IdAccionPadre == accion.Id && af.TipoAccion != TipoAccion.RamasParalelas).ToList();

            if (flujo.Instancia == null) return null;

            accion.AccionPorInstancia = ConsultarAccionPorInstancia(flujo.Instancia.Id, accion.Id, usuarioDnp);
            if (accion.AccionPorInstancia?.Enrutamiento != null)
            {
                var accionEnrutada = listaAcciones.Find(a => a.EnrutamientoId == accion.AccionPorInstancia.Enrutamiento);
                return accionEnrutada;
            }
            else
            {
                return null;
            }
        }

        private static string ConstruirOrden(string ordenPadre, int ordenVisualizacion)
        {
            var nuevoOrdenPadre = string.Empty;
            if (ordenPadre != string.Empty)
                nuevoOrdenPadre = $".{ordenPadre}";

            nuevoOrdenPadre = $"{ordenVisualizacion}{nuevoOrdenPadre}";

            return nuevoOrdenPadre;
        }

        private List<AccionesParalelasFlujoDto> CrearAccionesParalelasMenu(List<AccionesFlujosDto> accionesFlujo,
                       AccionesFlujosDto accion, Guid idInstancia, string ordenVisualizacionPadre, string usuarioDnp)
        {
            var accionesParalelasIniciales = accionesFlujo.FindAll(x => x.IdScope == accion.Id && x.IdAccionPadre == accion.Id).OrderBy(par => par.IndiceCreacion);
            var ordenVisualizacion = 0;
            var listaAccionesParalelas = new List<AccionesParalelasFlujoDto>();


            foreach (var accionParalela in accionesParalelasIniciales)
            {
                var esAccionFinalParalela = false;
                var accionesRamaMenu = new List<AccionesFlujosMenuContextualDto>();
                var siguienteAccion = accionesFlujo.FirstOrDefault(x => x.IdAccionPadre == accionParalela.Id);
                var rama = new AccionesParalelasFlujoDto();

                do
                {
                    if ((siguienteAccion != null) && ((siguienteAccion.IdFormulario != Guid.Empty) &&
                                                      (siguienteAccion.TipoAccion == TipoAccion.Transaccional) ||
                                                      (siguienteAccion.TipoAccion == TipoAccion.Anidada)))
                    {

                        siguienteAccion.AccionPorInstancia =
                            ConsultarAccionPorInstancia(idInstancia, siguienteAccion.Id, usuarioDnp);
                        var accionMenu = CrearAccionMenu(siguienteAccion);
                        ordenVisualizacion++;
                        accionMenu.OrdenVisualizacion = ConstruirOrden(ordenVisualizacionPadre, ordenVisualizacion);

                        if (siguienteAccion.TipoAccion == TipoAccion.Anidada)
                        {
                            siguienteAccion.FlujoAnidado = ConsultarFlujo(siguienteAccion.IdFlujoAnidado, usuarioDnp);

                            if (siguienteAccion.AccionPorInstancia != null)
                            {
                                siguienteAccion.FlujoAnidado.Instancia = ConsultarInstanciaAnidada(
                                   siguienteAccion.AccionPorInstancia.Id, siguienteAccion.FlujoAnidado.Id,
                                   usuarioDnp);
                            }

                            accionMenu.FlujoAnidado = CrearFlujoMenu(siguienteAccion.FlujoAnidado,
                                accionMenu.OrdenVisualizacion, usuarioDnp);
                        }

                        accionesRamaMenu.Add(accionMenu);
                        siguienteAccion = accionesFlujo.FirstOrDefault(x => x.IdAccionPadre == siguienteAccion.Id);
                    }
                    else if (siguienteAccion.TipoAccion == TipoAccion.Enrutamiento)
                    {
                        siguienteAccion.AccionPorInstancia = ConsultarAccionPorInstancia(
                            idInstancia, siguienteAccion.Id, usuarioDnp);

                        if (siguienteAccion.AccionPorInstancia != null)
                        {
                            if (siguienteAccion.AccionPorInstancia.Enrutamiento != null &&
                           siguienteAccion.AccionPorInstancia.EstadoAccionPorInstanciaId == (int)EstadoAccionPorInstancia.Ejecutada)
                            {
                                siguienteAccion = accionesFlujo.FirstOrDefault(
                                    af => af.IdAccionPadre == siguienteAccion.AccionPorInstancia.AccionId &&
                                    af.EnrutamientoId == siguienteAccion.AccionPorInstancia.Enrutamiento);
                            }
                            else
                                siguienteAccion = null;
                        }
                        else
                            siguienteAccion = null;
                    }
                    else
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        siguienteAccion = accionesFlujo.FirstOrDefault(x => x.IdAccionPadre == siguienteAccion.Id);
                    }

                    if (siguienteAccion == null)
                        esAccionFinalParalela = true;
                }
                while (!esAccionFinalParalela);

                if (accionesRamaMenu.Count > 0)
                {
                    rama = new AccionesParalelasFlujoDto()
                    {
                        Id = accionParalela.Id,
                        Acciones = accionesRamaMenu,
                        NumeroAcciones = accionesRamaMenu.Count,
                        Estado = ObtenerEstado(accionParalela.EstadoAccionPorInstanciaId),
                        EsObligatoria = accionParalela.EsObligatoria,
                        TipoAccion = accionParalela.TipoAccion,
                        Nombre = accionParalela.Nombre
                    };
                }

                if (rama.Acciones != null)
                {
                    if (rama.Acciones.Count > 0)
                        listaAccionesParalelas.Add(rama);
                }

            }
            return listaAccionesParalelas.Count > 0 ? listaAccionesParalelas : new List<AccionesParalelasFlujoDto>();


            // return listaAccionesParalelas;
        }
        private static string ObtenerEstado(int? accionParalelaEstadoAccionPorInstanciaId)
        {
            return accionParalelaEstadoAccionPorInstanciaId == (int)EstadoAccionPorInstancia.Ejecutada ||
                accionParalelaEstadoAccionPorInstanciaId == (int)EstadoAccionPorInstancia.Finalizada
                ? EstadoAccionMenuContextual.Ejecutada.ToString() : EstadoAccionMenuContextual.PasoEnProgreso.ToString();
        }

        public AccionesPorInstanciaDto ConsultarAccionPorInstancia(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerAccionPorInstancia"];
            var parametros = $"?idInstancia={idInstancia}&idAccion={idAccion}";

            var accionPorInstancia = JsonConvert.DeserializeObject<AccionesPorInstanciaDto>(_clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo,
                    parametros, null, usuarioDnp, useJWTAuth: false).Result);

            return accionPorInstancia;
        }

        private FlujoDto ConsultarFlujo(Guid idFlujo, string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerFlujoConAccionesPorIdFlujo"];
            var parametros = $"?idFlujo={idFlujo}";

            var flujo = JsonConvert.DeserializeObject<FlujoDto>(_clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo,
                    parametros, null, usuarioDnp, useJWTAuth: false).Result);

            return flujo;
        }

        private InstanciaDto ConsultarInstanciaAnidada(Guid? idInstancia, Guid idflujo, string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerInstanciaAnidadaPorIdPadre"];
            var parametros = $"?idInstancia={idInstancia}&idflujo={idflujo}";

            var instancia = JsonConvert.DeserializeObject<InstanciaDto>(_clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo,
                    parametros, null, usuarioDnp, useJWTAuth: false).Result);

            return instancia;
        }

        public Task<AlertasGeneradasDto> ObtenerAlertasGeneradas(AlertasGeneradasDto alertasGeneradasFiltro)
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionary<int, bool>> ObtenerSituacaoAlertasProyectos(InstanciaProyectoDto proyectoDto)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSituacaoAlertasProyectos"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, endPoint, uriMetodo, parametros: null, proyectoDto.ProyectoFiltroDto.ProyectosIds, proyectoDto.ProyectoParametrosDto?.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<IDictionary<int, bool>>(response);
        }
        #endregion

        public async Task<IEnumerable<FlujosProgramacionDto>> ObtenerListaFlujosTramitePorNivel(Guid idNivel, string idUsuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriFlujoTramitePorNivel"];
            var uriParametros = $"?idNivel={idNivel}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, idUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<IEnumerable<FlujosProgramacionDto>>(respuesta);
        }

        public async Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadas(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObjetosNegocioAccionesActivasYPausadas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<InstanciaDto> ActivarInstancia(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriActivarInstancia"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<InstanciaDto>(respuesta);
        }

        public async Task<InstanciaDto> PausarInstancia(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriPausarInstancia"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<InstanciaDto>(respuesta);
        }

        public async Task<InstanciaDto> DetenerInstancia(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriDetenerInstancia"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<InstanciaDto>(respuesta);
        }

        public async Task<InstanciaDto> CancelarInstanciaMisProcesos(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriCancelarInstanciaMisProcesos"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<InstanciaDto>(respuesta);
        }

       

        public async Task<List<Dominio.Dto.InstanciaResultado>> GenerarInstancias(Dominio.Dto.ParametrosInstanciaDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriGenerarInstanciasFlujo"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<List<Dominio.Dto.InstanciaResultado>>(respuesta);
        }

        public async Task<List<Dominio.Dto.InstanciaResultado>> GenerarInstanciasMasivo(List<Dominio.Dto.ParametrosInstanciaDto> parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriGenerarInstanciasMasivo"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros[0].IdUsuarioDNP);
            return JsonConvert.DeserializeObject<List<Dominio.Dto.InstanciaResultado>>(respuesta);
        }

        public async Task<IList<LogsInstanciasDto>> ObtenerLogInstancia(ParametrosLogsInstanciasDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerLogInstancia"];
            if (parametros.EsTramite)
            {
                parametros.TipoObjetoId = new Guid(ConfigurationManager.AppSettings["IdTipoTramite"]);
            }
            else
            {
                parametros.TipoObjetoId = new Guid(ConfigurationManager.AppSettings["IdTipoProyecto"]);
            }

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDnp);
            return JsonConvert.DeserializeObject<IList<LogsInstanciasDto>>(respuesta);
        }

       

        public async Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarProyecto"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.TramiteFiltroDto.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> CrearLogFlujo(FlujosLogsInstanciasDto logs, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriCrearLogFlujo"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, logs, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<IList<FlujosLogsInstanciasDto>> ObtenerFlujoLogInstancia(Guid instanciaId, Guid nivelId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerLogFlujoInstancia"];
            var uriParametros = $"?instanciaId={instanciaId}&nivelId={nivelId}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp);
            return JsonConvert.DeserializeObject<IList<FlujosLogsInstanciasDto>>(respuesta);
        }

        public async Task<List<HistoricoObservacionesDto>> ObtenerHistoricoObservaciones(Guid instanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerHistoricoObservaciones"];
            var uriParametros = $"?instanciaId={instanciaId}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<HistoricoObservacionesDto>>(respuesta);
        }

        public async Task<List<AutorizacionAccionesPorInstanciaDto>> ObtenerInstanciasPermiso(ParametrosObjetosNegocioDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerInstanciasPermiso"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<AutorizacionAccionesPorInstanciaDto>>(respuesta);
        }

        public async Task<RespuestaParametrosValidarFlujoDto> ValidarFlujoConInstanciaActiva(ParametrosValidarFlujoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriValidarFlujoConInstanciaActiva"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaParametrosValidarFlujoDto>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosInstancias(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriRegistrarPermisosInstancias"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> EliminarInstanciasPermiso(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarInstanciasPermiso"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasConsolaProcesos(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObjetosNegocioAccionesActivasYPausadasConsolaProcesos"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioDto>>(respuesta);
        }

        public async Task<List<TramiteDto>> ObtenerTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriTramitesConsolaProcesos"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteDto>>(respuesta);
        }

        public async Task<List<TramiteDto>> ObtenerProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriProgramacionConsolaProcesos"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteDto>>(respuesta);
        }

        public async Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriTiposTramites"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.ParametrosInboxDto.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TipoTramiteDto>>(respuesta);
        }

        public async Task<List<string>> ObtenerInstanciasActivasProyectos(string Bpins, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerInstanciasActivasProyectos"];
            var uriParametros = $"?Bpins={Bpins}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<string>>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> EliminarInstanciaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarInstanciaProyectoTramite"];
            var uriParametros = $"?instanciaTramite=" + instanciaTramite + "&Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<List<int>> ObtenerTramitesInstanciasEstadoCerrado(int proyectoId, int entidadId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerTramitesInstanciasEstadoCerrado"];
            var uriParametros = $"?proyectoId=" + proyectoId + "&entidadId=" + entidadId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<int>>(respuesta);
        }

        public async Task<TrazaAccionesPorInstanciaDto> ObtenerObservacionesPasoPadre(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["uriObtenerObservacionesPasoPadre"];
            var uriParametros = $"?idInstancia={idInstancia}&idAccion={idAccion}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<TrazaAccionesPorInstanciaDto>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosAccionPorUsuario(Dominio.RegistrarPermisosAccionDto permisosAccion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriRegistrarPermisosAccionPorUsuario"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, permisosAccion, permisosAccion.listadoUsuarios[0].IdUsuarioDNP);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> CerrarInstancia(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriCerrarInstancia"];
            var uriParametros = $"?tramiteId={tramiteId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<DetalleTramiteDto> ObtenerDetallesTramite(string numerotramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["UriObtenerDetalleTramite"];
            var uriParametros = $"?numeroTramite={numerotramite}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<DetalleTramiteDto>(respuesta);
        }

        public async Task<DetalleTramiteDto> ObtenerDetallesTramitePorInstancia(string instanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["UriDetalleTramitePorInstancia"];
            var uriParametros = $"?instanciaId={instanciaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<DetalleTramiteDto>(respuesta);
        }

        public async Task<List<InstanciaDto>> DevolverInstanciasHijas(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriDevolverInstanciasHijas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP);
            return JsonConvert.DeserializeObject<List<InstanciaDto>>(respuesta);
        }

        public async Task<ProyectoTramiteDto> ObtenerProyectosPorTramite(Guid? instanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["UriObtenerProyectosPorTramite"];
            var uriParametros = $"?instanciaId={instanciaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            var respuestaTmp = JsonConvert.DeserializeObject<List<ProyectoTramiteDto>>(respuesta);
            if (respuestaTmp != null)
                return respuestaTmp[0];
            else
                return new ProyectoTramiteDto();
        }

        public async Task<Dominio.Dto.ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, DateTime fechaiInicial, DateTime fechaFinal, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["UrlValidarConpesVigenciaFutura"];
            var urlParams = new
            {
                FechaInicial = fechaiInicial,
                FechaFinal = fechaFinal,
                TramiteId = tramiteId
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, String.Empty, urlParams, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.ResponseDto<bool>>(respuesta);
        }

        public async Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarAsociacionVFO"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, eliminacionAsociacionDto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        public async Task<List<LogDto>> ObtenerLog(Guid instanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerLog"];
            var uriParametros = $"?instanciaId={instanciaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<LogDto>>(respuesta);
        }

        public async Task<IList<SubpasoDto>> ObtenerLogSubpasos(Guid idInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerLogSubpasos"]; 
            var uriParametros = $"?instanciaId={idInstancia}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<IList<SubpasoDto>>(respuesta);
        }

        public async Task<List<TrazaAccionDto>> ObtenerTrazaInstancia(Guid idInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriFlujosObtenerTrazaInstancia"];
            var uriParametros = $"?instanciaId={idInstancia}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TrazaAccionDto>>(respuesta);
        }

        public async Task<List<Dominio.Dto.Flujos.InstanciaResultado>> CrearInstancia(ParametrosInstanciaFlujoDto parametrosInstanciaDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriCrearInstanciaFlujo"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametrosInstanciaDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<Dominio.Dto.Flujos.InstanciaResultado>>(respuesta);

        }

        public async Task<List<OpcionFlujoDto>> ObtenerPermisosFlujosPorAplicacionYRoles(FiltroConsultaOpcionesDto filtroConsulta, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriFlujosObtenerFlujosPorRolesPost"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, filtroConsulta, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<OpcionFlujoDto>>(respuesta);

        }

        public async Task<string> ObtenerEstadoOcultarObservacionesGenerales(string usuarioDnp)
        {
            var data = string.Empty;
            var result = await _serviciosNegocioServicios.ConsultarSystemConfiguracion("OcultarObservacionesGenerales", ",", usuarioDnp);
            if(result != null && result?.Valores != null)
            {
                foreach (var item in result.Valores)
                {
                    data = item.Valor;
                }
            }
            
            return data;
        }

        public async Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidadesSinInstanciasActivas(ParametrosProyectosFlujosDto parametros, string usuarioDnp)
        {
            var urlBase = string.Empty;
            var uri=string.Empty ;

            bool controlarPorSp = false;
            var tiposTramiteId = await _serviciosNegocioServicios.ConsultarSystemConfiguracion("TiposTramiteControlaCrearProcesoPorSP", ",", usuarioDnp);
            foreach (var item in tiposTramiteId.Valores)
            {
                if (parametros.tipoTramiteId.ToString().Contains(item.Valor))
                {
                    controlarPorSp = true;
                }
            }

            if (controlarPorSp == true)
            {
                urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
                uri = ConfigurationManager.AppSettings["uriFlujosObtenerProyectosASeleccionar"];
            }
            else
            {
                urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
                uri = ConfigurationManager.AppSettings["uriFlujosObtenerProyectos"]; 
            }

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDnp, useJWTAuth: false);
            List<ProyectoEntidadDto> LProyectos = JsonConvert.DeserializeObject<List<ProyectoEntidadDto>>(respuesta);
          
            return LProyectos;
        }

        public async Task<bool> ObtenerValidacionVerAccion(ValidarRolAccionDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["uriFlujosObtenerValidacionVerAccion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);

        }

        public async Task<InstanciaProyectoDto> ObtenerInstanciaProyecto(Guid idInstancia, string bpin,  string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["uriFlujosObtenerInstanciaProyecto"];
            uri = uri + "?idInstancia=" + idInstancia + "&bpin=" + bpin;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<InstanciaProyectoDto>(respuesta);

        }

        public async Task<int> CrearTrazaAccionesPorInstancia(TrazaAccionesPorInstanciaDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["uriFlujosCrearTrazaAccionesPorInstancia"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri,   string.Empty, parametros, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<int>(respuesta);

        }

        public async Task<List<DevolucionAccionesDto>> ObtenerDevolucionesPorIdInstanciaYIdAccion(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["uriFlujosObtenerDevolucionesPorIdInstanciaYIdAccion"];
            uri = uri + "?idInstancia=" + idInstancia + "&idAccion=" + idAccion;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<DevolucionAccionesDto>>(respuesta);

        }
        public async Task<ResultadoEjecucionFlujoDto> EjecutarFlujo(ParametrosEjecucionFlujo parametrosEjecucionFlujo,string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEjecutarFlujo"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametrosEjecucionFlujo, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoEjecucionFlujoDto>(respuesta);

        }

        public async Task<ResultadoDevolverFlujoDto> DevolverFlujo(ParametrosDevolverFlujoDto parametrosDevolucionFlujo, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriDevolverFlujo"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametrosDevolucionFlujo, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoDevolverFlujoDto>(respuesta);

        }

        public async Task<Dominio.Dto.InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarInstanciaCerrada_AbiertaProyectoTramite"];
            var uriParametros = $"?instanciaTramite=" + instanciaTramite + "&Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<Dominio.Dto.InstanciaResultado> NotificarUsuariosPorInstanciaPadre(Guid instanciaId, string nombreNotificacion, string texto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriNotificarUsuariosPorInstanciaPadre"];
            var uriParametros = $"?instanciaId=" + instanciaId + "&nombreNotificacion=" + nombreNotificacion + "&texto=" + texto;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        public async Task<List<FlujoDto>> ObtenerFlujosPorTipoObjeto(Guid tipoObjetoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerFlujosPorTipoObjeto"];
            var uriParametros = $"?tipoObjetoId=" + tipoObjetoId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<FlujoDto>>(respuesta);
        }

        public async Task<List<AccionesFlujosDto>> ObtenerAccionesFlujoPorFlujoId(Guid flujoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerAccionesFlujoPorFlujoId"];
            var uriParametros = $"?flujoId=" + flujoId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<AccionesFlujosDto>>(respuesta);
        }

        public async Task<List<int>> ObtenerVigencias(Guid tipoObjetoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObtenerVigencias"];
            var uriParametros = $"?tipoObjetoId=" + tipoObjetoId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<int>>(respuesta);
        }

        public async Task<bool> ExisteFlujoProgramacion(int entidadId, Guid flujoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriExisteFlujoProgramacion"];
            var uriParametros = $"?entidadId=" + entidadId + "&flujoId=" + flujoId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<bool> SubPasoEjecutar(ParametrosEjecucionSubPasoDto oParametrosEjecucionSubPasoDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriFlujosSubPasoEjecutar"];

            oParametrosEjecucionSubPasoDto.Usuario = usuarioDnp;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, oParametrosEjecucionSubPasoDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }
        
        public async Task<List<NegocioVerificacionOcadPazDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasVerificacionOcadPazSgr(ParametrosObjetosNegocioDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriObjetosNegocioAccionesActivasYPausadasVerificacionOcadPazSgr"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<NegocioVerificacionOcadPazDto>>(respuesta);
        }

        public async Task<EstadoFlujoResultado> SubPasosValidar(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriFlujosSubPasosValidar"];
            var uriParametros = $"?idInstancia=" + idInstancia + "&idAccion=" + idAccion + "&usuario=" + usuarioDnp;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<EstadoFlujoResultado>(respuesta);
        }

        public async Task<List<ProyectoPriorizarDto>> ConsultarProyectosPriorizarSGR(String IdUsuarioDNP, string usuarioDnp)
        {
            var urlBase = string.Empty;
            var uri = string.Empty;
            var uriParametros = $"?IdUsuarioDNP=" + IdUsuarioDNP;    

            urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uri = ConfigurationManager.AppSettings["uriProyectosPriorizarSGR"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            List<ProyectoPriorizarDto> LProyectos = JsonConvert.DeserializeObject<List<ProyectoPriorizarDto>>(respuesta);

            return LProyectos;
        }
    }
}
