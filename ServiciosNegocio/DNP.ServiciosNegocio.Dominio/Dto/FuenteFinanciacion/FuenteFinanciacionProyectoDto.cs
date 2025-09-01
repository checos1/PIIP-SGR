using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FuenteFinanciacionProyectoDto
    {
        public FuenteFinanciacionProyectoDto()
        {
            this.FechaAutorizado = DateTime.MinValue;
            this.Autorizado = false;
        }
        public string CodigoBpin { get; set; }
        public int? CrProyecto { get; set; }
        public int? Mes { get; set; }
        public decimal? ApropiacionInicial { get; set; }
        public decimal? ApropiacionVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public int? FuenteId { get; set; }
        public int? ProgramacionId { get; set; }
        public int? EjecucionId { get; set; }

        /*********************************/
        public string GrupoRecurso { get; set; }
        public int? TipoEntidadId { get; set; }
        public string TipoEntidad { get; set; }
        public int? EntidadId { get; set; }
        public string OtraEntidad { get; set; }
        public string Entidad { get; set; }
        public int? TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public string NombreCompleto { get; set; }

        /*********************************/

        public string Nombre { get; set; }
        public int? Vigencia { get; set; }
        public decimal? Solicitado { get; set; }
        public decimal? ValorTotalProyecto { get; set; }
        public int? EtapaId { get; set; }
        public bool? Autorizado { get; set; }
        public DateTime? FechaAutorizado { get; set; }
        public List<FuenteFinanciacionProyectoDto> ListadoFuenteFinanciacion { get; set; }
    }

    public class FuenteFinanciacionResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
