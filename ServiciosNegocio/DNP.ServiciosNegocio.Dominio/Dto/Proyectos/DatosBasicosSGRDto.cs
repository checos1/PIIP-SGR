using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Collections.Generic;
    using Formulario;
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class DatosBasicosSGRDto
    {
        public int? DatosBasicosSGRId { get; set; }
        public int ? ProyectoId { get; set; }
        public string Bpin { get; set; }
        public string NumeroPresentacion { get; set; }
        public DateTime? FechaVerificacionRequisitos { get; set; }
      
        public int? ObjetivoSGRId { get; set; }
        public string ObjetivoSGR { get; set; }
        public int? EjecutorPropuestoId { get; set; }
        public string NitEjecutorPropuesto { get; set; }
        public string EjecutorPropuesto { get; set; }

        public int? InterventorPropuestoId { get; set; }
        public string NitInterventorPropuesto { get; set; }
        public string InterventorPropuesto { get; set; }
        public int? TiempoEstimadoEjecucionFisicaFinanciera { get; set; }
        public decimal? EstimacionCostosFasesPosteriores { get; set; }

        public List<FuentesInterventoriaDto> FuentesInterventoria{ get; set; }
      
    }
}
