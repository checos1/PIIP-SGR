namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.Identidad;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;

    public class IdentidadServiciosMock : IIdentidadServicios
    {
    
        public Task<bool> InvitarUsuario(InvitarUsuarioDto dto, string usuarioDnp, bool esUsuarioBackbone)
        {
            return Task.FromResult(true);
        }

        public Task<InvitarUsuarioDto> ObtenerUsuarioDominio(string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        Task<bool> IIdentidadServicios.CambiarClaveUsuario(CredencialUsuarioDto request, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CambiarContrasenaSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {

            if (string.IsNullOrEmpty(usuarioSTS.pNumeroDocumento))
                return await Task.Run(() => false);
            else
                return true;
        }

        public async Task<string> EnviarCorreoInvitacionSTS(NotificarInvitacionUsuarioSTSDto notificacion, string idUsuarioDNP)
        {
            if (string.IsNullOrEmpty(notificacion.pNumeroDocumento))
                return await Task.Run(() => string.Empty);
            else
                return "Exito";
        }

        public async Task<bool> RegistrarUsuarioAPPSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            if (string.IsNullOrEmpty(usuarioSTS.pNumeroDocumento))
                return await Task.Run(() => false);
            else
                return true;
        }

        public async Task<bool> RegistrarUsuarioSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            if (string.IsNullOrEmpty(usuarioSTS.pNumeroDocumento))
                return await Task.Run(() => false);
            else
                return true;

        }

        public async Task<bool> ValidarContrasenaActualSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            if (string.IsNullOrEmpty(usuarioSTS.pNumeroDocumento))
                return await Task.Run(() => false);
            else
                return true;
        }

		public Task<bool> apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(string pAplicacion, string pTD, string pNumeroDocumento,string usuarioDnp)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> apiObtenerAplicacionesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string usuarioDnp)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> apiObtenerAplicacionesConfiablesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string usuarioDnp)
		{
			throw new System.NotImplementedException(); 
		}

		public Task<UsuarioDNPDto> ObtenerUsuarioPorId(string id, string idUsuarioDNP)
		{
			throw new System.NotImplementedException();
		}
	}
}
