using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Usuario;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Identidad
{
    public interface IIdentidadServicios
    {
        Task<bool> InvitarUsuario(InvitarUsuarioDto dto, string usuarioDnp, bool esUsuarioBackbone);
        Task<bool> CambiarClaveUsuario(CredencialUsuarioDto request, string usuarioDnp);
        Task<InvitarUsuarioDto> ObtenerUsuarioDominio(string usuarioDnp);
        Task<bool> CambiarContrasenaSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP);
        Task<string> EnviarCorreoInvitacionSTS(NotificarInvitacionUsuarioSTSDto notificacion, string idUsuarioDNP);
        Task<bool> RegistrarUsuarioAPPSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP);
        Task<bool> RegistrarUsuarioSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP);
        Task<bool> ValidarContrasenaActualSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP);
        Task<bool> apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP);
        Task<bool> apiObtenerAplicacionesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP);
        Task<bool> apiObtenerAplicacionesConfiablesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP);
        Task<UsuarioDNPDto> ObtenerUsuarioPorId(string id, string idUsuarioDNP);
    }
}