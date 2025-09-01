
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ProyectoFuentePresupuestalDto
    {
        public int Id { get; set; }
        public int TramiteProyectoId { get; set; }
        public string Accion { get; set; }

        public IEnumerable<FuentePresupuestalDto> ListaFuentes { get; set; }

       

       
    }
}
