using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    [ExcludeFromCodeCoverage]
    public class EncabezadoSGRDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoBPIN { get; set; }
        public string EntidadPresenta { get; set; }
        public string EntidadDestino { get; set; }
        public string Estado { get; set; }
        public string Fase { get; set; }
        public int? VigenciaInicial { get; set; }
        public int? VigenciaFinal { get; set; }
        public decimal Valor { get; set; }
        public int AnioEstudio { get; set; }
        public string Sector { get; set; }
        public string TipoEntidadPresenta { get; set; }
        public string ProyectoTipo { get; set; }
        public string ObjetivoGeneral { get; set; }
        public int PoblacionObjetivo { get; set; }
        public string Alcance { get; set; }
        public decimal ValorTotalSGR { get; set; }
        public int AjustesRealizados { get; set; }
        public int PuntajeSEP { get; set; }
        public decimal ValorTotalInversion { get; set; }
        public decimal ValorTotalPreInversion { get; set; }
        public decimal ValorTotalOperacion { get; set; }
        public DateTime FechaPresentacion { get; set; }
        public string ProgramaPresupuestal { get; set; }
        public string SubProgramaPresupuestal { get; set; }
        public int TipoTramiteId { get; set; }
        public int EntidadAdscritaId { get; set; }
        public int TipoProyectoCTUSId { get; set; }
        public string CofinanciadoPGN { get; set; }
        public string DelegoViabilidad { get; set; }
        public Nullable<System.Guid> IdInstanciaViabiliad { get; set; }
        public bool ProyectoTieneOPC { get; set; }
        public bool TieneOPC { get; set; }
        public decimal PorcentajeCredito { get; set; }
        public bool ProyectoTieneInstanciasNoOPC { get; set; }
    }
}
