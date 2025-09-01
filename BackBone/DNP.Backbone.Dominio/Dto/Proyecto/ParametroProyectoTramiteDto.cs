using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class ParametroProyectoTramiteDto
    {        
        public String TokenAutorizacion { get; set; }
        public Guid InstanciaId { get; set; }
        public List<ProyectoTramiteDto> Proyectos { get; set; }
    }


    public class DatosTramiteProyectosDto
    {
        public int? TramiteId { get; set; }
        public List<ProyectosTramiteDto> Proyectos { get; set; }
    }


    public class ProyectosTramiteDto
    {
        public int? ProyectoId { get; set; }
        public int? EntidadId { get; set; }
        public string TipoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public int TramiteId { get; set; }
        public Guid? FlujoId { get; set; }
        public Guid? InstanciaId { get; set; }
        public int? TipoRolId { get; set; }
        public decimal? ValorMontoNacionEnTramite { get; set; }
        public decimal? ValorMontoPropiosEnTramite { get; set; }
        public int? TipoTramiteId { get; set; }
    }

    public class ProyectosEnTramiteDto
    {
        public string Sector { get; set; }
        public string NombreEntidad { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int? ProyectoId { get; set; }
        public int? EntidadId { get; set; }
        public string TipoProyecto { get; set; }
        public string Estado { get; set; }
        public string EstadoActualizacion { get; set; }
        public int? TramiteId { get; set; }
        public decimal? ValorMontoProyectoNacion { get; set; }
        public decimal? ValorMontoProyectoNacionSSF { get; set; }
        public decimal? ValorMontoProyectoPropios { get; set; }
        public decimal? ValorMontoTramiteNacion { get; set; }
        public decimal? ValorMontoTramitePropios { get; set; }
        public decimal? ValorMontoAprobadosNacion { get; set; }
        public decimal? ValorMontoAprobadosPropios { get; set; }
        public decimal? ValorMontoEntidad { get; set; }
        public decimal? ValorMontoEnAprobacion { get; set; }
        public string Programa { get; set; }
        public string SubPrograma { get; set; }
        public string CodigoPresupuestal { get; set; }
        public decimal? ValorMontoTramiteNacionSSF { get; set; }
    }

    public class FuentesTramiteProyectoAprobacionDto
    {
        public int? FuenteId { get; set; }
        public string NombreFuente { get; set; }
        public string TipoAccion { get; set; }
        public string Origen { get; set; }
        public string TipoSituacion { get; set; }
        public int? ProyectoId { get; set; }
        public int? TramiteId { get; set; }
        public decimal? ValorInicialCSF { get; set; }
        public decimal? ValorInicialSSF { get; set; }
        public decimal? ValorVigenteCSF { get; set; }
        public decimal? ValorVigenteSSF { get; set; }
        public decimal? ValorSolicitadoCSF { get; set; }
        public decimal? ValorSolicitadoSSF { get; set; }
        public decimal? ValorAprobadoCSF { get; set; }
        public decimal? ValorAprobadoSSF { get; set; }
    }

    public class CapituloModificado
    {
        public int ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public string Usuario { get; set; }
        public int SeccionCapituloId { get; set; }
        public int CapituloId { get; set; }
        public int SeccionId { get; set; }
        public bool Modificado { get; set; }
        public Guid InstanciaId { get; set; }
        public int AplicaJustificacion { get; set; }
        public bool? Cuenta { get; set; }
    }

    public class ErroresProyectoDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Errores { get; set; }

    }

    public class DatosProyectoTramiteDto
    {
        public int? TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int? EntidadId { get; set; }
        public string Sector { get; set; }
        public string NombreProyecto { get; set; }
        public string BPIN { get; set; }
        public string EstadoProyecto { get; set; }
        public string MacroProceso { get; set; }
        public string Proceso { get; set; }
        public DateTime? FechaInicioProceso { get; set; }
        public string EstadoProceso { get; set; }
        public string NombrePaso { get; set; }
        public DateTime? FechaInicioPaso { get; set; }
        public string EntidadDestino { get; set; }
        public string CodigoPresupuestal { get; set; }
        public int? VigenciaInicial { get; set; }
        public int? VigenciaFinal { get; set; }
        public decimal? ValorInicialNacion { get; set; }
        public decimal? ValorInicialPropios { get; set; }
        public decimal? ValorVigenteNacion { get; set; }
        public decimal? ValorVigentePropios { get; set; }
        public decimal? ValorDisponibleNacion { get; set; }
        public decimal? ValorDisponiblePropios { get; set; }
        public decimal? ValorVigenciaFuturaNacion { get; set; }
        public decimal? ValorVigenciaFuturaPropios { get; set; }
        public string NombreEntidad { get; set; }
        public string FechaFinal { get; set; }
    }

    public class JustificacionPoliticaModificada
    {
        public int ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public string Usuario { get; set; }
        public int SeccionCapituloId { get; set; }
        public Guid InstanciaId { get; set; }
        public int PoliticaId { get; set; }
    }

}
