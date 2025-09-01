using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Utilidades;
using DNP.Backbone.Web.UI.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DNP.Backbone.Comunes.Excepciones;
using System.Threading;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Identidad;


namespace DNP.Backbone.Web.UI.Comunes
{
    [ExcludeFromCodeCoverage]
    //[Authorize]
    public class BaseController : Controller
    {
        private static readonly string _jwtTokenServicio;
        private readonly IAutorizacionServicios _autorizacionServicios;
        private readonly IIdentidadServicios _identidadServicios;

        static BaseController()
        {
            var secret = ConfigurationManager.AppSettings["JwtSecret-Servicio"];

            var claims = new List<Claim>();
            claims.Add(new Claim("Tipo", "servicio"));
            claims.Add(new Claim("Nombre-Servicio", "DNP.Backbone.Web.UI"));

            var tokenManager = new JwtTokenManager(secret);
            _jwtTokenServicio = tokenManager.EscribirToken(claims, DateTime.UtcNow.AddYears(5));
        }

        public BaseController(IAutorizacionServicios autorizacionServicios, IIdentidadServicios identidadServicios)
        {
            _autorizacionServicios = autorizacionServicios;
            _identidadServicios = identidadServicios;
        }

        public UsuarioDto UsuarioLogueado { get; set; }
        public string UserObjectId { get { return ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value; } }
        public string UserTenantId { get { return ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value; } }
        
        private string apiIdentidadServicioBaseUri
        {
            get
            {
                return ViewBag.ApiAuditoriaServicioBaseUri is null ? ConfigurationManager.AppSettings["ApiIdentidad"] : ViewBag.ApiIdentidadServicioBaseUri;
            }
        }
       
        private string apiAutorizacionServicioBaseUri
        {
            get
            {
                return ViewBag.ApiAuditoriaServicioBaseUri is null ? ConfigurationManager.AppSettings["ApiAutorizacion"] : ViewBag.ApiAutorizacionServicioBaseUri;
            }
        }
        
        public BaseController() { }

        public string ObtenerTipoDocumentoEquivalente (string tipoDocumento, int tipoConversion)
        {
            string tipoDocumentoTemporal;

            tipoDocumentoTemporal = tipoConversion == 1 ? "CC" : "itcert";

            switch (tipoConversion)
            {
                case 1:
                    switch (tipoDocumento)
                    {
                        case "itcert":
                            tipoDocumentoTemporal = "CC";
                            break;
                        case "pasaporte":
                            tipoDocumentoTemporal = "PA";
                            break;
                        case "NIT":
                        case "nit":
                            tipoDocumentoTemporal = "NI";
                            break;
                        default:
                            tipoDocumentoTemporal = "CC";
                            break;
                    }
                    break;
                case 2:
                    switch (tipoDocumento)
                    {
                        case "CC":
                            tipoDocumentoTemporal = "itcert";
                            break;
                        case "PA":
                            tipoDocumentoTemporal = "pasaporte";
                            break;
                        case "NI":
                            tipoDocumentoTemporal = "NIT";
                            break;
                        default:
                            tipoDocumentoTemporal = "itcert";
                            break;
                    }
                    break;
            }

            return tipoDocumentoTemporal;
        }
        
        public string FormatearIdUsuarioDNP(string usuario)
        {

            string usuarioTemporal;
            string numeroDocumento;
            string tipoDocumento;

            // Usuario STS
            if (usuario.Contains("#"))
            {
                tipoDocumento = this.ObtenerTipoDocumentoEquivalente(usuario.Replace(":", "").Split('#')[0], 1);
                numeroDocumento = usuario.Replace(":", "").Split('#')[1];
                usuarioTemporal = tipoDocumento + numeroDocumento;
            }
            else // Usuario DNP
            {
                // Buscar usuario por usuario DNP
                usuarioTemporal = usuario + "@dnp.gov.co"; // " Ej: usuario@dnp.gov.co";
            }

            return usuarioTemporal;
        }

