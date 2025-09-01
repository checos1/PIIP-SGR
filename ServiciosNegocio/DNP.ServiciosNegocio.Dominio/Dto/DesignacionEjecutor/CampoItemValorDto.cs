using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.DesignacionEjecutor
{
    public class CampoItemValorDto
    {
        public List<ItemValorDto> ListaValores { get; set; }
        public string Campo { get; set; }
    }
}
