using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public partial class CartaConcepto
    {
        public int Id { get; set; }
        public int FaseId { get; set; }
        public int TramiteId { get; set; }
        public string RadicadoEntrada { get; set; }
        public string RadicadoSalida { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public string ExpedienteId { get; set; }
    }
}
