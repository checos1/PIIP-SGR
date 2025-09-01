using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Collections.Generic;
    using Formulario;
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FuentesInterventoriaDto
    {
        public int? ProgramacionFuenteId { get; set; }
        public string Vigencia { get; set; }
        public string GrupoRecurso { get; set; }
        public int? TipoEntidadId { get; set; }
        public string TipoEntidad { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public decimal? Solicitado { get; set; }
        public string ValorAprobadoBienio1 { get; set; }
        public string ValorAprobadoBienio2 { get; set; }
        public string ValorAprobadoBienio3 { get; set; }
        public string ValorAprobadoBienio4 { get; set; }
      
    }
}
