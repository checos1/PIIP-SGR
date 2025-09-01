using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad
{
    public class ProyectoViabilidadInvolucradosDto
    {
        public Nullable<int> Id { get; set; }
        public int ProyectoId { get; set; }
        public int TipoConceptoViabilidadId { get; set; }
        public string TipoConceptoViabilidad { get; set; }
        public int TipoRolConceptoId { get; set; }
        public string TipoRolConcepto { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public Nullable<int> ProyectoViabilidadId { get; set; }
    }

    public class ProyectoViabilidadInvolucradosResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

    public class ProyectoViabilidadInvolucradosFirmaDto
    {
        public string IdUsuarioDNP { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Area { get; set; }
        public string Accion { get; set; }
        public DateTime? FechaFirma { get; set; }
        public bool Firmado { get; set; }
    }
}
