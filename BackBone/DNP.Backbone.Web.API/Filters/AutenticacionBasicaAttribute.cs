using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Web.API.Filters
{
    using DNP.Backbone.Comunes.Utilidades;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using DNP.Backbone.Servicios.Interfaces.Identidad;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public abstract class AutenticacionBasicaAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;
        public string Realm { get; set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                context.ErrorResult = new AuthenticationFailureResult("Faltan las credenciales", request);
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Faltan las credenciales", request);
                return;
            }

            //Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            //if (userNameAndPasword == null)
            //{
            //    // Authentication was attempted but failed. Set ErrorResult to indicate an error.
            //    context.ErrorResult = new AuthenticationFailureResult("Credenciales invalidas", request);
            //    return;
            //}

            //string userName = userNameAndPasword.Item1;
            //string password = userNameAndPasword.Item2;

            var authorizationType = request.Headers.Contains("Authorization-Type") ?
                request.Headers.GetValues("Authorization-Type").First() : "default";

            IPrincipal principal = null;

            switch (authorizationType)
            {
                case "JWT":
                    principal = await JWTAuthenticationMethodAsync(context, cancellationToken);
                    break;
                default:
                    principal = await DefaultAuthenticationMethodAsync(context, cancellationToken);
                    break;
            }

            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        protected abstract Task<IPrincipal> JWTAuthenticateAsync(IEnumerable<Claim> claims,
           HttpRequestMessage httpRequest, CancellationToken cancellationToken);

        protected abstract Task<IPrincipal> DefaultAuthenticateAsync(string userName, string password,
            CancellationToken cancellationToken);

        private static IEnumerable<Claim> ExtractClaimsFromJwtToken(string authorizationParameter)
        {
            var tokenManager = new JwtTokenManager(ConfigurationManager.AppSettings["JwtSecret-Usuario"]);
            var claims = tokenManager.ValidarToken(authorizationParameter);

            return claims;
        }

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.LastIndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }



            string userName = decodedCredentials.Substring(0, colonIndex);
            //userName = userName.Substring(userName.LastIndexOf("#", StringComparison.Ordinal) + 1);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            /*string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }
            */
            //context.ChallengeWith("Basic", parameter);
        }

        private async Task<IPrincipal> JWTAuthenticationMethodAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            var tokenManager = new JwtTokenManager(ConfigurationManager.AppSettings["JwtSecret-Usuario"]);
            var claims = tokenManager.ValidarToken(authorization.Parameter);

            return await JWTAuthenticateAsync(claims, request, cancellationToken);
        }

        private async Task<IPrincipal> DefaultAuthenticationMethodAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            if (userNameAndPasword == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return null;
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            //var cacheService = HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pPassword = password;
            usuarioSTS.pNumeroDocumento = userName;
            usuarioSTS.pTipoDocumento = "CC";

            var identidadServicios = (IIdentidadServicios)context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IIdentidadServicios));
            var ls = identidadServicios.ValidarContrasenaActualSTS(usuarioSTS, "dnp");

            if (ls.Result == false)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return null;
            }


            return await DefaultAuthenticateAsync(userName, password, cancellationToken);
        }
    }
}