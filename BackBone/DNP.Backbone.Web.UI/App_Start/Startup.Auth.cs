using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.UI
{
    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public partial class Startup
    {
        public static readonly string PolicyB2BSignin = ConfigurationManager.AppSettings["b2b:PolicySignin"];
        public static readonly string PolicyMultiTenantSignin = ConfigurationManager.AppSettings["multitenant:PolicySignin"];
        public static readonly string UriBaseOpenidB2c = ConfigurationManager.AppSettings["b2c:OpenIdBaseUrl"]; 
        public static readonly string TenantB2C = ConfigurationManager.AppSettings["b2c:Tenant"];
        private static readonly string ClientIdB2C = ConfigurationManager.AppSettings["b2c:ClientId"];
        public static readonly string PolicyB2CSignin = ConfigurationManager.AppSettings["b2c:PolicySignin"];
        public static readonly string TenantB2B = ConfigurationManager.AppSettings["b2b:Tenant"];
        private static readonly string ClientIdB2B = ConfigurationManager.AppSettings["b2b:ClientId"];
        private static readonly string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static readonly string MetadataAddress = ConfigurationManager.AppSettings["ida:MetadataAddress"];
        private static readonly string MetadataAddressOpenId = ConfigurationManager.AppSettings["ida:MetadataAddressOpenId"];
        public static readonly string AadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static readonly string MultiTenantClientId = ConfigurationManager.AppSettings["ida:MultiTenantClientId"];

        /// <summary>
        /// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<Microsoft.IdentityModel.Protocols.OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            if (!string.IsNullOrEmpty(context.ProtocolMessage.State) ||
                context.ProtocolMessage.State.StartsWith("OpenIdConnect.AuthenticationProperties="))
            {
                var authenticationPropertiesString = context.ProtocolMessage.State.Split('=')[1];

                AuthenticationProperties authenticationProperties = context.Options.StateDataFormat.Unprotect(authenticationPropertiesString);
                context.Response.Redirect(authenticationProperties.RedirectUri);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            try
            {
                context.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", context.ProtocolMessage.IdToken));
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("SecurityTokenValidated: " + ex.Message);
            }
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            string signInB2BPath = string.Concat(RedirectUri, "Account/SignInB2B");
            string signInMultiPath = string.Concat(RedirectUri, "Account/SignInMulti");
            string signInB2CPath = string.Concat(RedirectUri, "Account/SignIn");
            var urlAutorizacion = $"{CodificaBase64(TenantB2C)}/{CodificaBase64(ClientIdB2C)}/{CodificaBase64(PolicyB2CSignin)}/{CodificaBase64(signInB2BPath)}/{CodificaBase64(RedirectUri)}/{CodificaBase64(signInMultiPath)}";

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "PIIPAuthCookie"
            });

            //B2B
            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = PolicyB2BSignin,
                ClientId = ClientIdB2B,
                Authority = $"{AadInstance}{TenantB2B}",
                RedirectUri = RedirectUri,
                PostLogoutRedirectUri = RedirectUri,                
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    RedirectToIdentityProvider = (context) =>
                    {
                        var login_hint = context?.OwinContext?.Authentication?.AuthenticationResponseChallenge?.Properties?.Dictionary["login_hint"];
                        if (login_hint != null)
                            context.ProtocolMessage.LoginHint = login_hint;

                        return Task.CompletedTask;
                    },
                    SecurityTokenValidated = OnSecurityTokenValidated
                },
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    SaveSigninToken = true,
                    NameClaimType = "name",
                    RoleClaimType = "roles",
                    RequireSignedTokens = false
                }
            });

            //MULTI-TENANT
            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = PolicyMultiTenantSignin,
                ClientId = MultiTenantClientId,
                Authority = $"{AadInstance}common",
                RedirectUri = RedirectUri,
                PostLogoutRedirectUri = RedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    RedirectToIdentityProvider = (context) =>
                    {
                        var login_hint = context?.OwinContext?.Authentication?.AuthenticationResponseChallenge?.Properties?.Dictionary["login_hint"];
                        if (login_hint != null)
                            context.ProtocolMessage.LoginHint = login_hint;

                        return Task.CompletedTask;
                    },
                    SecurityTokenValidated = OnSecurityTokenValidated
                },
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    SaveSigninToken = true,
                    NameClaimType = "name",
                    RoleClaimType = "roles",
                    RequireSignedTokens = false
                }
            });

            //B2C
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = PolicyB2CSignin,
                MetadataAddress = $"{MetadataAddress}{MetadataAddressOpenId}{urlAutorizacion}",                
                ClientId = ClientIdB2C,
                RedirectUri = RedirectUri,
                PostLogoutRedirectUri = RedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    RedirectToIdentityProvider = (context) =>
                    {
                        var login_hint = context?.OwinContext?.Authentication?.AuthenticationResponseChallenge?.Properties?.Dictionary["login_hint"];
                        if (login_hint != null)
                            context.ProtocolMessage.LoginHint = login_hint;

                        return Task.CompletedTask;
                    },
                    SecurityTokenValidated = OnSecurityTokenValidated
                },
                Scope = "openid",
                ResponseType = "id_token",
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    SaveSigninToken = true,
                    NameClaimType = "name",
                    RoleClaimType = "roles",
                    RequireSignedTokens = false
                },
                ProtocolValidator = new OpenIdConnectProtocolValidator()
                {
                    RequireNonce = false,
                },
            });
        }

        public static string CodificaBase64(string cadena)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(cadena);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
