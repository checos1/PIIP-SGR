using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramiteLiberacionVfDto
    {
        public int TramiteLiberadionId { get; set; }
        public int ProyectoLiberacionId { get; set; }
        public bool HabilitaValoresSolicitados { get; set; } = true;
        public List<TramiteALiberarVfDto> TramitesALiberar { get; set; }
    }

    public class TramiteALiberarVfDto
    {
        public int LiberacionVigenciasFuturasId { get; set; }
        public string CodigoProceso { get; set; }
        public string NombreProceso { get; set; }
        public DateTime? Fecha { get; set; }
        public string Objeto { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public bool EsConstante { get; set; }
        public string CodigoAutorizacion { get; set; }
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int AnioBase { get; set; }
        public string EstadoAutorizacion { get; set; }
        public string LabelBoton { get; set; } = "+";
        public bool EditarTramiteLiberacion { get; set; } = false;
        public DateTime? FechaAutorizacionOriginal { get; set; }
        public string CodigoAutorizacionOriginal { get; set; }
        public string EstadoAutorizacionOriginal { get; set; }
        public List<ValoresCorrientesAutorizaLiberacionDto> ValoresCorrientesAutorizaLiberacion { get; set; }
        public decimal TotalCorrientesAutorizadosNacion { get; set; } = 0;
        public decimal TotalCorrientesAutorizadosPropios { get; set; } = 0;
        public decimal TotalCorrientesUtilizadosNacion { get; set; } = 0;
        public decimal TotalCorrientesUtilizadosPropios { get; set; } = 0;
        public List<ValoresConstantesAutorizaLiberacionDto> ValoresConstantesAutorizaLiberacion { get; set; }
        public decimal TotalConstantesAutorizadosNacion { get; set; } = 0;
        public decimal TotalConstantesAutorizadosPropios { get; set; } = 0;
        public decimal TotalConstantesUtilizadosNacion { get; set; } = 0;
        public decimal TotalConstantesUtilizadosPropios { get; set; } = 0;
        public decimal TotalConstantesCorrientesAutorizadosNacion { get; set; } = 0;
        public decimal TotalConstantesCorrientesAutorizadosPropios { get; set; } = 0;
    }

    public class ValoresCorrientesAutorizaLiberacionDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int Vigencia { get; set; }
        public decimal AprobadoNacion { get; set; }
        public decimal AprobadoPropios { get; set; }
        public decimal UtilizadoNacion { get; set; }
        public decimal UtilizadoPropios { get; set; }
        public decimal UtilizadoNacionOriginal { get; set; }
        public decimal UtilizadoPropiosOriginal { get; set; }
    }

    public class ValoresConstantesAutorizaLiberacionDto
    {
        public int Vigencia { get; set; }
        public decimal Deflactor { get; set; }
        public decimal AprobadoConstanteNacion { get; set; }
        public decimal AprobadoConstantePropios { get; set; }
        public decimal UtilizadoConstanteNacion { get; set; }
        public decimal UtilizadoConstantePropios { get; set; }
        public decimal UtilizadoConstanteNacionOriginal { get; set; }
        public decimal UtilizadoConstantePropiosOriginal { get; set; }
        public decimal AprobadoCorrienteNacion { get; set; }
        public decimal AprobadoCorrientePropios { get; set; }
        public decimal UtilizadoCorrienteNacion { get; set; }
        public decimal UtilizadoCorrientePropios { get; set; }
    }
}


