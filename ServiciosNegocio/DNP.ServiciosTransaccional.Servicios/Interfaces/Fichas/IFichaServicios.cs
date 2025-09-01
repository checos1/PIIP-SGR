using DNP.ServiciosNegocio.Comunes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas
{
    public  interface IFichaServicios
    {
        string Usuario { get; set; }
        string Ip { get; set; }

        Task<string> ObtenerAnexoRadicadoTramite(int tramiteId, string nombreReporte, string idReporte, string usuarioDnp);

        Task<FichaPlantillaReporteDto> ObtenerPlantillaReporteAnexo(string nombreReporteRadicado, string usuarioDnp);

        Task<string> ObtenerFichaFisicaSGR(string instanciaId, string nivelId, string tramiteId, string nombreReporte, string idReporte, bool borrador, string usuarioDnp);
    }
}
