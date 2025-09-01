using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{
    public interface IIncluirPoliticasPersistencia
    {
        IncluirPoliticasDto ObtenerIncluirPoliticas(string bpin);
        IncluirPoliticasDto ObtenerIncluirPoliticasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
