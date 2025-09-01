namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoEntidadDto
    {
        public int SectorId { get; set; }
        public string SectorNombre { get; set; }
        public int EntidadId { get; set; }
        public string EntidadNombre { get; set; }
        public string TipoEntidad { get; set; }
        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; }
        public string CodigoBpin { get; set; }
        public int? TipoEntidadId { get; set; }
        public string Estado { get; set; }
        public string DescripcionCR { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? EstadoId { get; set; }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public string TipoProyecto { get; set; }
        public bool TieneInstancia { get; set; }
        public bool TieneRecurso { get; set; }
        public int? CRTypeId { get; set; }
        public int? ResourceGroupId { get; set; }

        public int? ProyectoSuifId { get; set; }

        public int Orden { get; set; }
        public int PermiteCrearInstancia { get; set; }
        public Guid FlujoId { get; set; }
        public int EntidadProcesoId { get; set; }
        public string EntidadProcesoNombre { get; set; }
        public string NombreFlujo { get; set; }
        public string TurnoActual { get; set; }
        public string EntidadProceso { get; set; }
        public int ProcesoId { get; set; }
    }
}
