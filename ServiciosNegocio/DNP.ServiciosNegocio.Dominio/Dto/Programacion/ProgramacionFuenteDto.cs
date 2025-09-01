using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente
{
    public class ProgramacionFuenteDto
    {
        public int TramiteProyectoId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public List<ValoresFuente> ValoresFuente { get; set; }
        public List<ValoresCredito> ValoresCredito { get; set; }
    }

    public class ValoresFuente
    {
        public int FuenteId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }

    public class ValoresCredito
    {
        public int CreditoId { get; set; }
        public int FuenteId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }
}
