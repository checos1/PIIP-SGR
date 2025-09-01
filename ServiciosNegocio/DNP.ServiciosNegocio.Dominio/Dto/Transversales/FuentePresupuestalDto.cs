using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    [ExcludeFromCodeCoverage]
    public class FuentePresupuestalDto
    {
        public FuentePresupuestalDto tmpfuente;

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; }

        public int TramiteProyectoId { get; set; }
        public string Accion { get; set; }
        public Nullable<decimal> ValorInicialCSF { get; set; }
        public Nullable<decimal> ValorInicialSSF { get; set; }
        public Nullable<decimal> ValorVigenteCSF { get; set; }
        public Nullable<decimal> ValorVigenteSSF { get; set; }
        public Nullable<decimal> ValorDisponibleCSF { get; set; }
        public Nullable<decimal> ValorDisponibleSSF { get; set; }
        public Nullable<decimal> ValorContracreditoCSF { get; set; }
        public Nullable<decimal> ValorContracreditoSSF { get; set; }

        public int idTipoValorContracreditoCSF { get; set; }
        public int idTipoValorContracreditoSSF { get; set; }

        public IEnumerable<ProyectoFuentePresupuestalValoresDto> ListaValores { get; set; }
    }
}
