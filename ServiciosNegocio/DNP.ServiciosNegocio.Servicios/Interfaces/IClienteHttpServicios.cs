using DNP.ServiciosNegocio.Comunes.Enum;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces
{
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
            bool useJWTAuth = false
            , bool useBearerToken = false,
            string tokenBearerJWT = ""
        );

        Task<HttpResponseMessage> PostRequestApiMultiContent(string url, MultipartFormDataContent body, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "");
    }
}
