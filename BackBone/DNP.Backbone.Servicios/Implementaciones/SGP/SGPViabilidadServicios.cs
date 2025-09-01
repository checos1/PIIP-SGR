namespace DNP.Backbone.Servicios.Implementaciones.SGP
{

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    using Interfaces;
    using Interfaces.SGP;

    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.SGP;
    using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
    using DNP.Backbone.Dominio.Dto.SGP.Transversal;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto;
    using System;
    using DNP.Backbone.Dominio.Dto.Tramites;

    public class SGPViabilidadServicios : ISGPViabilidadServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="clienteHttpServicios">Instancia de clienteHttp</param>
        public SGPViabilidadServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<ParametroDto> SGPTransversalLeerParametro(string parametro, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGPTransversalLeerParametro"];
            var parametros = $"?parametro={parametro}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<ParametroDto>(respuesta);
            return lst;
        }

        /// <summary>
        /// Obtiene los requisitos de un proyecto
        /// </summary>
        public async Task<List<LstAcuerdoSectorClasificadorDto>> SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGPAcuerdoLeerProyecto"];
            var parametros = $"?proyectoId={proyectoId}&nivelId={nivelId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<LstAcuerdoSectorClasificadorDto>>(respuesta);

            return lst;
        }


        public async Task<string> SGPAcuerdoGuardarProyecto(AcuerdoSectorClasificadorSGPDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGPAcuerdoGuardarProyecto"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;
        }

        public async Task<List<ListaDto>> SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGPProyectosLeerListas"];
            var parametros = $"?nivelId={nivelId}&proyectoId={proyectoId}&nombreLista={nombreLista}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<ListaDto>>(respuesta);

            return lst;
        }

        public async Task<LeerInformacionGeneralViabilidadDto> SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode)
        {
            var uri = ConfigurationManager.AppSettings["uriSGPViabilidadLeerInformacionGeneral"];
            var parametros = $"?proyectoId={proyectoId}&instanciaId={instanciaId}&tipoConceptoViabilidadCode={tipoConceptoViabilidadCode}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            var response = JsonConvert.DeserializeObject<LeerInformacionGeneralViabilidadDto>(respuesta);
            return response;
        }

        public async Task<List<LeerParametricasViabilidadDto>> SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGPViabilidadLeerParametricas"];
            var parametros = $"?proyectoId={proyectoId}&nivelId={nivelId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var response = JsonConvert.DeserializeObject<List<LeerParametricasViabilidadDto>>(respuesta);
            return response;
        }

        public async Task<ResultadoProcedimientoDto> SGPViabilidadGuardarInformacionBasica(InformacionBasicaViabilidadSGPDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGPViabilidadGuardarInformacionBasica"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        public async Task<RespuestaGeneralDto> SGPCargarFirma(string firma, string rolId, string usuarioDnp)
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

        public async Task<RespuestaGeneralDto> SGPValidarSiExisteFirmaUsuario(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarSiExisteFirmaUsuario"];
            uriMetodo += "?idUsuario=" + usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> SGPFirmar(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriFirmarSgp"];
            object parametros = new
            {
                InstanciaId = instanciaId,
                TipoConceptoViabilidadId = tipoConceptoViabilidadId,
                EntidadId = entidadId
            };
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> SGPBorrarFirma(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriBorrarFirma"];
            FileToUploadDto parametros = new FileToUploadDto();
            parametros.UsuarioId = usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<string> GuardarProyectoViabilidadInvolucradosSGP(ProyectoViabilidadInvolucradosSGPDto objProyectoViabilidadInvolucradosDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoViabilidadInvolucradosAgregarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objProyectoViabilidadInvolucradosDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarProyectoViabilidadInvolucradosSGP(int id, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectoViabilidadInvolucradosEliminarSGP"];
            uri = $"{uri}?id={id}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<List<ProyectoViabilidadInvolucradosSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerProyectoViabilidadInvolucradosSGP"];
            uri = $"{uri}?proyectoId={proyectoId}&instanciaId={instanciaId}&tipoConceptoViabilidadId={tipoConceptoViabilidadId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoViabilidadInvolucradosSGPDto>>(response);
        }

        public async Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerEntidadDestinoResponsableFlujoSGP"];
            uri = $"{uri}?rolId={rolId}&crTypeId={crTypeId}&entidadResponsableId={entidadResponsableId}&proyectoId={proyectoId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<EntidadDestinoResponsableFlujoSgpDto>(response);
        }

        public async Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerEntidadDestinoResponsableFlujoTramiteSGP"];
            uri = $"{uri}?rolId={rolId}&entidadResponsableId={entidadResponsableId}&tramiteId={tramiteId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<EntidadDestinoResponsableFlujoSgpDto>(response);
        }

        public async Task<List<ProyectoViabilidadInvolucradosFirmaSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriLeerProyectoViabilidadInvolucradosFirmaSGP"];
            uri = $"{uri}?instanciaId={instanciaId}&tipoConceptoViabilidadId={tipoConceptoViabilidadId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoViabilidadInvolucradosFirmaSGPDto>>(response);
        }
    }
}
