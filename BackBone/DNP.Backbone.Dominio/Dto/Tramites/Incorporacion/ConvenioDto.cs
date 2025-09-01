using System.Collections.Generic;
namespace DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion
{
    using System;

    public class ConvenioDto
    {
        public ConvenioDto()
        {
        }

        public int? Id { get; set; }

        public int? TramiteId { get; set; }

        public string NumeroConvenio { get; set; }

        public string ObjetoConvenio { get; set; }

        public decimal? ValorConvenio { get; set; }

        public decimal? ValorConvenioVigencia { get; set; }

        public string FechaInicial { get; set; }

        public string FechaFinal { get; set; }


    }
}
