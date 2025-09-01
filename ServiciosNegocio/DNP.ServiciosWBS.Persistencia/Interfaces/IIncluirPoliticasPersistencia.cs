using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IIncluirPoliticasPersistencia
    {
        IncluirPoliticasTDto ObtenerIncluirPoliticas(string bpin);
        IncluirPoliticasTDto ObtenerIncluirPoliticasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasTDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
