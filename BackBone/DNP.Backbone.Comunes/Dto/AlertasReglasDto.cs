using DNP.Backbone.Comunes.Dto.Base;
using DNP.Backbone.Comunes.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class AlertasReglasDto : DtoBase<int>
    {
        public int? Operador { get; set; }
        public string Valor { get; set; }
        public int? Condicional { get; set; }

        public int AlertasConfigId { get; set; }

        public int MapColumnasUnoId { get; set; }
        public MapColumnasDto MapColumnasUno { get; set; }

        public int? MapColumnasDosId { get; set; }
        public MapColumnasDto MapColumnasDos { get; set; }

    }

}
