using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.DesignacionEjecutor;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SGR;
using DNP.Backbone.Dominio.Dto.SGR.AvalUso;
using DNP.Backbone.Dominio.Dto.SGR.CTEI;
using DNP.Backbone.Dominio.Dto.SGR.CTUS;
using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
using DNP.Backbone.Dominio.Dto.SGR.OcadPaz;
using DNP.Backbone.Dominio.Dto.SGR.Reportes;
using DNP.Backbone.Dominio.Dto.SGR.Transversal;
using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;


namespace DNP.Backbone.Servicios.Implementaciones.SGR
{
    public class SGRServicios : ISGRServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="clienteHttpServicios">Instancia de clienteHttp</param>
        public SGRServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// llamado al servicio para obtener Operacion Credito Datos Generales
        /// </summary>
        /// <param name="proyectoFuenteFinanciacionAgregarDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<OperacionCreditoDatosGeneralesDto> ObtenerOperacionCreditoDatosGenerales(string bpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerOperacionCreditoDatosGenerales"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?Bpin={bpin}", null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return new OperacionCreditoDatosGeneralesDto();
            return JsonConvert.DeserializeObject<OperacionCreditoDatosGeneralesDto>(respuesta);
        }

        /// <summary>
        /// llamado al servicio para agregar Operacion Credito Datos Generales
        /// </summary>
        /// <param name="OperacionCreditoDatosGeneralesDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns></returns>
        public async Task<string> GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarOperacionCreditoDatosGenerales"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, OperacionCreditoDatosGeneralesDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarOperacionCreditoSGR(int proyectoid, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarOperacionCreditoSGR"];
            uri += $"?proyectoid={proyectoid}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// llamado al servicio para obtener Operacion Credito Detalles
        /// </summary>
        /// <param name="proyectoFuenteFinanciacionAgregarDto"></param>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<OperacionCreditoDetallesDto> ObtenerOperacionCreditoDetalles(string bpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerOperacionCreditoDetalles"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?Bpin={bpin}", null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return new OperacionCreditoDetallesDto();
            return JsonConvert.DeserializeObject<OperacionCreditoDetallesDto>(respuesta);
        }

        /// <summary>
        /// llamado al servicio para agregar Operacion Credito Detalles
        /// </summary>
        /// <param name="OperacionCreditoDetallesDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns></returns>
        public async Task<string> GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarOperacionCreditoDetalles"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, OperacionCreditoDetallesDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// Obtiene los ejecutores de acuerdo al tipo de Entidad
        /// </summary>
        /// <param name="idTipoEntidad"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns></returns>
        public async Task<List<EjecutorCatalogoDto>> ObtenerEjecutorByTipoEntidad(int idTipoEntidad, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarEjecutorPorTipoEntidadId"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?idTipoEntidad={idTipoEntidad}", null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EjecutorCatalogoDto>>(respuesta);
        }

        /// <summary>
        /// Obtiene los ejecutores de acuerdo a los filtros indicados
        /// </summary>
        /// <param name="nit"></param>
        /// <param name="tipoEntidadId"></param>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        public async Task<List<EjecutorEntidadDto>> ObtenerListadoEjecutores(string nit, int? tipoEntidadId, int? entidadId, string usuarioDNP)
        {
            string parametros = string.Empty;

            if (nit == "" && tipoEntidadId == null && entidadId == null)
            {
                parametros = "";
            }
            else if (nit != "" && (tipoEntidadId == null && entidadId == null))
            {
                parametros = $"?nit={nit}";

            }
            else if (nit == "" && tipoEntidadId != null && entidadId == null)
            {
                parametros = $"?tipoEntidadId={tipoEntidadId}";
            }
            else if (nit == "" && tipoEntidadId != null && entidadId != null)
            {
                parametros = $"?tipoEntidadId={tipoEntidadId}&entidadId={entidadId}";
            }
            else if (nit != "" && tipoEntidadId != null && entidadId == null)
            {
                parametros = $"?nit={nit}&tipoEntidadId={tipoEntidadId}";
            }
            else
            {
                parametros = $"?nit={nit}&tipoEntidadId={tipoEntidadId}&entidadId={entidadId}";
            }

            var uri = ConfigurationManager.AppSettings["uriConsultarEjecutores"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EjecutorEntidadDto>>(respuesta);
        }

        public async Task<List<EjecutorEntidadAsociado>> ObtenerListadoEjecutoresAsociados(int proyectoId, string usuarioDNP)
        {
            string parametros = $"?proyectoId={proyectoId}";

            var uri = ConfigurationManager.AppSettings["uriConsultarEjecutoresAsociados"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EjecutorEntidadAsociado>>(respuesta);
        }

        public async Task<bool> CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId)
        {
            var uri = ConfigurationManager.AppSettings["uriCrearEjecutorAsociado"] + $"?proyectoId={proyectoId}&ejecutorId={ejecutorId}&usuario={usuario}&tipoEjecutorId={tipoEjecutorId}"; ;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<SeccionesEjecutorEntidad> EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarEjecutorAsociado"] + $"?EjecutorAsociadoId={EjecutorAsociadoId}&usuario={usuario}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<SeccionesEjecutorEntidad>(respuesta);
        }



        public async Task<DesagregarRegionalizacionDto> ObtenerDesagregarRegionalizacionSgr(string bpin, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerDesagregarRegionalizacionSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<DesagregarRegionalizacionDto>(respuesta);
        }
        //public async Task<DatosGeneralesProyectosDto> ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId, string usuario)
        //{
        //    var uri = ConfigurationManager.AppSettings["uriObtenerDatosGeneralesProyectoSgr"];
        //    var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?pProyectoId={pProyectoId}&pNivelId={pNivelId}", null, usuario, useJWTAuth: false);
        //    return JsonConvert.DeserializeObject<DatosGeneralesProyectosDto>(respuesta);
        //}
        public async Task<FocalizacionPoliticaSgrDto> ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerFocalizacionPoliticasTransversalesFuentesSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<FocalizacionPoliticaSgrDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarFocalizacionCategoriasAjustesSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, focalizacionCategoriasAjuste, usuario);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        //public async Task<PoliticasTCrucePoliticasDto> ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente, string usuario)
        //{
        //    var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesCrucePoliticasSgr"];
        //    var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}&IdFuente={IdFuente}", null, usuario, useJWTAuth: false);
        //    return JsonConvert.DeserializeObject<PoliticasTCrucePoliticasDto>(respuesta);
        //}
        //public async Task<string> ObtenerDatosIndicadoresPoliticaSgr(string bpin, string usuario)
        //{
        //    var uri = ConfigurationManager.AppSettings["uriObtenerDatosIndicadoresPoliticaSgr"];
        //    var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuario, useJWTAuth: false);
        //    if (string.IsNullOrEmpty(respuesta)) return string.Empty;
        //    return respuesta;
        //}
        public async Task<string> ObtenerCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosCategoriaProductosPoliticaSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?Bpin={bpin}&fuenteId={fuenteId}&politicaId={politicaId}", null, usuarioDnp, useJWTAuth: false);
            return response;
        }

        public async Task<string> ObtenerPoliticasTransversalesProyectoSgr(string bpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesProyectoSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> EliminarPoliticasProyectoSgr(int proyectoId, int politicaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarPoliticasProyectoSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?proyectoId={proyectoId}&politicaId={politicaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> AgregarPoliticasTransversalesAjustesSgr(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriAgregarPoliticasTransversalesAjustesSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPoliticaTransversalDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        public async Task<string> ConsultarPoliticasCategoriasIndicadoresSgr(System.Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarPoliticasCategoriasIndicadoresSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> ObtenerPoliticasTransversalesCategoriasSgr(System.Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesCategoriasSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> EliminarCategoriasPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarCategoriasPoliticasProyectoSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?proyectoId={proyectoId}&politicaId={politicaId}&categoriaId={categoriaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriModificarPoliticasCategoriasIndicadoresSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametrosGuardar, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        public async Task<string> ObtenerCrucePoliticasAjustesSgr(System.Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCrucePoliticasAjustesSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        public async Task<string> GuardarCrucePoliticasAjustesSgr(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarCrucePoliticasAjustesSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objListCruecePoliticasAjustesDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        public async Task<string> ObtenerPoliticasTransversalesResumenSgr(System.Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesResumenSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<List<TipoDocumentoSoporteDto>> ObtenerTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, string usuarioDnp, string nivelId, string instanciaId, string accionId)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerDocumentosSoporteSgr"];
            uri = $"{uri}?tipoTramiteId={tipoTramiteId}&roles={roles}&tramiteId={tramiteId}&nivelId={nivelId}&instanciaId={instanciaId}&accionId={accionId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TipoDocumentoSoporteDto>>(response);
        }

        public async Task<List<TipoDocumentoSoporteDto>> ObtenerListaTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, string usuarioDnp, string nivelId, string instanciaId, string accionId)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListaDocumentosSoporteSgr"];
            uri = $"{uri}?tipoTramiteId={tipoTramiteId}&roles={roles}&tramiteId={tramiteId}&nivelId={nivelId}&instanciaId={instanciaId}&accionId={accionId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TipoDocumentoSoporteDto>>(response);
        }

        public async Task<List<ProyectoViabilidadInvolucradosDto>> LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerProyectoViabilidadInvolucrados"];
            uri = $"{uri}?proyectoId={proyectoId}&tipoConceptoViabilidadId={tipoConceptoViabilidadId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoViabilidadInvolucradosDto>>(response);
        }

        public async Task<List<ProyectoViabilidadInvolucradosFirmaDto>> LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerProyectoViabilidadInvolucradosFirma"];
            uri = $"{uri}?instanciaId={instanciaId}&tipoConceptoViabilidadId={tipoConceptoViabilidadId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoViabilidadInvolucradosFirmaDto>>(response);
        }

        public async Task<string> GuardarProyectoViabilidadInvolucrados(ProyectoViabilidadInvolucradosDto objProyectoViabilidadInvolucradosDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoViabilidadInvolucradosAgregar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objProyectoViabilidadInvolucradosDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarProyectoViabilidadInvolucradoso(int id, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoViabilidadInvolucradosEliminar"];
            uri = $"{uri}?id={id}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriTiposRecursosEntidadPorGrupoRecursos"];
            uriMetodo = $"{uriMetodo}?entityTypeCatalogId={entityTypeCatalogId}&resourceGroupId={resourceGroupId}&incluir={incluir}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCargarFirma"];
            FileToUploadDto parametros = new FileToUploadDto
            {
                FileAsBase64 = firma,
                RolId = new Guid(rolId),
                UsuarioId = usuarioDnp
            };
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarSiExisteFirmaUsuario"];
            uriMetodo += "?idUsuario=" + usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> Firmar(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriFirmarSgr"];
            object parametros = new
            {
                InstanciaId = instanciaId,
                TipoConceptoViabilidadId = tipoConceptoViabilidadId,
                EntidadId = entidadId
            };

            var firma = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));

            if (firma.Exito)
            {
                uriMetodo = ConfigurationManager.AppSettings["uriSGR_Transversal_LeerUsuariosNotificacionInvolucrados"];
                var parametro = $"?instanciaId={instanciaId}&usuarioFirma={usuarioDnp}";
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, parametro, null, usuarioDnp, useJWTAuth: false);
                var usuarios = JsonConvert.DeserializeObject<List<UsuariosProyectoDto>>(respuesta);

                List<ParametrosCrearNotificacionFlujoDto> data = new List<ParametrosCrearNotificacionFlujoDto>();

                data = usuarios.Select(u => new ParametrosCrearNotificacionFlujoDto()
                {
                    IdUsuarioDNP = u.Usuario,
                    NombreNotificacion = "Proceso de firma",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(3),
                    ContenidoNotificacion = u.Notificacion
                }).ToList();

                try
                {
                    var uriServicioNotificacion = ConfigurationManager.AppSettings["ApiNotificacion"] + ConfigurationManager.AppSettings["uriCrearNotificacionFlujo"];
                    var notificacion = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.PostAsync,
                        uriServicioNotificacion,
                        "",
                        null,
                        data,
                        usuarioDnp,
                        useJWTAuth: false);
                }
                catch { }
            }
            return firma;
        }

        public async Task<RespuestaGeneralDto> EliminarFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarFirmaSgr"];
            object parametros = new
            {
                InstanciaId = instanciaId,
                TipoConceptoViabilidadId = tipoConceptoViabilidadId,
                EntidadId = entidadId
            };

            var firma = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
            return firma;
        }

        public async Task<RespuestaGeneralDto> BorrarFirma(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriBorrarFirma"];
            FileToUploadDto parametros = new FileToUploadDto();
            parametros.UsuarioId = usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<ProyectoCtusDto> SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectoCtusSgr"];
            uri = $"{uri}?ProyectoId={ProyectoId}&instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ProyectoCtusDto>(response);
        }

        public async Task<List<EntidadesSolicitarCtusDto>> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerEntidadesSolicitarCtusrSgr"];
            uri = $"{uri}?ProyectoId={ProyectoId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EntidadesSolicitarCtusDto>>(response);
        }

        public async Task<string> GuardarProyectoSolicitarCTUS(ProyectoCtusDto objProyectoCtusDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarProyectoCtusSgr"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objProyectoCtusDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<ConfiguracionReportesDto> SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConfiguracionReportes"];
            uri = $"{uri}?instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ConfiguracionReportesDto>(response);
        }

        public async Task<List<UsuariosVerificacionOcadPazDto>> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerUsuariosVerificacionOcadPaz"];
            uri = $"{uri}?rolId={rolId}&entidadId={entidadId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<UsuariosVerificacionOcadPazDto>>(response);
        }

        public async Task<ResultadoProcedimientoDto> SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_OCADPaz_GuardarAsignacionUsuarioEncargado"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        public async Task<bool> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["SGR/Transversal/AutorizacionAccionesPorInstanciaSubFlujoOCADPaz"];
            uri = $"{uri}?instanciaId={instanciaId}&RolId={RolId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(response);
        }

        public async Task<List<MensajeDto>> SGR_Proyectos_validarTecnicoOcadpaz(Guid instanciaId, Guid accionId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Proyectos_validarTecnicoOcadpaz"];
            uri = $"{uri}?instanciaId={instanciaId}&accionId={accionId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<MensajeDto>>(respuesta);
            return lst;
        }

        public async Task<ValidacionOCADPazDto> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriValidacionOCADPaz"];
            uri = $"{uri}?proyectoId={proyectoId}&nivelId={nivelId}&instanciaId={instanciaId}&flujoId={flujoId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ValidacionOCADPazDto>(response);
        }

        public async Task<string> ValidarInstanciaCTUSNoFinalizada(int idProyecto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_ValidarInstanciaCTUSNoFinalizada"];
            uri = $"{uri}?idProyecto={idProyecto}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return String.IsNullOrEmpty(response) ? "" : JsonConvert.DeserializeObject<string>(response);
        }

        public async Task<int> ValidarViavilidadCumplimentoFlujoSGR(Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoCumplimentoFlujoSGR"];
            uri = $"{uri}?instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<int>(response);
        }

        public async Task<bool> TieneInstanciaActiva(String ObjetoNegocioId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoTieneInstanciaActivaSGR"];
            uri = $"{uri}?ObjetoNegocioId={ObjetoNegocioId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(response);
        }

        #region Entidad Nacional SGR

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>List<EntidadesAdscritasDto></returns> 
        public async Task<List<EntidadesAdscritasDto>> SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerEntidadesAdscritas"];
            uri = $"{uri}?proyectoId={proyectoId}&tipoEntidad={tipoEntidad}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            response = JsonConvert.DeserializeObject<string>(response);
            var listaEnt = JsonConvert.DeserializeObject<List<EntidadesAdscritasDto>>(response);
            return listaEnt;
        }

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="usuarioDnp"></param>  
        /// <returns>Json</returns> 
        public async Task<ResultadoProcedimientoDto> SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriValidarEntidadDelegada"];
            uri = $"{uri}?proyectoId={proyectoId}&tipo={tipo}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        /// <summary>
        /// Validar usuario encargado
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="instanciaId"></param>  
        /// <param name="usuarioDnp"></param>  
        /// <returns>UsuarioEncargadoDto</returns> 
        public async Task<List<ListaUsuarioDto>> SGR_Proyectos_LeerAsignacionUsuarioEncargado(int proyectoId, Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerAsignacionUsuarioEncargado"];
            uri = $"{uri}?proyectoId={proyectoId}&instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            response = JsonConvert.DeserializeObject<string>(response);
            if (response == null)
                return new List<ListaUsuarioDto>();
            else
                return JsonConvert.DeserializeObject<List<ListaUsuarioDto>>(response);
        }

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="delegado"></param> 
        /// <param name="usuarioDnp"></param> 
        /// <returns>int</returns> 
        public async Task<bool> SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriActualizarEntidadAdscrita"];
            uri = $"{uri}?proyectoId={proyectoId}&entityId={entityId}&delegado={delegado}&user={usuarioDnp}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            var listaEnt = JsonConvert.DeserializeObject<bool>(response);
            return listaEnt;
        }

        /// <summary>
        /// Guardar asignacion usuario encargado
        /// </summary> 
        /// <param name="json"></param> 
        /// <param name="usuarioDnp"></param> 
        /// <returns>ResultadoProcedimientoDto</returns> 
        public async Task<ResultadoProcedimientoDto> SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarAsignacionUsuarioEncargado"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, json, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(respuesta);
        }

        #endregion Entidad Nacional SGR

        #region CTEI

        public async Task<string> SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerDatosAdicionalesCTEISgr"];
            uri = $"{uri}?proyectoId={proyectoId}&instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(response);
        }

        public async Task<string> SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosAdicionalesCTEISgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            //return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }
        #endregion CTEI

        #region Aval de Uso
        public async Task<string> SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarAvalUsoSgr"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            //return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerAvalUsoSgr"];
            uri = $"{uri}?proyectoId={proyectoId}&instanciaId={instanciaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(response);
        }
        #endregion

        #region Priorizacion
        public async Task<List<EstadosPriorizacionDto>> SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Proyectos_MostrarEstadosPriorizacion"];
            var parametros = $"?proyectoId={proyectoId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<EstadosPriorizacionDto>>(respuesta);

            return lst;
        }
        #endregion

        #region Aprobacion
        public async Task<IEnumerable<ProyectoAprobacionInstanciasDto>> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectoAprobacionInstanciasSGR"];
            uri += "?instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoAprobacionInstanciasDto>>(respuesta);
        }

        public async Task<ProyectoAprobacionInstanciasResultado> GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarProyectoAprobacionInstanciasSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoAprobacionInstanciasDto, UsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ProyectoAprobacionInstanciasResultado>(respuesta);
        }

        public async Task<ProyectoProcesoResultado> GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarGuardarProyectoPermisosProcesoSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoProcesoDto, UsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ProyectoProcesoResultado>(respuesta);
        }

        public async Task<IEnumerable<ProyectoAprobacionResumenDto>> ObtenerProyectoResumenAprobacionSGR(string proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectoResumenAprobacionSGR"];
            uri += "?proyectoId=" + proyectoId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var jsonInterno = JsonConvert.DeserializeObject<List<ProyectoAprobacionResumenDto>>(respuesta);
            return jsonInterno;
        }
        public async Task<IEnumerable<ProyectoAprobacionResumenDto>> ObtenerProyectoResumenAprobacionCreditoParcialSGR(string proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectoResumenAprobacionCreditoParcialSGR"];
            uri += "?proyectoId=" + proyectoId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var jsonInterno = JsonConvert.DeserializeObject<List<ProyectoAprobacionResumenDto>>(respuesta);
            return jsonInterno;
        }

        public async Task<IEnumerable<ProyectoResumenEstadoAprobacionCreditoDto>> ObtenerProyectoResumenEstadoAprobacionCreditoSGR(string proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectoResumenEstadoAprobacionCreditoSGR"];
            uri += "?proyectoId=" + proyectoId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            var jsonInterno = JsonConvert.DeserializeObject<List<ProyectoResumenEstadoAprobacionCreditoDto>>(respuesta);
            return jsonInterno;
        }


        #endregion

        #region Designacion Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuarioDNP"></param>
        /// <returns>bool</returns> 
        public async Task<bool> RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_RegistrarRespuestaEjecutorSGR"];
            var resp = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, valores, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(resp);
        }

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>string</returns>
        public async Task<string> ObtenerRespuestaEjecutorSGR(string campo, int proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_ObtenerRespuestaEjecutorSGR"];
            uri = $"{uri}?campo={campo}&proyectoId={proyectoId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        /// <summary>
        /// Obtiene los valores de la aprobación por proyectoId.
        /// </summary>    
        /// <param name="proyectoId"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>string</returns>
        public async Task<string> LeerValoresAprobacionSGR(int proyectoId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_LeerValoresAprobacionSGR"];
            uri = $"{uri}?proyectoId={proyectoId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>  
        /// <param name="valores"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>bool</returns>
        public async Task<bool> ActualizarValorEjecutorSGR(CampoItemValorDto valores, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_ActualizarValorEjecutorSGR"];
            var resp = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, valores, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(resp);
        }

        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        public async Task<string> ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_ObtenerValorCostosEstructuracionViabilidadSGR"];
            uri = $"{uri}?instanciaId={instanciaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        #endregion Designacion Ejecutor

        public async Task<List<EjecutorEntidadAsociado>> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId, string usuarioDNP)
        {
            //string parametros = $"?proyectoId={proyectoId}&tipoEjecutorId ={tipoEjecutorId}";

            //var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_ConsultarEjecutorbyTipo"];
            //var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            var uri = ConfigurationManager.AppSettings["uriSGR_Procesos_ConsultarEjecutorbyTipo"];

            uri = $"{uri}?proyectoId={proyectoId}&tipoEjecutorId={tipoEjecutorId}";

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<EjecutorEntidadAsociado>>(respuesta);

        }

    }
}
