using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces
{
    using Comunes.Enums;
    using System.Net.Http;
    using System.Security.Principal;

    public interface IClienteHttpServicios
    {
        Task<string> ConsumirServicio(
            MetodosServiciosWeb metodoServicio,
            string endPoint,
            string uriMetodo,
            string parametros,
            object peticion,
            string usuarioDnp,
            bool readCustomHttpCodes = false,
            IPrincipal principal = null,
            bool useJWTAuth = false,
            bool useBearerToken = false,
            string tokenBearerJWT = ""
        );
        Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, object peticion, string usuarioDnp, bool readCustomHttpCodes = false, IPrincipal principal = null, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "");
        Task<HttpResponseMessage> PostRequestApiMultiContent(string url, MultipartFormDataContent body, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "");
        Task<byte[]> RequestApiByteArray(MetodosServiciosWeb metodoServicio, string url, string parametros, object peticion, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "");
    }
}
