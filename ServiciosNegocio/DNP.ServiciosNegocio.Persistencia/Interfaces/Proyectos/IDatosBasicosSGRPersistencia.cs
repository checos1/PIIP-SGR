
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IDatosBasicosSGRPersistencia
    {
        DatosBasicosSGRDto ObtenerDatosBasicosSGR(string bpin);
              DatosBasicosSGRDto ObtenerDatosBasicosSGRPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<DatosBasicosSGRDto> parametrosGuardar, string usuario);
    }
}
