using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICuantificacionBeneficiarioPersistencia
    {
        PoblacionDto ObtenerCuantificacionBeneficiario(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        PoblacionDto ObtenerCuantificacionBeneficiarioPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario);
    }
}
