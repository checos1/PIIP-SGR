namespace DNP.Backbone.Web.UI.Servicios
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Principal;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Controllers;

    public class AutorizacionServicio : IAutorizacionServicio
    {
        private readonly HomeController _homeController;

        public AutorizacionServicio(HomeController homeController)
        {
            _homeController = homeController;
        }

        public object Recursos { get; private set; }

        public string GenerarTokenAutorizacion(IPrincipal user)
        {
            using (var client = new HttpClient())
            {
                var autorizacion = Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", _homeController.ViewBag.UsuarioDNP, user.Identity.GetHashCode())));
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic", autorizacion
                    );
                return autorizacion;
            }
        }
    }
}