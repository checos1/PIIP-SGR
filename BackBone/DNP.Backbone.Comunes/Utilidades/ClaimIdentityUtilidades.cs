using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Utilidades
{
    public static class ClaimIdentityUtilidades
    {
        public static string ObtenerIdUsuarioDNP(IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(x => x.Type == "IdUsuarioDNP");

            return claim.Value;
        }

        public static IEnumerable<string> ObtenerMenus(IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            var claims = identity.FindAll(x => x.Type == "PermissoMenu");

            return claims.Select(x => x.Value).ToArray();
        }

        public static Dictionary<string, List<string>> ObtenerDictionaryEntidadesOpciones(IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            var claims = identity.FindAll(x => x.Type.StartsWith("PermissoEntidad="));

            var dict = new Dictionary<string, List<string>>();
            foreach (var item in claims)
            {
                var entidad = item.Type.Replace("PermissoEntidad=", string.Empty);
                var opcion = item.Value;

                if (dict.ContainsKey(entidad))
                {
                    var dictItem = dict[entidad];
                    dictItem.Add(opcion);
                } 
                else
                {
                    var dictItem = new List<string>();
                    dictItem.Add(opcion);

                    dict.Add(entidad, dictItem);
                }
            }

            return dict;
        }
    }
}
