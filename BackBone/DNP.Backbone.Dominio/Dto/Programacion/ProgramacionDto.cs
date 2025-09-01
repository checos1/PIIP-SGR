using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class ProgramacionDto
    {
        public int IdProgramacion { get; set; }

        public Guid FlujoId { get; set; }

        public string TipoEntidad { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool? Creado { get; set; }

        public bool? Cerrado { get; set; }

        public bool? IniciarProceso { get; set; }

        public int? IdNotificacion { get; set; }

        public virtual FlujosProgramacionDto FlujoTramite { get; set; }
        
        /// <summary>
        ///  Obtiene o establece el estatus del proceso actual
        /// </summary>
        public Enums.EstadoProceso EstadoProceso {
            get {

                if (this.Creado != null && this.Cerrado != null)
                    return ((bool)this.Creado && (bool)!this.Cerrado) ? Enums.EstadoProceso.Ejecutado : ((bool)this.Cerrado ? Enums.EstadoProceso.Terminado : Enums.EstadoProceso.EnEspera);
                else
                    return Enums.EstadoProceso.EnEspera;
            }
        }
    }
}
