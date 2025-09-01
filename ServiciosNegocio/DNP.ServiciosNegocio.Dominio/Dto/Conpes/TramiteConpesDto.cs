namespace DNP.ServiciosNegocio.Dominio.Dto.Conpes
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TramiteConpesDto
    {
        public int Id { get; set; }

        public string NumeroCONPES{ get; set; }

        public string Titulo {  get; set; }
    }
}
