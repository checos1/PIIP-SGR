namespace DNP.ServiciosNegocio.Comunes.Autorizacion
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IAutorizacionUtilidades
    {
        Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion,
                                                 string nombreServicio);
    }
}
