namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalVlrConstanteDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int? TramiteId { get; set; }
        public bool EsConstante { get; set; }
        public List<InformacionPresupuestalVigencia> Vigencias { get; set; }
        public List<InformacionPresupuestalObjetivo> ObjetivosEspecificos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalVigencia
    {
        public int? AnoBase { get; set; }
        public List<InformacionPresupuestalVigenciaValor> Valores { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalVigenciaValor
    {
        public int? Vigencia { get; set; }
        public decimal? ValorDeflactor { get; set; }
        public decimal? NacionConstante { get; set; }
        public decimal? PropiosConstante { get; set; }
        public decimal? NacionCorriente { get; set; }
        public decimal? PropiosCorriente { get; set; }
        public decimal? AprobadoNacionConstante { get; set; }
        public decimal? AprobadoPropiosConstante { get; set; }
        public decimal? AprobadoNacionCorriente { get; set; }
        public decimal? AprobadoPropiosCorriente { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalObjetivo
    {
        public int? ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<InformacionPresupuestalProducto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalProducto
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public decimal? TotalValoresConstantes { get; set; }
        public decimal? TotalValoresCorrientes { get; set; }
        public List<InformacionPresupuestalObjetivoVigencia> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalObjetivoVigencia
    {
        public int? Vigencia { get; set; }
        public decimal? NACION { get; set; }
        public decimal? PROPIOS { get; set; }
        public decimal? NacionCorriente { get; set; }
        public decimal? PropiosCorriente { get; set; }
    }
}
