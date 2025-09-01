using DNP.ServiciosNegocio.Comunes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites
{
    public interface IRadicadoService
    {
        Task<CommonResponseDto<dynamic>> GenerarRadicadoEntrada(string numeroTramite);
    }
}
