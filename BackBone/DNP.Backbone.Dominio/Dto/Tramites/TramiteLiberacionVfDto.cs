using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
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
        public decimal TotalCorrientesAutorizadosNacion { get; set; }
        public decimal TotalCorrientesAutorizadosPropios { get; set; }
        public decimal TotalCorrientesUtilizadosNacion { get; set; }
        public decimal TotalCorrientesUtilizadosPropios { get; set; }
        public List<ValoresConstantesAutorizaLiberacionDto> ValoresConstantesAutorizaLiberacion { get; set; }
        public decimal TotalConstantesAutorizadosNacion { get; set; }
        public decimal TotalConstantesAutorizadosPropios { get; set; }
        public decimal TotalConstantesUtilizadosNacion { get; set; }
        public decimal TotalConstantesUtilizadosPropios { get; set; }
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
