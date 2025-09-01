namespace DNP.Backbone.Servicios.Implementaciones.PowerBI
{
    using DNP.Backbone.Comunes;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.PowerBI;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Threading.Tasks;

    public class EmbedConfigServicios
    {
        public static NameValueCollection sectionConfig = ConfigurationManager.GetSection("PowerBI") as NameValueCollection;
        public static string AuthorityUrl = sectionConfig == null ? string.Empty : sectionConfig["authorityUrl"];
        public static string AuthenticationType = sectionConfig == null ? string.Empty : sectionConfig["AuthenticationType"];
        public static string ResourceUrl = sectionConfig == null ? string.Empty : sectionConfig["resourceUrl"];
        public static string ApplicationId = sectionConfig == null ? string.Empty : sectionConfig["applicationId"];
        public static string ApiUrl = sectionConfig == null ? string.Empty : sectionConfig["apiUrl"];
        public static string ReportsWorkspaceId = sectionConfig == null ? string.Empty : sectionConfig["reportesWorkspaceId"];
        public static string DashboardsWorkspaceId = sectionConfig == null ? string.Empty : sectionConfig["dashboardsWorkspaceId"];
        public static string ApplicationSecret = sectionConfig == null ? string.Empty : sectionConfig["applicationSecret"];
        public static string Tenant = sectionConfig == null ? string.Empty : sectionConfig["tenant"];
        public static string Username = sectionConfig == null ? string.Empty : sectionConfig["pbiUsername"];
        public static string Password = sectionConfig == null ? string.Empty : sectionConfig["pbiPassword"];
        //public static Guid Groups = sectionConfig == null ? Guid.NewGuid() : Guid.TryParse(sectionConfig["groups"], out Guid guidOutput) ? Guid.Parse(sectionConfig["groups"]) : Guid.NewGuid();

        public EmbedConfig m_embedConfig;
        public TokenCredentials m_tokenCredentials;

        public EmbedConfigServicios()
        {
            m_embedConfig = new EmbedConfig();
            m_embedConfig.ApiUrl = ApiUrl;
            //m_embedConfig.WorkspaceId = Guid.TryParse(WorkspaceId, out Guid guidOutput) ? Guid.Parse(WorkspaceId) : Guid.NewGuid();
        }


        /// <summary>
        /// Compruebe si los parámetros de inserción de web.config tienen valores válidos.
        /// </summary>
        /// <returns>Nulo si los parámetros web.config son válidos; de lo contrario, devuelve una cadena de error específica.</returns>
        private string GetWebConfigErrors()
        {
            if (string.IsNullOrWhiteSpace(ApplicationId)) return BackboneRecursos.PowerBI_NoTienesApplicationId;
            if (!Guid.TryParse(ApplicationId, out var result)) return BackboneRecursos.PowerBI_ApplicationIdTienesSerGuid;
            if (string.IsNullOrWhiteSpace(ReportsWorkspaceId)) return BackboneRecursos.PowerBI_NoTienesWorkaspaceId;
            if (!Guid.TryParse(ReportsWorkspaceId, out result)) return BackboneRecursos.PowerBI_WorkspaceTienesSerGuid;
            if (string.IsNullOrWhiteSpace(DashboardsWorkspaceId)) return BackboneRecursos.PowerBI_NoTienesWorkaspaceId; //mudar mensagem
            if (!Guid.TryParse(DashboardsWorkspaceId, out result)) return BackboneRecursos.PowerBI_WorkspaceTienesSerGuid; //mudar mensagem
            if (AuthenticationType.Equals("MasterUser"))
            {
                if (string.IsNullOrWhiteSpace(Username)) return BackboneRecursos.PowerBI_NoTienesLoginUsuarioMaster;
                if (string.IsNullOrWhiteSpace(Password)) return BackboneRecursos.PowerBI_NoTienesSenaUsuarioMaster;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ApplicationSecret)) return BackboneRecursos.PowerBI_NoTienesClaveSecreta;
                if (string.IsNullOrWhiteSpace(Tenant)) return BackboneRecursos.PowerBI_NoTienesTenat;
            }
            return null;
        }

        /// <summary>
        /// Realice la autenticación del directorio activo, como usuario master o mediante la aplicación del portal azure
        /// </summary>
        /// <returns>Autenticación master o aplicación del portal azure</returns>
        public async Task<AuthenticationResult> DoAuthentication()
        {
            AuthenticationResult authenticationResult = null;
            if (AuthenticationType.Equals("MasterUser"))
            {
                var authenticationContext = new AuthenticationContext(AuthorityUrl);

                // Authentication using master user credentials
                var credential = new UserPasswordCredential(Username, Password);
                authenticationResult = authenticationContext.AcquireTokenAsync(ResourceUrl, ApplicationId, credential).Result;
            }
            else
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificURL = AuthorityUrl.Replace("common", Tenant);
                var authenticationContext = new AuthenticationContext(tenantSpecificURL);

                // Authentication using app credentials
                var credential = new ClientCredential(ApplicationId, ApplicationSecret);
                authenticationResult = await authenticationContext.AcquireTokenAsync(ResourceUrl, credential);
            }

            return authenticationResult;
        }

        /// <summary>
        /// Obtiene el token de autenticación.
        /// </summary>
        /// <returns>Devuelve si está autenticado o no</returns>
        public async Task<bool> GetTokenCredentials(TipoPowerBIEnum tipo)
        {
            var error = GetWebConfigErrors();
            if (error != null)
            {
                m_embedConfig.ErrorMessage = error;
                return false;
            }

            m_embedConfig.WorkspaceId = tipo == TipoPowerBIEnum.Reporte ? Guid.Parse(ReportsWorkspaceId) : Guid.Parse(DashboardsWorkspaceId);

            // Authenticate using created credentials
            AuthenticationResult authenticationResult = null;
            try
            {
                authenticationResult = await DoAuthentication();
            }
            catch (AggregateException exc)
            {
                m_embedConfig.ErrorMessage = exc.InnerException.Message;
                return false;
            }

            if (authenticationResult == null)
            {
                m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_ErrorDeAutenticación;
                return false;
            }

            m_tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");
            return true;
        }
    }
}
