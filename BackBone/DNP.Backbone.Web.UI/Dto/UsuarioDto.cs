// ReSharper disable InconsistentNaming
namespace DNP.Backbone.Web.UI.Dto
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UsuarioDto
    {
        public string objectId { get; set; }

        public string tipoUsuario { get; set; }

        public bool accountEnabled { get; set; }

        public List<NombreInicioSesionDto> signInNames { get; set; }

        public string creationType { get; set; }

        public string displayName { get; set; }

        public string mailNickname { get; set; }

        public string passwordPolicies { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string streetAddress { get; set; }

        public string givenName { get; set; }

        public string surname { get; set; }

        public string mail { get; set; }

        public string mobile { get; set; }

        public string userPrincipalName { get; set; }

        public string extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion { get; set; }

        public string extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion { get; set; }        
        [JsonIgnore]
        public string IdUsuarioDNP { get; set; }
        //metodo usado anteriormente 
        [JsonIgnore]
        public string CacheIdUsuarioDNP { get { return (creationType == "Invitation") ? mail : (creationType == "LocalAccount") ? signInNames[0].value : userPrincipalName; } }
        [JsonIgnore]
        public bool UsuarioTipoB2C { get { return userPrincipalName == null ? false : userPrincipalName.Contains("dnpb2c"); } }

        [JsonIgnore]
        private Guid idUsuarioPIIP = new Guid();
        public Guid IdUsuarioPIIP {
            get { return this.idUsuarioPIIP; }
            set {
                if (this.idUsuarioPIIP == value) return;
                this.idUsuarioPIIP = value;
            }
        }
    }
}