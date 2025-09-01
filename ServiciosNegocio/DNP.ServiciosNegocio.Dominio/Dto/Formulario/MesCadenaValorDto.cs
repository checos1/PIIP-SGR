namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MesCadenaValorDto
    {
        public int? Mes { get; set; }
        public string NombreMes { get; set; }
        public List<GrupoRecursoDto> GrupoRecurso { get; set; }
    }
}
