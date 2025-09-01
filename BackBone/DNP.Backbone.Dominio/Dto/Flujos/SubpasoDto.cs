using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    public class SubpasoDto
    {
        public DateTime? FechaRegistro { get; set; }
        public Guid? AccionId { get; set; }
        // Se cambia de Guid? a int? porque en la base de datos es un entero.
        public int? EstadoAccionPorInstanciaId { get; set; }
        public Guid? AccionFlujoSubPasoId { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacionDetalle { get; set; }
        public string Flujo { get; set; }
        public string Proceso { get; set; }
        public string Observacion { get; set; }
        public int? SubPasoId { get; set; }
        public string SubPaso { get; set; }
        public string Nombre { get; set; }
        public string NombreRol { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta { get; set; }
    }
}
