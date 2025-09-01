using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class DatosTramiteProyectosDto
    {
        public int? TramiteId { get; set; }
        public List<ProyectosTramiteDto> Proyectos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProyectosTramiteDto
    {
        public int? ProyectoId { get; set; }
        public int? EntidadId { get; set; }
        public string TipoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public int? TipoTramiteId { get; set; }
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public class TramiteFiltroDto
    {
        public string TokenAutorizacion { get; set; }
        public string IdUsuarioDNP { get; set; }
        public int TramiteId { get; set; }
        public Guid IdTipoObjetoNegocio { get; set; }
        public List<Guid> IdsRoles { get; set; }

        public List<FiltroGradeDto> FiltroGradeDtos { get; set; }

        public Guid? InstanciaId { get; set; }
        public int ProyectoId { get; set; }
        public string[] IdsEtapas { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class FiltroGradeDto
    {
        public string Campo { get; set; }
        public string Campo2 { get; set; }
        public string Valor { get; set; }
        public string Valor2 { get; set; }
        public FiltroTipo Tipo { get; set; }
        public FiltroTipo Tipo2 { get; set; }
        public TipoCombinacao Combinacao { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProyectoTramiteDto
    {
        public string IdUsuarioDNP { get; set; }
        public int TramiteId { get; set; }
        public Guid? FlujoId { get; set; }
        public Guid? InstanciaId { get; set; }
        public int ProyectoId { get; set; }
        public int EntidadId { get; set; }
        public int? TipoRolId { get; set; }
        public decimal? ValorMontoNacionEnTramite { get; set; }
        public decimal? ValorMontoPropiosEnTramite { get; set; }
        public string TipoProyecto { get; set; }
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public class UsuarioTramite
    {
        public Guid IdUsuario { get; set; }
        public string IDUsuarioDNP { get; set; }
        public string NombreUsuario { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public bool ActivoUsuario { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class Carta
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public string Proceso { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoId { get; set; }
        public string Tipo { get; set; }
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public bool Firmada { get; set; }
        public string RadicadoEntrada { get; set; }
        public string RadicadoSalida { get; set; }
        public List<CartaSecciones> ListaCartaSecciones { get; set; }
        public List<ConceptoDespedida> ListaConceptoDespedida { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CartaSecciones{
        public int Id { get; set; }
        public int CartaId { get; set; }
        public string NombreSeccion { get; set; }
        public List<CartaCampo> ListaCartaCampos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CartaCampo
    {
        public int Id { get; set; }
        public int CartaConceptoSeccionId { get; set; }
        public int PlantillaCartaCampoId { get; set; }
        public string DatoValor { get; set; }
        public string NombreCampo { get; set; }
        public TipoCampo TipoCampo { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PlantillaCarta
    {
        public int Id { get; set; }
        public int IipoTramiteId { get; set; }
        public List<PlantillaCartaSecciones> PlantillaSecciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PlantillaCartaSecciones
    {
        public int Id { get; set; }
        public int PlantillaCartaId { get; set; }
        public string NombreSeccion { get; set; }
        public List<Campo> PlantillaSeccionCampos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Campo {
        public int Id { get; set; }
        public int PlantillaCartaSeccionId { get; set; }
        public string NombreCampo { get; set; }
        public TipoCampo TipoCampo { get; set; }
        public string TituloCampo { get; set; }
        public string TextoDefecto { get; set; }
        public bool Editable { get; set; }
        public int Orden { get; set; }
        public string ConsultaSql { get; set; }
        public string ValorDefecto { get; set; }  

    }

    [ExcludeFromCodeCoverage]
    public class TipoCampo{
        public int TipoCampoId { get; set; }
        public int Longitud { get; set; }
        public string Formato { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CuerpoConceptoCDP
    {
        public string CDP { get; set; }
        public DateTime FechaCDP { get; set; }
        public string ValorCDP { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CuerpoConceptoAutorizacion
    {
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string Recurso { get; set; }
        public int ProgramaId { get; set; }
        public string ProgramaCodigo { get; set; }
        public string Programa { get; set; }
        public int SubprogramaId { get; set; }
        public string SubProgramaCodigo { get; set; }
        public string SubPrograma { get; set; }
        public string TipoProyecto { get; set; }
        public string Valor { get; set; }
        public string CodigoPresupuestal { get; set; }
        public DateTime FechaRadicacion { get; set; }
        public string NumeroRadicacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ConceptoDespedida
    {
        public string CartaFirmada { get; set; }
        public string PalabraFraseDespedida { get; set; }
        public string RemitenteDNP { get; set; }
        public string UsuarioRemitenteDNP { get; set; }
        public string PreparoEntidad { get; set; }
        public string PreparoColaborador { get; set; }
        public string RevisoEntidad { get; set; }
        public string RevisoColaborador { get; set; }
        public string CopiarEntidad { get; set; }
        public string CopiarServidor { get; set; }
        public int? DetalleId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class proyectoAsociarTramite
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public string Accion { get; set; }
        public string TipoProyecto { get; set; }
        public int? TramiteId { get; set; }
        public int? EntidadId { get; set; }
        public string NombreEntidad { get; set; }
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public class tramiteVFAsociarproyecto
    {
        public int? Id{ get; set; }
        public string NumeroTramite { get; set; }
        public string Descripcion { get; set; }
        public string ObjContratacion { get; set; }
        public int? tipotramiteId { get; set; }
        public DateTime? fecha { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class TramiteRVFAutorizacion
    {
        public int? Id { get; set; }
        public string NumeroTramite { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string Descripcion { get; set; }
        public int? TramiteLiberarId { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public int? ReprogramacionId { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class tramiteRVFAsociarproyecto
    {
        public int? Id { get; set; }
        public string NumeroTramite { get; set; }
        public string Descripcion { get; set; }
        public int? ProyectoId { get; set; }
        public int? TramiteId { get; set; }
        public int? EntidadId { get; set; }
        public int? ReprogramacionId { get; set; }
    }
    public enum FiltroTipo
    {
        Igual = 0,
        Diferente = 1,
        Menor = 2,
        MenorIgual = 3,
        Maior = 4,
        MaiorIgual = 5,
        Contem = 6,
        Inicia = 7,
        Termina = 8
    }

    public enum TipoCombinacao
    {
        NULO = 0,
        E = 1,
        OU = 2
    }

}
