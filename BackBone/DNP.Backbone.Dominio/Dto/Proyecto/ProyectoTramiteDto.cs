using DNP.Backbone.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class ProyectoTramiteDto
    {
        /// <summary>
        ///  Initializa una nueva instancia de la clase con valores predeterminados
        /// </summary>
        public ProyectoTramiteDto()
        {
            this.identificador = 0;
            this.instanciaId = Guid.NewGuid();
            this.proyectoId = 0;
            this.objetoNegocioId = String.Empty; //BPIN
            //this.tramite = new TramiteDto(); // null
            this.estado = false; // null
            this.proyectoTipo = String.Empty; // Credito e Contracredito
            this.proyectoNombre = String.Empty;
            this.fechaMovimiento = DateTime.Now;
            this.usuarioId = 0;
            this.usuario = String.Empty;
            this.flujoId = Guid.NewGuid();
            this.entidadId = 0;
        }

        #region Metodos

        /// <summary>
        ///  Devuelve la instancia como un <see cref="String"/>. Con un formato sin conversión real.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Instancia: {this.instanciaId.ToString()} - Proyecto: {this.proyectoId.ToString()}/{this.proyectoNombre}";
        }
        #endregion Metodos

        #region Propiedades

        /// <summary>
        ///  Obtiene o establece el identificador del objeto actual
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
        ///  Obtiene o establece el identificador único del objeto Instancia como un objeto de tipo <see cref="Guid"/>
        /// </summary>
        public Guid InstanciaId
        {
            get { return this.instanciaId; }
            set
            {

                if (this.instanciaId == null) throw new Exception("La propiedad 'ProyectoTramiteDto.InstanciaId' no puede ser nula");
                if (this.instanciaId == value) return;
                this.instanciaId = value;
            }

        }

        /// <summary>
        ///  Obtiene o establece el identificador del proyecto asociado
        /// </summary>
        public int ProyectoId
        {
            get { return this.proyectoId; }
            set { this.proyectoId = value; }
        }

        /// <summary>
        ///  Obtiene o establece el identificador único provisto desde PIIP del objeto actual
        /// </summary>
        public String ObjetoNegocioId
        {

            get { return this.objetoNegocioId; }
            set
            {
                if (this.objetoNegocioId == value) return;
                this.objetoNegocioId = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el tramite asociado al flujo actual
        /// </summary>
        //public TramiteDto Tramite {
        //    get { return this.tramite; }
        //    set {

        //        if (this.tramite == value) return;
        //        this.tramite = value;
        //    }
        //}

        /// <summary>
        ///  Obtiene el identificador del trámite asociado
        /// </summary>
        public int? TramiteId { get; set; }
        //{
        //    get { return this.tramite?.Id; }
        //}

        /// <summary>
        ///  Obtiene el nombre/descripción del trámite actualmente asociado
        /// </summary>
        //public String TramiteDescripcion {
        //    get { return this.tramite.NombreObjetoNegocio; }
        //}

        /// <summary>
        ///  Obtiene o establece el status del objeto actual
        /// </summary>
        public bool Estado
        {
            get { return this.estado; }
            set
            {
                if (this.estado == value) return;
                this.estado = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el tipo de proyecto asociado al flujo actual
        /// </summary>
        public String ProyectoTipo
        {
            get { return this.proyectoTipo; }
            set
            {
                if (!String.IsNullOrEmpty(value) && this.proyectoTipo.Equals(value)) return;
                this.proyectoTipo = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el nombre del proyecto asociado al flujo actual
        /// </summary>
        public String ProyectoNombre
        {
            get { return this.proyectoNombre; }
            set
            {
                if (!String.IsNullOrEmpty(value) && this.proyectoNombre.Equals(value)) return;
                this.proyectoNombre = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece la fecha de cualquier movimiento del objeto en el Modelo <see cref="Proyectos"/>
        /// </summary>
        public DateTime FechaMovimiento
        {
            get { return this.fechaMovimiento; }
            set
            {
                if (this.fechaMovimiento == value) return;
                this.fechaMovimiento = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el identificador del usuario que realiza algun cambio
        /// </summary>
        public int UsuarioId
        {
            get { return this.usuarioId; }
            set
            {
                if (this.usuarioId == value) return;
                this.usuarioId = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el nombre del usuario que realiza algun cambio
        /// </summary>
        public String Usuario
        {
            get { return this.usuario; }
            set
            {
                if (!String.IsNullOrEmpty(value) && this.usuario.Equals(value)) return;
                this.usuario = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el identificador único del objeto Flujo como un objeto de tipo <see cref="Guid"/>
        /// </summary>
        public Guid FlujoId
        {
            get { return this.flujoId; }
            set { this.flujoId = value; }

        }

        /// <summary>
        ///  Obtiene o establece el identificador del entidad asociado
        /// </summary>
        public int EntidadId
        {
            get { return this.entidadId; }
            set { this.entidadId = value; }
        }

        #endregion Propiedades

        #region Variables
        private int identificador;
        private Guid instanciaId;
        private int proyectoId;
        private String objetoNegocioId;
        // private TramiteDto tramite;
        private bool estado;
        private String proyectoTipo;
        private String proyectoNombre;
        private DateTime fechaMovimiento;
        private int usuarioId;
        private String usuario;
        private Guid flujoId;
        private int entidadId;
        #endregion Variables
    }
}
