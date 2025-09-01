using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.{
    public class InvitarUsuarioB2BRequestDto
    {
        public string InvitedUserDisplayName { get; set; }
        
        public string InvitedUserEmailAddress { get; set; }
        
        public string Surname { get; set; }
        
        public string GivenName { get; set; }
        
        public bool EsUsuarioBackbone { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }

        public string IdUsuarioDNP { get; set; }

    }
}
