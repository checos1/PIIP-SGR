namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProblemaCentralDto
    {
        public int ProblemaCentralId { get; set; }
        public int ProyectoId { get; set; }
        public string ProblemaCentral { get; set; }
        public string Situacion { get; set; }
        public string Magnitud { get; set; }
        public List<CausaDto> Causa { get; set; }
    }
}
