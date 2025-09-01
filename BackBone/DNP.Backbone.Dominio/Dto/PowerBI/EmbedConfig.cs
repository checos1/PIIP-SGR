namespace DNP.Backbone.Dominio.Dto.PowerBI
{
    using Microsoft.PowerBI.Api.Models;
    using System;


    public class EmbedConfig
    {
        public string Id { get; set; }

        public string EmbedUrl { get; set; }

        public EmbedToken EmbedToken { get; set; }

        public int MinutesToExpiration
        {
            get
            {
                var minutesToExpiration = EmbedToken != null ? EmbedToken.Expiration - DateTime.UtcNow : new System.TimeSpan();
                return (int)minutesToExpiration.TotalMinutes;
            }
        }

        public bool? IsEffectiveIdentityRolesRequired { get; set; }

        public bool? IsEffectiveIdentityRequired { get; set; }

        public bool EnableRLS { get; set; }

        public string Username { get; set; }

        public string Roles { get; set; }

        public string ApiUrl { get; set; }

        public Guid WorkspaceId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
