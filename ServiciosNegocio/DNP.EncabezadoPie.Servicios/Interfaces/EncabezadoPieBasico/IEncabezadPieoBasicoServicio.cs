
namespace DNP.EncabezadoPie.Servicios.Interfaces.EncabezadoPieBasico
{
    using Dominio.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEncabezadPieoBasicoServicio
    {
        EncabezadoPieBasicoDto ConsultarEncabezadoPieBasico(ParametrosEncabezadoPieDto parametros);
        EncabezadoPieBasicoDto ConsultarEncabezadoPieBasicoPreview();
        EncabezadoGeneralDto ObtenerEncabezadoGeneral(ParametrosEncabezadoGeneral parametros);
    }
}
