using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class IncluirPoliticasDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Politicas> Politicas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Politicas
    {
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public int? Dimension1Id { get; set; }
        public string Dimension1 { get; set; }
        public int? Dimension2Id { get; set; }
        public string Dimension2 { get; set; }
    }
}
