
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;

    public interface IDatosAdicionalesPersistencia
    {
        string ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId);
        RespuestaGeneralDto GuardarDatosAdicionales(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario);
        DatosAdicionalesResultado EliminarDatosAdicionales(int coFinanciacionId);

    }
}
