using DNP.Backbone.Comunes.Dto.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class AlertasGeneradasDto : DtoBase<int>
    {
        public int AlertasConfigId { get; set; }
        public virtual AlertasConfigDto AlertasConfig { get; set; }

        public int ProyectoId { get; set; }
        public string Mensaje { get; set; }
        public int Classificacion { get; set; }
    }
}
