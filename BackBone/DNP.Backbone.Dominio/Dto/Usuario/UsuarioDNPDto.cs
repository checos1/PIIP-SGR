using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    //Se excluye por ser un DTO que se comunica con el GraphAPI y otros servicios externos por lo que se sigue su estandares de nombramiento.
    public class UsuarioDNPDto
    {
        public UsuarioDNPDto() { }

        public UsuarioDNPDto(UsuarioDNPDto usuarioDNPDto)
        {
            objectId = usuarioDNPDto.objectId;
            accountEnabled = usuarioDNPDto.accountEnabled;
            signInNames = usuarioDNPDto.signInNames;
            creationType = usuarioDNPDto.creationType;
            displayName = usuarioDNPDto.displayName;
            mailNickname = usuarioDNPDto.mailNickname;
            passwordProfile = usuarioDNPDto.passwordProfile;
            passwordPolicies = usuarioDNPDto.passwordPolicies;
            city = usuarioDNPDto.city;
            country = usuarioDNPDto.country;
            facsimileTelephoneNumber = usuarioDNPDto.facsimileTelephoneNumber;
            givenName = usuarioDNPDto.givenName;
            mail = usuarioDNPDto.mail;
            mobile = usuarioDNPDto.mobile;
            otherMails = usuarioDNPDto.otherMails;
            postalCode = usuarioDNPDto.postalCode;
            preferredLanguage = usuarioDNPDto.preferredLanguage;
            state = usuarioDNPDto.state;
            streetAddress = usuarioDNPDto.streetAddress;
            surname = usuarioDNPDto.surname;
            telephoneNumber = usuarioDNPDto.telephoneNumber;
            extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion = usuarioDNPDto.extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion;
            extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion = usuarioDNPDto.extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion;
            userPrincipalName = usuarioDNPDto.userPrincipalName;
            dirSyncEnabled = usuarioDNPDto.dirSyncEnabled;
        }

        public UsuarioDNPDto
        (
            string objectId,
            bool accountEnabled,
            List<NombreInicioSesionDto> signInNames,
            string creationType,
            string displayName,
            string mailNickname,
            PerfilClaveDto passwordProfile,
            string passwordPolicies,
            string city,
            string country,
            string facsimileTelephoneNumber,
            string givenName,
            string mail,
            string mobile,
            List<string> otherMails,
            string postalCode,
            string preferredLanguage,
            string state,
            string streetAddress,
            string surname,
            string telephoneNumber,
            string extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion,
            string extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion,
            string userPrincipalName,
            string dirSyncEnabled
        )
        {
            this.objectId = objectId;
            this.accountEnabled = accountEnabled;
            this.signInNames = signInNames;
            this.creationType = creationType;
            this.displayName = displayName;
            this.mailNickname = mailNickname;
            this.passwordProfile = passwordProfile;
            this.passwordPolicies = passwordPolicies;
            this.city = city;
            this.country = country;
            this.facsimileTelephoneNumber = facsimileTelephoneNumber;
            this.givenName = givenName;
            this.mail = mail;
            this.mobile = mobile;
            this.otherMails = otherMails;
            this.postalCode = postalCode;
            this.preferredLanguage = preferredLanguage;
            this.state = state;
            this.streetAddress = streetAddress;
            this.surname = surname;
            this.telephoneNumber = telephoneNumber;
            this.extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion = extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion;
            this.extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion = extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion;
            this.userPrincipalName = userPrincipalName;
            this.dirSyncEnabled = dirSyncEnabled;
        }

        public string objectId { get; set; }
        public bool accountEnabled { get; set; }
        public List<NombreInicioSesionDto> signInNames { get; set; }
        public string creationType { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public PerfilClaveDto passwordProfile { get; set; }
        public string passwordPolicies { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }
        public List<string> otherMails { get; set; }
        public string postalCode { get; set; }
        public string preferredLanguage { get; set; }
        public string state { get; set; }
        public string streetAddress { get; set; }
        public string surname { get; set; }
        public string telephoneNumber { get; set; }
        public string extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion { get; set; }
        public string extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion { get; set; }
        public string userPrincipalName { get; set; }
        public string dirSyncEnabled { get; set; }
    }
}
