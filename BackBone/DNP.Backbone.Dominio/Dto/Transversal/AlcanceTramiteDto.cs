using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    public class AlcanceTramiteDto
    {

        public int TramiteId { get; set; }

        public Guid IdInstancia { get; set; }

        public List<int> TipoMotivo { get; set; }

        public string Descripcion { get; set; }
        public string Motivo { get; set; }
    }
    public class AlcanceTramiteMGADto
    {
        public int TramiteId { get; set; }
        public int NuevoTramiteId { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid NuevaInstanciaId { get; set; }
        public Guid? FlujoId { get; set; }
        public string Usuario { get; set; }
        public string NumeroTramite { get; set; }
    }
}
