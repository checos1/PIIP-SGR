
namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class DuplaDto
    {
        public string Id { get; set; }

        public Guid IdRol
        {
            set { Id = value.ToString(); }
        }

        public string Valor { get; set; }

        public string Nombre { get { return Valor; } }
    }
}
