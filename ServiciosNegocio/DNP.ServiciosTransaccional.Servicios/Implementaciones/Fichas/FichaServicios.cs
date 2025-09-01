
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Fichas
{
    public class FichaServicios : ServicioBase<FichaPlantillaReporteDto>, IFichaServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string _UrlFicha = ConfigurationManager.AppSettings["ApiFicha"];
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public FichaServicios(IClienteHttpServicios clienteHttpServicios, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ObtenerAnexoRadicadoTramite(int tramiteId, string nombreReporte, string idReporte, string usuarioDnp)
        {
            var uriAnexoRadicado = ConfigurationManager.AppSettings["uriFichaFisico"];

            var model = new {
                tramiteId = tramiteId,
                NombreReporte = nombreReporte,
                IdReporte = idReporte
            };

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostFile,
                    _UrlFicha,
                    uriAnexoRadicado,
                    string.Empty,
                    model,
                    usuarioDnp,
                    useJWTAuth: false
            );


            if (string.IsNullOrWhiteSpace(jsonResponse)) {
                throw new Exception("Ocurrió un en ObtenerAnexoRadicadoTramite");
            }

            return jsonResponse;
        }

        public async Task<FichaPlantillaReporteDto> ObtenerPlantillaReporteAnexo(string nombreReporteRadicado, string usuarioDnp)
        {
            var uriPlantillaAnexo = ConfigurationManager.AppSettings["uriFichaPlantillaReporte"];

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    _UrlFicha,
                    string.Format("{0}{1}", uriPlantillaAnexo, nombreReporteRadicado),
                    string.Empty,
                    null,
                    usuarioDnp,
                    useJWTAuth: false
            );

            if (string.IsNullOrWhiteSpace(jsonResponse)) {
                throw new Exception("Ocurrió un error en ObtenerPlantillaReporteAnexo");
            }

            var response = JsonConvert.DeserializeObject<FichaPlantillaReporteDto>(jsonResponse);

            return response;
        }

        public async Task<string> ObtenerFichaFisicaSGR(string instanciaId, string nivelId, string tramiteId, string nombreReporte, string idReporte, bool borrador, string usuarioDnp)
        {
           
                var uriAnexoRadicado = ConfigurationManager.AppSettings["uriFichaFisicoSGR"];

                var model = new
                {
                    TramiteId = tramiteId,
                    NombreReporte = nombreReporte,
                    IdReporte = idReporte,
                    PARAM_BORRADOR = borrador,
                    PARAM_BPIN = tramiteId,
                    InstanciaId = instanciaId,
                    NivelId = nivelId
                };

                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.PostFile,
                        _UrlFicha,
                        uriAnexoRadicado,
                        string.Empty,
                        model,
                        usuarioDnp,
                        useJWTAuth: false
                );

            var parametrosGuardar = new ParametrosGuardarDto<object>
            {
                Contenido = model
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = usuarioDnp,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, TipoMensajeEnum.Creacion, "ObtenerFichaFisicaSGR");

            if (string.IsNullOrWhiteSpace(jsonResponse))
                {
                    throw new Exception("Ocurrió un error en ObtenerFichaFisicaSGR");
                }

                return jsonResponse;
        }

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<FichaPlantillaReporteDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
