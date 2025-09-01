using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.GestionRecursos
{

    public class ProyectoAprobacionResumenDto
    {
        // (Opcional) Si quieres cargar el Id de la fuente para cada registro
        public int FuenteBienioValoresId { get; set; }

        public string Etapa { get; set; }
        public string TipoEntidad { get; set; }
        public string Entidad { get; set; }
        
        

        public string TipoRecurso { get; set; }
        public string Bienio { get; set; }
        public decimal ValorSolicitado { get; set; }

        // Estado: “Aprobado” / “No Aprobado” / “Pendiente”
        public string EstadoAprobacion { get; set; }

        // Fecha en formato ISO 8601, o cadena vacía
        public string FechaAprobacion { get; set; }

        // Ahora se devuelve con: null => "", 0 => "No", 1 => "Si"
        public string VigenciaFutura { get; set; }

        // ValorAprobado o cadena vacía
        public string ValorAprobado { get; set; }

        // Crédito aprobado o cadena vacía
        public string Credito { get; set; }

        // Nuevos campos que agregamos en #FuentesExtendida
        public string EntidadApruebaNombre { get; set; }
        public string EntidadApruebaTipo { get; set; }
        public decimal ValorAprobadoCredito { get; set; }
    }



}
