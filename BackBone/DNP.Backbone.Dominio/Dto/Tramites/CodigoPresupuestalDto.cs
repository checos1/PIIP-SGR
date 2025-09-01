using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class CodigoPresupuestalDto
    {
        public int Id { get; set; }
        public int EntidadId { get; set; }
        public int ProyectoId { get; set; }
        public int TramiteId { get; set; }
        public string CodigoPresupuestal { get; set; }
        public int FuenteId { get; set; }
        public string CodigoEntidad { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public int Consecutivo { get; set; }
    }
}
