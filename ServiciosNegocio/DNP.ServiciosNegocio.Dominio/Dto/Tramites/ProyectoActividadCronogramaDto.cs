namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoActividadCronogramaDto
    {       
        public int? ActividadPreContractualId { get; set; }
        public string Actividad { get; set; }
        public int? CronogramaId { get; set; }
        public int ModalidadContratacionId { get; set; }
        public int? TramiteProyectoId { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
    }       
}
