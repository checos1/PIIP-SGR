using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion
{
    public class ProyectoAprobacionInstanciasDto
    {
        public int Id { get; set; }
        public int AprobacionId { get; set; }
        public Guid InstanciaId { get; set; }
        public bool? Aprobado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaAprobacionInicial { get; set; }
        public DateTime? FechaActoAdmtvo { get; set; }
        public String CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class ProyectoAprobacionInstanciasResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
