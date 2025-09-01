using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Web.API.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class IdentidadAutenticacionBasicaAttribute : AutenticacionBasicaAttribute
    {
#pragma warning disable 1998
        protected override async Task<IPrincipal> JWTAuthenticateAsync(IEnumerable<Claim> claims,
            HttpRequestMessage httpRequest, CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.
            var claimsList = new List<Claim> { new Claim(ClaimTypes.Name, claims.First(x => x.Type == "IdUsuarioDNP").Value) };
            claimsList.AddRange(claims);

            var identity = new ClaimsIdentity(claimsList, DefaultAuthenticationTypes.ApplicationCookie);

            return new ClaimsPrincipal(identity);
        }

#pragma warning disable 1998
        protected override async Task<IPrincipal> DefaultAuthenticateAsync(string userName, string password,
            CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            //UserManager<IdentityUser> userManager = CreateUserManager();

            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            /*IdentityUser user =  await userManager.FindAsync(userName, password);

            if (user == null)
            {
                // No user with userName/password exists.
                return null;
            }*/

            // Create a ClaimsIdentity with all the claims for this user.
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, IClaimsIdenityFactory doesn't support CancellationTokens.
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            return new ClaimsPrincipal(identity);
        }


    }
}