        public async Task ObtieneUsuario()
        {
            try
            {
                string usuarioTemporal = FormatearIdUsuarioDNP(User.Identity.Name);
                string tipoUsuario = "externo";
                if (usuarioTemporal.Contains("@"))
                {
                    tipoUsuario = "interno";
                    usuarioTemporal = await BuscarUsuarioPIIPXCorreoDNP(usuarioTemporal);
                }

                await ObtieneUsuarioAAD(usuarioTemporal);
                var usuarioPIIP = await ObtenerUsuarioAutorizacionPorCuenta(usuarioTemporal);

                UsuarioLogueado.tipoUsuario = tipoUsuario;

                if (!(usuarioPIIP is null))
                {
                    UsuarioLogueado.displayName = usuarioPIIP?.Usuario.Nombre;
                    UsuarioLogueado.IdUsuarioDNP = usuarioPIIP?.Usuario.IdUsuarioDnp;
                    UsuarioLogueado.IdUsuarioPIIP = usuarioPIIP is null ? Guid.NewGuid() : usuarioPIIP.IdUsuario;
                } else
                {
                    //UsuarioLogueado.displayName = "Sin autorización";
                    //UsuarioLogueado.IdUsuarioDNP = this.FormatearIdUsuarioDNP(User.Identity.Name);
                    //UsuarioLogueado.IdUsuarioPIIP = Guid.NewGuid();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ObtieneUsuarioAAD(string usuarioTemporal)
        {
            try
            {
                if (User != null && User.Identity.Name != null)
                {
                    string resultado = string.Empty;
                    HttpResponseMessage response;

                    response = await EjecutarlHttpClient($"{apiAutorizacionServicioBaseUri}api/Autorizacion/ObtenerUsuarioPorIdUsuarioDnp?idUsuarioDnp={usuarioTemporal}");
                    UsuarioLogueado = ObtenerUsuarioDto(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private UsuarioDto ObtenerUsuarioDto(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<UsuarioDto>(resultado);
            }
            else
                return null;
        }

        public async Task<string> BuscarUsuarioPIIPXCorreoDNP(string usuarioTemporal)
        {
            string resultado = string.Empty;
            var response = await EjecutarlHttpClient($"{apiAutorizacionServicioBaseUri}api/Autorizacion/ObtenerUsuarioPIIPXCorreoDNP?usuarioTemporal={usuarioTemporal}");
            if (response.IsSuccessStatusCode)
            {
                resultado = await response.Content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<string>(resultado);
        }

        public async Task<UsuarioCuentaDto> ObtenerUsuarioAutorizacionPorCuenta(string cuenta)
        {
            string resultado = string.Empty;
            var response = await EjecutarlHttpClient($"{apiAutorizacionServicioBaseUri}api/Autorizacion/ObtenerCuentaUsuario?nomeCuenta={cuenta}");
            if (response.IsSuccessStatusCode)
            {
                resultado = await response.Content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<UsuarioCuentaDto>(resultado);
        }

        public async Task<List<RolDto>> ObtenerRoles(string usuarioDNP)
        {
            try
            {
                var response = EjecutarlHttpClient(
                    $"{apiIdentidadServicioBaseUri}api/Autorizacion/ObtenerRolesPorUsuario",
                    new UsuarioInboxDto
                    {
                        IdUsuarioDnp = usuarioDNP
                    }).Result;
                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<RolDto>>(resultado);
                }
                else
                {
                    throw new Exception("No pudimos procesar su solicitud");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> EjecutarlHttpClient(string url, dynamic parameters = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.Timeout = TimeSpan.FromMinutes(10);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                HttpClientAsignarAutorizacionJWT(client, User.Identity.Name);

                if (parameters is null)
                    return await client.GetAsync(url);
                else
                {
                    var jsonContent = JsonConvert.SerializeObject(parameters);
                    var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    return await client.PostAsync(url, contentString);
                }
            }
        }

        public string Autorizacion() => Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
                        $"{User.Identity.Name}:{User.Identity.GetHashCode()}"));

        protected void HttpClientAsignarAutorizacionJWT(HttpClient client, string usuarioDnp)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _jwtTokenServicio);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization-Type", "JWT");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Application-User", usuarioDnp);
        }
    }
}