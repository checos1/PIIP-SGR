using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Configuration;
using System;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.KeyVault
{
    public class KeyVaultManager : IKeyVaultManager
    {
        private readonly SecretClient _secretClient;

        public KeyVaultManager()
        {
            _secretClient = new SecretClient(new Uri(ConfigurationManager.AppSettings["Azure.KeyVaultUri"]), new DefaultAzureCredential());
        }

        public async Task<string> GetSecret(string secretName)
        {
            KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);
            return keyValueSecret.Value;
        }
    }
}