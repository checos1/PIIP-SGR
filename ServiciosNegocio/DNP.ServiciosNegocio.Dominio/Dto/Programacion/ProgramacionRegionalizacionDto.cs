using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class ProgramacionRegionalizacionDto
    {
        public int TramiteProyectoId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public List<ValoresRegionalizar> ValoresRegionalizar { get; set; }
        
    }
    public class ValoresRegionalizar
    {
        public int LocalizacionId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }  
}
