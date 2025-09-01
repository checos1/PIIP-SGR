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
    public class TipoRequisitoDto
    {
        public int Id { get; set; }
        public string TipoRequisito { get; set; }
        public int FaseId { get; set; }
        public string Descripcion { get; set; }

        public IEnumerable<ProyectoRequisitoValoresDto> ListaValores { get; set; }
    }
}
