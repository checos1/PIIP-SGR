namespace DNP.ServiciosNegocio.Dominio.Dto.Auditoria
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ValidacionTransferenciaAuditoriaDto
    {
        public string Mensaje { get; set; }
        public bool ResultadoValidacion { get; set; }
    }
}
