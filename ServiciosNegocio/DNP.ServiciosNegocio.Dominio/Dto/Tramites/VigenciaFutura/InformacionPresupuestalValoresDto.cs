namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalValoresDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int? TramiteId { get; set; }
        public bool AplicaConstante { get; set; }
        public int? AñoBase { get; set; }
        public List<InformacionPresupuestalResumenSolicitado> ResumensolicitadoFuentesVigenciaFutura { get; set; }
        public List<InformacionDetalleProductos> DetalleProductosVigenciaFutura { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionPresupuestalResumenSolicitado
    {
        public int? Vigencia { get; set; }
        public decimal? Deflactor { get; set; }
        public decimal? ValorFuentesNacion { get; set; }
        public decimal? ValorFuentesPropios { get; set; }
        public decimal? ValorAprobadoNacion { get; set; }
        public decimal? ValorAprobadoPropios { get; set; }

    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionDetalleProductos
    {
        public int? ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<InformacionProducto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionProducto
    {
        public int? ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal? TotalValores { get; set; }
        public List<InformacionVigencia> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InformacionVigencia
    {
        public decimal? Deflactor { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public decimal? ValorSolicitadoVF { get; set; }

    }

}