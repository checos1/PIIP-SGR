namespace DNP.Backbone.Dominio.Dto.Identidad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.{
    public class UsuarioSTSDto
    {
        public string pTD { get; set; }
        public string pNumeroDocumento { get; set; }
        public string pPassword { get; set; }
        public string pCorreo { get; set; }
        
    }
}
