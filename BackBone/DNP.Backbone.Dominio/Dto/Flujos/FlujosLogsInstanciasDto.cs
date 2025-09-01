using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    [ExcludeFromCodeCoverage]
    public class FlujosLogsInstanciasDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public Guid FlujoId { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid NivelId { get; set; }
        public string Proceso { get; set; }
        public string De { get; set; }
        public string Operacion { get; set; }
        public string A { get; set; }
        public string Rol { get; set; }
        public string Entidad { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class ParametrosInstanciaFlujoDto
    {
        public Guid FlujoId { get; set; }
        public string ObjetoId { get; set; }
        public Guid RolId { get; set; }
        public string UsuarioId { get; set; }
        public IEnumerable<int> ListaEntidades { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public string DireccionIp { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public bool CreadoAutomatico { get; set; }
        public int? IdProgramacion { get; set; }
        public int? IdNotificacion { get; set; }
        public string TipoProceso { get; set; }
        public string App { get; set; }
        public string Macroproceso { get; set; }

        public Guid IdInstancia { get; set; }

        //public List<NegocioFlujoDto> Proyectos { get; set; }

        public int? AnioInicio { get; set; }

        public int? AnioFinal { get; set; }
        public DateTime? FechaFinal { get; set; }
        public DateTime? FechaInicial { get; set; }
    }

    public class InstanciaResultado
    {
        public bool Exitoso { get; set; }
        public string MensajeOperacion { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? AccionPorInstanciaId { get; set; }
        public string NumeroTramite { get; set; }
    }

    public class FiltroConsultaOpcionesDto
    {
        public string IdAplicacion { get; set; }

        public string IdMacroproceso { get; set; }

        public string IdUsuario { get; set; }

        public List<string> IdsRoles { get; set; }
    }

    public class OpcionFlujoDto
    {
        public string IdOpcionDnp { get; set; }

        public int IdTipoOpcion { get; set; }

        public string NombreOpcion { get; set; }

        public string IdOpcionPadre { get; set; }

        public Guid IdNivel { get; set; }

        public Guid IdNivelPadre { get; set; }

        public TipoObjetoDto TipoObjeto { get; set; }
        public bool Activo { get; set; }

        public List<Guid> Roles { get; set; }
        public string AliasTipoTramite { get; set; }
        public int? PadreTramiteId { get; set; }
        public int TipoTramiteId { get; set; }
        public Guid IdFlujo { get; set; }
        public bool ActivoTipoTramite { get; set; }
    }

    public class TipoObjetoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
    }

    public class ParametrosProyectosFlujosDto
    {
        public List<int> IdsEntidades { get; set; }
        public List<string> NombresEstadosProyectos { get; set; }
        public string TokenAutorizacion { get; set; }
        public Guid IdTipoObjetoNegocio { get; set; }
        public int? SectorId { get; set; }
        public int? EntidadId { get; set; }
        public string CodigoBpin { get; set; }
        public string ProyectoNombre { get; set; }
        public string ProyectoEstado { get; set; }
        public string IdUsuarioDNP { get; set; }
        public List<Guid> IdsRoles { get; set; }
        public bool? TieneInstancias { get; set; }
        public Guid IdOpcionDNP { get; set; }
        //Estas propiedades se adicionan para realizar la busqueda de los proyectos
        public Guid flujoid { get; set; }
        public int tipoTramiteId { get; set; }
        public string tipoEntidad { get; set; }
    }

    public class HistoricoObservacionesDto
    {
        public string Paso { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaObservacion { get; set; }
        public string Observacion { get; set; }
    }
}
