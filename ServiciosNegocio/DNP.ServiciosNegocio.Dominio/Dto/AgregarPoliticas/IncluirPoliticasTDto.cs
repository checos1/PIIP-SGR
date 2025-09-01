namespace DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class IncluirPoliticasTDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Politicas> Politicas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class Politicas
    {
        public int PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<Dimensiones> Dimensiones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class Dimensiones
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public int? FocalizacionProyectoId { get; set; }
    }
}
