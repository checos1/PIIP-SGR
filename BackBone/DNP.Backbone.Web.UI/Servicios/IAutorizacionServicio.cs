

namespace DNP.Backbone.Web.UI.Servicios
{
    using System.Security.Principal;
    public interface IAutorizacionServicio
    {
        string GenerarTokenAutorizacion(IPrincipal user);
    }
}
