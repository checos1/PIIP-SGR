using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    // inicio: clase
    public class AuditoriaEntidadDto
    {
        /// <summary>
        ///  Inicializa una nueva instancia de la clase con valores predeterminados
        /// </summary>
        public AuditoriaEntidadDto()
        {
            this.identificador = 0;
            this.fechaMovimiento = DateTime.Now;
            this.entidadOrigenId = 0;
            this.entidadOrigen = String.Empty;
            this.entidadDestinoId = 0;
            this.entidadDestino = String.Empty;
            this.usuarioId = Guid.NewGuid();
            this.usuario = String.Empty;
            this.proyecto = new ProyectoEntidadDto();
        }

        #region Propiedades
        /// <summary>
        ///  Obtiene o establece el identificador único del objeto actual
        /// </summary>
        public int Identificador
        {
            get { return this.identificador; }
            set
            {
                if (this.identificador == value) return;
                this.identificador = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece la fecha del cambio de entidad del proyecto actual
        /// </summary>
        public DateTime FechaMovimiento
        {
            get { return this.fechaMovimiento; }
        }

        /// <summary>
        ///  Obtiene o establece el identificador de la entidad actual en el proyecto
        /// </summary>
        public int EntidadOrigenId
        {
            get
            {
                return this.entidadOrigenId;
            }
            set
            {
                if (this.entidadOrigenId == value) return;
                this.entidadOrigenId = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el nombre de la entidad origen
        /// </summary>
        public String EntidadOrigen
        {
            get { return this.entidadOrigen; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("La propiedad EntidadOrigen no puede ser nula o vacía");

                if (this.entidadOrigen.Equals(value)) return;
                this.entidadOrigen = value.ToString();
            }
        }

        /// <summary>
        ///  Obtiene o establece el identificador de la nueva entidad asociada al proyecto actual
        /// </summary>
        public int EntidadDestinoId
        {
            get { return this.entidadDestinoId; }
            set
            {
                if (this.entidadDestinoId == value) return;
                this.entidadDestinoId = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece la entidad destino del proyecto actual
        /// </summary>
        public String EntidadDestino
        {
            get { return this.entidadDestino; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("La propiedad EntidadDestino no puede ser nula o vacía");

                if (this.entidadDestino.Equals(value)) return;
                this.entidadDestino = value.ToString();
            }
        }

        /// <summary>
        ///  Obtiene o establece el identificador único GUID del usuario que realiza el cambio
        /// </summary>
        public Guid UsuarioId
        {
            get { return this.usuarioId; }
            set
            {
                if (this.usuarioId == value) return;
                this.usuarioId = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el nombre del usuario que realiza el cambio
        /// </summary>
        public String Usuario
        {
            get { return this.usuario; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("La propiedad Usuario no peude ser nula o vacía");

                if (this.usuario.Equals(value)) return;
                this.usuario = value.ToString();
            }
        }

        /// <summary>
        ///  Obtiene o establece el proyecto actual
        /// </summary>
        public ProyectoEntidadDto Proyecto
        {
            get { return this.proyecto; }
            set
            {
                if (this.proyecto == value) return;
                this.proyecto = value;
            }
        }

        /// <summary>
        ///  Obtiene el identificador del proyecto actual
        /// </summary>
        public int ProyectoId
        {
            get { return this.proyecto.ProyectoId; }
        }

        /// <summary>
        ///  Obtiene el nombre del proyecto actual
        /// </summary>
        public String ProyectoNombre
        {
            get { return this.proyecto.ProyectoNombre; }
        }
        #endregion Propiedades

        #region variables
        private int identificador;
        private DateTime fechaMovimiento;
        private int entidadOrigenId;
        private String entidadOrigen;
        private int entidadDestinoId;
        private String entidadDestino;
        private Guid usuarioId;
        private String usuario;
        private ProyectoEntidadDto proyecto;
        #endregion variables
    }
    // fin: clase

    public class ProyectoEntidadDto
    {
        public int SectorId { get; set; }
        public string SectorNombre { get; set; }
        public int EntidadId { get; set; }
        public string EntidadNombre { get; set; }
        public string TipoEntidad { get; set; }
        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; }
        public string CodigoBpin { get; set; }
        public int? TipoEntidadId { get; set; }
        public string Estado { get; set; }
        public string DescripcionCR { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? EstadoId { get; set; }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public string TipoProyecto { get; set; }
        //Se adiciona estos campos para que la entidad quede igual a la de los flujos
        public bool TieneInstancia { get; set; }
        public bool TieneRecurso { get; set; }

        public int Orden { get; set; }
        public int PermiteCrearInstancia { get; set; }
        public Guid FlujoId { get; set; }
        public int EntidadProcesoId { get; set; }
        public string EntidadProcesoNombre { get; set; }
        public string NombreFlujo { get; set; }
        public string TurnoActual { get; set; }
        public string EntidadProceso { get; set; }
        public int ProcesoId { get; set; }
    }
}
