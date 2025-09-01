namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramiteDeflactoresDto
    {       
        public int Id { get; set; }
        public int AnioBase { get; set; }
        public int AnioConstante { get; set; }
        public double Valor { get; set; }
        public string IPC { get; set; }
    }       
}
