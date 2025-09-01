using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProgramacionIniciativaDto
    {
        public int? TramiteProyectoId { get; set; }
        public int? SeccionCapitulo { get; set; }
        public List<Iniciativa> Iniciativa { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Iniciativa
    {
        public int Id { get; set; }
        public int IniciativaId { get; set; }
    }
}
