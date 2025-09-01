using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ResumenLiberacionVfDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public decimal TotalValoresUtilizados { get; set; }
        public List<ResumenLiberacionVfValoresDto> ValoresAutorizadosUtilizados { get; set; }
        public decimal TotalAutorizadosNacion { get; set; } = 0;
        public decimal TotalAutorizadosPropios { get; set; } = 0;
        public decimal TotalUtilizadosNacion { get; set; } = 0;
        public decimal TotalUtilizadosPropios { get; set; } = 0;

    }

    public class ResumenLiberacionVfValoresDto
    {
        public int Vigencia { get; set; }
        public decimal AprobadosNacion { get; set; }
        public decimal AprobadosNPropios { get; set; }
        public decimal UtilizadoNacion { get; set; }
        public decimal UtilizadoPropios { get; set; }
    }


}


