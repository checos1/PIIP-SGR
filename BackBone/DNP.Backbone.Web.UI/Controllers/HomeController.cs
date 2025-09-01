namespace DNP.Backbone.Web.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Comunes;
    using DNP.Backbone.Comunes.Dto;
    using Dto;
    using Newtonsoft.Json;
    using Servicios;
    using Servicios.Implementaciones;
    using Servicios.Interfaces;
    using DNP.Autorizacion.Dominio.Dto;
    using System.Linq.Expressions;

    [ExcludeFromCodeCoverage]
    [Authorize]
    public class HomeController : Comunes.BaseController
    {
        private readonly IAutorizacionServicio _autorizacionFlujos;
        private readonly IOpcionesServicio _opcionesServicio;

        public HomeController()
        {
            _autorizacionFlujos = new AutorizacionServicio(this);
            _opcionesServicio = new OpcionesServicio(this);
        }

        //Sobrecarga crear para permitir testear la clase.
        public HomeController(IOpcionesServicio opcionesServicio)
        {
            _opcionesServicio = opcionesServicio;
        }

        private void InicializarVariablesGlobales()
        {
            _opcionesServicio.InicializarVariablesGlobales();
        }

        public ActionResult Desautorizado()
        {
            return View();
        }

        public async Task<ActionResult> Index()
        {

            try
            {
                InicializarVariablesGlobales();

                List<MatrizOpcionesDto> opciones = new List<MatrizOpcionesDto>();
                ViewBag.ListaOpciones = opciones;

                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    await base.ObtieneUsuario();

                    ViewBag.UsuarioGuid = base.UserObjectId;
                    ViewBag.UsuarioDNP = base.UsuarioLogueado?.IdUsuarioDNP;
                    ViewBag.IdUsuarioPIIP = base.UsuarioLogueado?.IdUsuarioPIIP.ToString();
                    ViewBag.UsuarioTipoB2C = base.UsuarioLogueado?.UsuarioTipoB2C;
                    ViewBag.UsuarioLogueado = base.UsuarioLogueado?.displayName;
                    ViewBag.TipoUsuario = base.UsuarioLogueado?.tipoUsuario;

                    if (ViewBag.UsuarioDNP is null) {
                        ViewBag.UsuarioDNP = null;
                    }
                    else
                    {
                        await ObtenerPermisosMenu(ViewBag.UsuarioDNP);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        private async Task<string> getUsuarioDNP()
        {
            await base.ObtieneUsuario();
            return base.UsuarioLogueado.IdUsuarioDNP;
        }

        private async Task<UsuarioDto> ObtieneUsuarioAsync(string urlApiIdentidad, string userObjectId)
        {
            try
            {
                await base.ObtieneUsuario();
                return base.UsuarioLogueado;
            }
            catch (Exception e)
            {
                var resultado = e;
                return null;
            }
        }

        private async Task ObtenerPermisosMenu(string usuarioDnp)
        {
            using (var client = new HttpClient())
            {
                string url = "api/Autorizacion/ObtenerPermisosMenu";
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAutorizacion"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                var autorizacion = Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($"{usuarioDnp}:{User.Identity.GetHashCode()}")
                );

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic", autorizacion
                    );

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();
                    ViewBag.Menus = JsonConvert.DeserializeObject<List<string>>(resultado);
                }
                else
                {
                    throw new Exception("No pudimos procesar su solicitud");
                }
            }
        }

        /// <summary>
        /// Obtener token
        /// </summary>
        /// <returns>Returns <see cref="ActionResult"/></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<string> GetToken()
        {
            if (User.Identity.Name != null)
            {
                await Task.Run(() => base.ObtieneUsuario());
                return Convert.ToBase64String(
                   Encoding.ASCII.GetBytes($"{base.UsuarioLogueado.IdUsuarioDNP}:{User.Identity.GetHashCode()}"));
            }
            return null;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public string GetTokenChangePassword()
        {
            var appOid = new List<String>();
            foreach (Claim claim in ClaimsPrincipal.Current.FindAll("http://schemas.microsoft.com/identity/claims/objectidentifier"))
                appOid.Add(claim.Value);

            var token = "UserName:" + User.Identity.Name;
            token += ";Oid:" + appOid[0];
            token += ";RedirectUri:" + ConfigurationManager.AppSettings["ida:RedirectUri"];


            return CifradoTokens.EncryptStringWithTimeBasedSaltUrl(token);
        }

        public async Task<List<RolDto>> ObtenerRolesPorUsuario(string usuarioDNP) => await base.ObtenerRoles(usuarioDNP);

    }

}