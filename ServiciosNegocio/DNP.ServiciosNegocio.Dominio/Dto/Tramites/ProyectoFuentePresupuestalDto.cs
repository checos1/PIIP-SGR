using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class ProyectoFuentePresupuestalDto
    {
        public int Id { get; set; }
        public int TramiteProyectoId { get; set; }
        public string Accion { get; set; }
        public int FuenteId { get; set; }


        public IEnumerable<FuentePresupuestalDto> ListaFuentes { get; set; }

       

       
    }
}
