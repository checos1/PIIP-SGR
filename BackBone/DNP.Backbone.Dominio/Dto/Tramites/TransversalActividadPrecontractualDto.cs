namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TransversalActividadesPrecontractualesDto
    {       
        public int Id { get; set; }
        public int ModalidadContratacion { get; set; }
        public string Nombre { get; set; }
    }       
}
