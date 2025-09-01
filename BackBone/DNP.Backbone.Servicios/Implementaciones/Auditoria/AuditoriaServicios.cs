namespace DNP.Backbone.Servicios.Implementaciones.Auditoria
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Comunes.Enums;
    using Interfaces.Auditoria;
    using Newtonsoft.Json;

    public class AuditoriaServicios: IAuditoriaServicios
    {
        public void RegistrarErrorAuditoria(Exception exception, string ip, string usuario)
        {
            var evento = Enum.GetName(typeof(TipoEvento), TipoEvento.Sistema);
            var mensaje = Enum.GetName(typeof(TipoMensaje), TipoMensaje.ERROR);
            var auditoria = new RegistroAuditoriaDto
            {
                Ip = "::1".Equals(ip) ? "127.0.0.1" : ip,
                Aplicacion = ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                Usuario = usuario,
                TipoEvento = evento?.ToUpper(),
                TipoMensaje = mensaje?.ToUpper(),
                ContenidoMensaje = @"{ ""mensaje"": ""[[contenidoMensaje]]"" }",
                EntidadOrigen = null
            };
            auditoria.ContenidoMensaje = auditoria.ContenidoMensaje.Replace("[[contenidoMensaje]]", exception.Message + " " + exception.StackTrace);
            auditoria.ContenidoMensaje = auditoria.ContenidoMensaje.Replace("\\", "/");
            Task.Run(() => EnviarAditoria(auditoria));
        }

        private static bool EnviarAditoria(RegistroAuditoriaDto auditoria)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAuditoria"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsondata = JsonConvert.SerializeObject(auditoria);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");

                var response = client.PostAsync("api/Auditoria/AgregarDatosAuditoria", content).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
