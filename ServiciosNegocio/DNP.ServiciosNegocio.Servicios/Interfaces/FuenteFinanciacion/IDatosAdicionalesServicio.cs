using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;

    public interface IDatosAdicionalesServicio
    {
        string ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId);
        Task<RespuestaGeneralDto> GuardarDatosAdicionales(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario);
        DatosAdicionalesResultado EliminarDatosAdicionales(int coFinanciacionId);
    }
}
