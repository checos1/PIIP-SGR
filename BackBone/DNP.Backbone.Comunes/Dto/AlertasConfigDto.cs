using DNP.Backbone.Comunes.Dto.Base;
using DNP.Backbone.Comunes.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class AlertasConfigDto : DtoBase<int>
    {
        public string NombreAlerta { get; set; }
        public int TipoAlerta { get; set; }
        public string MensajeAlerta { get; set; }
        public int Classificacion { get; set; }
        public bool Estado { get; set; }

        public string TipoAlertaDescripcion
        { get
            { 
                return Enum.GetName(typeof(TipoAlertaEnum), TipoAlerta); 
            } 
        }

        public string EstadoDescripcion
        {
            get
            {
                return Estado ? EstadoEnum.Activo.ToString() : EstadoEnum.Inactivo.ToString();
            }
        }

        public virtual List<AlertasReglasDto> AlertasReglasDtos { get; set; }
    }

}
