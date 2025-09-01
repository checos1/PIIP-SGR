using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.IdentityModel.Services;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Comunes.Utilidades;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Web.UI.Comunes;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;

namespace DNP.Backbone.Web.UI.Controllers
{
    [ExcludeFromCodeCoverage]
    public class AccountController : BaseController
    {
        private readonly IDictionary<string, string> _openIdConfigurationsTenants;

        public AccountController()
        {
            _openIdConfigurationsTenants = new Dictionary<string, string>
            {
                { "b2c", "https://dnpb2c.b2clogin.com/dnpb2c.onmicrosoft.com/b2c_1_sign_in/v2.0/.well-known/openid-configuration" },
                { "b2b", "https://login.microsoftonline.com/dnp.gov.co/v2.0/.well-known/openid-configuration" }
            };
        }

        [AllowAnonymous]
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/", AllowRefresh = true },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType, Startup.PolicyB2CSignin);
            }
        }

        //receber por parametro o tipo de conexao ou cria outra entrada na controller
        [AllowAnonymous]
        public void SignInB2B(string loginUsuario)
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/",
                        AllowRefresh = true,
                        Dictionary = { { "login_hint", loginUsuario } }
                    },
                 Startup.PolicyB2BSignin);
            }
        }

        [AllowAnonymous]
        public void SignInMulti(string loginUsuario)
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/",
                        AllowRefresh = true,
                        Dictionary = { { "login_hint", loginUsuario } }
                    },
                 Startup.PolicyMultiTenantSignin);
            }
        }

        public ActionResult SignOut()
        {
            // To sign out the user, you should issue an OpenIDConnect sign out request.
            if (Request.IsAuthenticated)
            {

                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["MGAWebAppFedAuth"];
                cookie.Expires = DateTime.Now.AddDays(-365);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                Session.Clear();
                Session.Abandon();

                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);

                WSFederationAuthenticationModule authModule = FederatedAuthentication.WSFederationAuthenticationModule;
                WSFederationAuthenticationModule.FederatedSignOut(new Uri(authModule.Issuer), new Uri(authModule.Realm));
            }
            return RedirectToAction("SignOutCallback");

        }

        public ActionResult SignOutCallback()
        {
            //Llamar a la página de inicio, si no esta autorizado lo envia al STS para que su autenticaación
            return Redirect("/");
        }

        public async Task<ActionResult> ObtenerTokenAutorizacion()
        {
            PermisosEntidadDto permisosEntidad = new PermisosEntidadDto();
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { Exito = false });
            }
           
            await Task.Run(() => ObtieneUsuario());
            
            if (UsuarioLogueado.displayName  == "Sin autorización")
            {
                return Json(new
                {
                    Exito = false,
                    data = "No es posible  procesar su solicitud, no tiene cuentas activas en la PIIP",
                    ExceptionMessage = "No es posible  procesar su solicitud, no tiene cuentas activas en la PIIP"
                });
            }


            var claims = new List<Claim>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAutorizacion"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                var autorizacion = Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($"{UsuarioLogueado?.IdUsuarioDNP}:{User.Identity.GetHashCode()}")
                );

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic", autorizacion
                    );

                if (!(UsuarioLogueado is null))
                {

                    var permisos = await ObtenerPermisosPorEntidadAsync(client);
                    permisosEntidad = permisos;
                    claims.Add(new Claim("IdUsuarioDNP", UsuarioLogueado?.IdUsuarioDNP));
                    List<string> permissoEntidad = new List<string> { "EntidadesCargarDatos", "Usuarios:Editar", "Roles:Crear", "Roles:Editar", "Roles:Eliminar" };
                    foreach (var entidad in permisos.Entidades)
                    {
                        var id = entidad.IdEntidad;
                        //if (claims.Count <= 10)
                        //{
                        if (entidad.Opciones is null || !entidad.Opciones.Any())
                        {
                            claims.Add(new Claim($"PermissoEntidad={id}", "_NoPermisso"));
                        }
                        //int countOpciones = 0;
                        foreach (var opcion in entidad.Opciones)
                        {
                            if (permissoEntidad.Contains(opcion.IdOpcionDNP))
                            {
                                if (!claims.Exists(x => x.Type == $"PermissoEntidad={id}" && x.Value == opcion.IdOpcionDNP))
                                    claims.Add(new Claim($"PermissoEntidad={id}", opcion.IdOpcionDNP));
                            }
                            //if (countOpciones == 10)
                            //    break;
                            //else
                            //    countOpciones++;
                        }
                        //}
                    }

                    var menus = await ObtenerPermisosMenuAsync(client);
                    foreach (var item in menus)
                    {
                        claims.Add(new Claim("PermissoMenu", item));
                    }
                }

            }

            var tokenManager = new JwtTokenManager(ConfigurationManager.AppSettings["JwtSecret"]);
            var jwt = tokenManager.EscribirToken(claims, DateTime.Now.AddDays(1));

            var oldToken = Convert.ToBase64String(
                   Encoding.ASCII.GetBytes($"{UsuarioLogueado?.IdUsuarioDNP}:{User.Identity.GetHashCode()}"));

            return Json(new { Exito = true, Token = jwt, OldToken = oldToken });
        }

        public async Task<ActionResult> ValidarTokenAutorizacion(string token)
        {
            var tokenManager = new JwtTokenManager(ConfigurationManager.AppSettings["JwtSecret"]);
            var claims = tokenManager.ValidarToken(token);

            return await Task.FromResult(Json(new { Exito = true, Claims = claims }));
        }

        private async Task<PermisosEntidadDto> ObtenerPermisosPorEntidadAsync(HttpClient client)
        {
            var url = "api/Autorizacion/ObtenerPermisosPorEntidad";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PermisosEntidadDto>(respuesta);
            }
            else
            {
                throw new Exception("No pudimos procesar su solicitud");
            }
        }

        private async Task<List<string>> ObtenerPermisosMenuAsync(HttpClient client)
        {
            string url = "api/Autorizacion/ObtenerPermisosMenu";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<string>>(respuesta);
            }
            else
            {
                throw new Exception("No pudimos procesar su solicitud");
            }
        }

        public ActionResult SwitchSignShared(string redirectUrl, string tenant)
        {
            if (!Request.IsAuthenticated)
                return Redirect("/");

            var cookie = Request.Cookies["PIIPAuthCookie"];
            var id_token = (HttpContext.User as ClaimsPrincipal)?.Claims.FirstOrDefault(x => x.Type.Equals("id_token"))?.Value ?? string.Empty;

            var dicionario = new Dictionary<string, string>
            {
                { "id_token", id_token },
                { "cookie", cookie.Value },
                { "tenant", tenant }
            };

            var token = JsonConvert.SerializeObject(dicionario).EncodeToBase64();
            return Redirect($"{redirectUrl}Account/SignShared?token={token}");
        }

        [AllowAnonymous]
        public async Task<ActionResult> SignShared(string token)
        {
            try
            {
                if (Request.IsAuthenticated)
                    return RedirectToAction("Index", "Home");

                var serialize = token.DecodeFromBase64();
                var values = serialize.Deserialize<IDictionary<string, string>>();
                var id_token = values["id_token"];
                var cookie = values["cookie"];
                var tenant = values["tenant"];

                var principal = await ValidarTokenAsync(id_token, tenant);
                if (!principal?.Identity?.IsAuthenticated ?? true)
                    return Redirect("/");

                HttpContext.Response.SetCookie(new HttpCookie("PIIPAuthCookie", cookie));
                HttpContext.User = new ClaimsPrincipal(principal);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return Redirect("/");
            }
        }

        private async Task<IPrincipal> ValidarTokenAsync(string id_token, string tenant)
        {
            var openIdConfiguration = _openIdConfigurationsTenants[tenant];
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(openIdConfiguration);
            var config = await configManager.GetConfigurationAsync();

            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningTokens = config.SigningTokens,
                ValidateLifetime = false
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(id_token, parameters, out var securityToken);
        }
    }
}
