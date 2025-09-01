using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ValoresUtilizadosLiberacionVfDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public List<TramitesVerificadosCorrientesDto> TramitesVerificadosCorrientes { get; set; }
        public List<TramitesVerificadosConstantesDto> TramitesVerificadosConstantes { get; set; }
    }

    public class TramitesVerificadosCorrientesDto
    {
        public int TramiteLiberarId { get; set; }
        public string CodigoProceso { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public string CodigoAutorizacion { get; set; }
        public List<ResumenValoresCorrientesDto> ResumenValoresCorrientes { get; set; }
        public List<DetalleObjetivosCorrientesDto> DetalleObjetivosCorrientes { get; set; }
        public string LabelBoton { get; set; }
        public decimal TotalValorSolicitado { get; set; }
        public decimal TotalValorTotalUtilizados { get; set; }
        public decimal TotalValorUtilizadoPorProductos { get; set; }
    }

    public class ResumenValoresCorrientesDto
    {
        public int Vigencia { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal ValorTotalUtilizados { get; set; }
        public decimal ValorUtilizadoPorProductos { get; set; }
    }

    public class DetalleObjetivosCorrientesDto
    {
        public int ObjetivoId { get; set; }
        public string Objetivo { get; set; }
        public List<DetalleProductosCorrientesDto> DetalleProductosCorrientes { get; set; }
    }

    public class DetalleProductosCorrientesDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Etapa { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public List<VigenciasDetalleProductosCorrientesDto> Vigencias { get; set; }
        public bool EditarTramiteLiberacion { get; set; }
        public string LabelBotonProducto { get; set; }
        public bool EsConstante { get; set; }
        public int TramiteLiberarId { get; set; }
        public decimal TotalValorSolicitado { get; set; }
        public decimal TotalValorTotalAprobado { get; set; }
        public decimal TotalValorUtilizado { get; set; }
    }

    public class VigenciasDetalleProductosCorrientesDto
    {
        public int ProgramacionVigenciaFuturaValoresId { get; set; }
        public int Vigencia { get; set; }
        public decimal Deflactor { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal ValorAprobado { get; set; }
        public decimal ValorUtilizado { get; set; }
        public decimal ValorUtilizadoOriginal { get; set; }
    }

    public class TramitesVerificadosConstantesDto
    {
        public int TramiteLiberarId { get; set; }
        public string CodigoProceso { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public string CodigoAutorizacion { get; set; }
        public int AnioBase { get; set; }
        public List<ResumenValoresConstantesDto> ResumenValoresConstantes { get; set; }
        public List<DetalleObjetivosConstantesDto> DetalleObjetivosConstantes { get; set; }
        public string LabelBoton { get; set; }
        public decimal TotalValorSolicitadoCorrientes { get; set; }
        public decimal TotalValorSolicitadoConstantes { get; set; }
        public decimal TotalTotalValorUtilizadoCorrientes { get; set; }
        public decimal TotalTotalValorUtilizadoConstantes { get; set; }
        public decimal TotalValorUtilizadoPorProductosCorrientes { get; set; }
        public decimal TotalValorUtilizadoPorProductosConstantes { get; set; }
    }

    public class ResumenValoresConstantesDto
    {
        public int Vigencia { get; set; }
        public decimal Deflactor { get; set; }
        public decimal ValorSolicitadoCorrientes { get; set; }
        public decimal ValorSolicitadoConstantes { get; set; }
        public decimal TotalValorUtilizadoCorrientes { get; set; }
        public decimal TotalValorUtilizadoConstantes { get; set; }
        public decimal ValorUtilizadoPorProductosCorrientes { get; set; }
        public decimal ValorUtilizadoPorProductosConstantes { get; set; }
    }

    public class DetalleObjetivosConstantesDto
    {
        public int ObjetivoId { get; set; }
        public string Objetivo { get; set; }
        public List<DetalleProductosConstantesDto> DetalleProductosConstantes { get; set; }
    }

    public class DetalleProductosConstantesDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Etapa { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int AnioBase { get; set; }
        public List<VigenciasDetalleProductosConstantesDto> Vigencias { get; set; }
        public bool EditarTramiteLiberacion { get; set; }
        public string LabelBotonProducto { get; set; }
        public bool EsConstante { get; set; }
        public int TramiteLiberarId { get; set; }
        public decimal TotalValorSolicitadoCorriente { get; set; }
        public decimal TotalValorTotalAprobadoCorriente { get; set; }
        public decimal TotalValorUtilizadoCorriente { get; set; }
        public decimal TotalValorSolicitadoConstante { get; set; }
        public decimal TotalValorTotalAprobadoConstante { get; set; }
        public decimal TotalValorUtilizadoConstante { get; set; }
    }

    public class VigenciasDetalleProductosConstantesDto
    {
        public int ProgramacionVigenciaFuturaValoresId { get; set; }
        public int Vigencia { get; set; }
        public decimal Deflactor { get; set; }
        public decimal ValorSolicitadoCorriente { get; set; }
        public decimal ValorSolicitadoConstante { get; set; }
        public decimal ValorAprobadoCorrientes { get; set; }
        public decimal ValorAprobadoConstantes { get; set; }
        public decimal ValorUtilizadoCorrientes { get; set; }
        public decimal ValorUtilizadoConstantes { get; set; }
        public decimal ValorUtilizadoConstantesOriginal { get; set; }
    }
}
