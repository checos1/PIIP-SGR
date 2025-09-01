using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.KeyVault
{
    public interface IKeyVaultManager
    {
        Task<string> GetSecret(string secretName);
    }
}
