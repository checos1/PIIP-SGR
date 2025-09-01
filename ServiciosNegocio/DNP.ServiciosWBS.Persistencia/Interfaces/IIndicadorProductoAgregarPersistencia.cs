using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{

    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IIndicadorProductoAgregarPersistencia
    {
        IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregar(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregarPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<IndicadorProductoAgregarDto> parametrosGuardar, string usuario);

    }
}
