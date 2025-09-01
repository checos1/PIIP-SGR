using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion
{
    public class TramiteRVFAutorizacionDto
    {
        public int? Id { get; set; }
        public string NumeroTramite { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string Descripcion { get; set; }
        public int? TramiteLiberarId { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public int? ReprogramacionId { get; set; }
    }

    public class tramiteRVFAsociarproyecto
    {
        public int? Id { get; set; }
        public string NumeroTramite { get; set; }
        public string Descripcion { get; set; }
        public int? ProyectoId { get; set; }
        public int? TramiteId { get; set; }
        public int? EntidadId { get; set; }
    }
}
