namespace DNP.ServiciosNegocio.Comunes.Auditoria
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Dominio.Dto.Auditoria;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class AuditoriaUtilidades
    {
        public static bool EnviarAuditoria(AuditoriaDto auditoria)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAuditoria"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                JsonConvert.SerializeObject(auditoria);
                var response = client.PostAsJsonAsync("api/Auditoria/AgregarDatosAuditoria", auditoria).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
