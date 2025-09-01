namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CuerpoConceptoAutorizacion
    {
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string Recurso { get; set; }
        public int ProgramaId { get; set; }
        public string ProgramaCodigo { get; set; }
        public string Programa { get; set; }
        public int SubprogramaId { get; set; }
        public string SubProgramaCodigo { get; set; }
        public string SubPrograma { get; set; }
        public string TipoProyecto { get; set; }
        public string Valor { get; set; }
        public string CodigoPresupuestal { get; set; }
        public DateTime FechaRadicacion { get; set; }
        public string NumeroRadicacion { get; set; }
    }
}
