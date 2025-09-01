using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IPoliticaTransversalBeneficiarioPersistencia
    {
        PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiario(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiarioPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTBeneficiarioDto> parametrosGuardar, string usuario);
    }
}
