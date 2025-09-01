namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CausaDto
    {
        public int CausaId { get; set; }
        public string Causa { get; set; }
        public int ProblemaCentralId { get; set; }
        public int CausaPadre { get; set; }
        public int TipoDeEfectoId { get; set; }
        public List<ObjetivoEspecificoDto> ObjectivoEspecifico { get; set; }

    }
}
