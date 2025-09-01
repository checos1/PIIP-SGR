using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PresupuestalProyectosAsociadosDto
    {
        public int TramiteId { get; set; }
        public List<ResumenProyectosAsociadosDto> ResumenProyectos { get; set; }
        public List<ProyectosAsociadosDto> ProyectosAsociados { get; set; }
        public List<ProyectosAsociadosDto> ProyectosAportantes { get; set; }
    }

    public class ResumenProyectosAsociadosDto
    {
        public string TipoOperacion { get; set; }
        public decimal? TotalTipoOperacion { get; set; }
        public decimal? totalTipoAprobacion { get; set; }
        public List<ProyectosAsociadosResDto> Proyectos { get; set; }
    }

    public class ProyectosAsociadosResDto
    {
        public int ProyectoId { get; set; }
        public string CodigoBpin { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreProyectoCorto { get; set; }
        public string CodigoPresupuestal { get; set; }
        public decimal? TotalSolicitadoNacion { get; set; }
        public decimal? TotalSolicitadoPropios { get; set; }
        public decimal? TotalAprobadoNacion { get; set; }
        public decimal? TotalAprobadoPropios { get; set; }
        
    }

    public class ProyectosAsociadosDto
    {
        public int ProyectoId { get; set; }
        public string CodigoBpin { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreProyectoCorto { get; set; }
        public string EntidadFinanciadora { get; set; }
        public string NombreSector { get; set; }
        public string TipoProyecto { get; set; }
        public string CodigoPresupuestal { get; set; }
        public int VigenciaInicial { get; set; }
        public int VigenciaFinal { get; set; }
        public decimal? TotalApropiacionInicialNacion { get; set; }
        public decimal? TotalApropiacionInicialPropios { get; set; }
        public decimal? TotalApropiacionVigenteNacion { get; set; }
        public decimal? TotalApropiacionVigentePropios { get; set; }
        public decimal? TotalVigenciasFuturasNacion { get; set; }
        public decimal? TotalVigenciasFuturasPropios { get; set; }
        public decimal? MontoTramiteNacion { get; set; }
        public decimal? MontoTramitePropios { get; set; }
        public List<DetalleFuentesProyectosAsociadosDto> DetalleFuentes { get; set; }
        public string LabelBoton { get; set; }
        public bool EditarTramiteIncorporacion { get; set; }
    }

    public class DetalleFuentesProyectosAsociadosDto
    {
        public int TipoRecursoId { get; set; }
        public string NombreTipoRecurso { get; set; }
        public decimal? ValorInicialCSF { get; set; }
        public decimal? ValorInicialSSF { get; set; }
        public decimal? ValorVigenteCSF { get; set; }
        public decimal? ValorVigenteSSF { get; set; }
        public decimal? ValorIncorporarCSF { get; set; }
        public decimal? ValorIncorporarSSF { get; set; }
        public decimal? ValorIncorporarCSFOriginal { get; set; }
        public decimal? ValorIncorporarSSFOriginal { get; set; }
        public decimal? ValorIncorporarAprobadoCSF { get; set; }
        public decimal? ValorIncorporarAprobadoSSF { get; set; }
        public decimal? ValorIncorporarAprobadoCSFOriginal { get; set; }
        public decimal? ValorIncorporarAprobadoSSFOriginal { get; set; }
    }
}
