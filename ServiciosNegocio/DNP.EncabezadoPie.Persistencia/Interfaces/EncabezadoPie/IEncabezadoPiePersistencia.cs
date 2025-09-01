
namespace DNP.EncabezadoPie.Persistencia.Interfaces.EncabezadoPie
{
    using Dominio.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEncabezadoPiePersistencia
    {
        EncabezadoPieBasicoDto ConsultarEncabezadoPieBasico(ParametrosEncabezadoPieDto parametros);
        EncabezadoPieBasicoDto ConsultarEncabezadoPieBasicoPreview();
        EncabezadoGeneralDto ObtenerEncabezadoGeneral(ParametrosEncabezadoGeneral parametros);
    }
}
