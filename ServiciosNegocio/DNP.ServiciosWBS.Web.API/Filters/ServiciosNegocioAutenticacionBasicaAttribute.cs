namespace DNP.ServiciosWBS.Web.API.Filters
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class ServiciosNegocioAutenticacionBasicaAttribute : AutenticacionBasicaAttribute
    {
#pragma warning disable 1998
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            // Create a ClaimsIdentity with all the claims for this user.
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, IClaimsIdenityFactory doesn't support CancellationTokens.
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            return new ClaimsPrincipal(identity);
        }
    }
}