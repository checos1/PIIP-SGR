using System.Collections.Generic;
namespace DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion
{
    using System;

    public class ConvenioDonanteDto
    {
        public ConvenioDonanteDto()
        {
        }

        public int? Id { get; set; }
        public int? ConvenioId { get; set; }
        public string NombreDonante { get; set; }
        public int?  EntityId { get; set; }
        public ConvenioDto objConvenioDto { get; set; }

    }
}
