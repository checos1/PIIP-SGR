using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IIncluirPoliticasServicios
    {
        IncluirPoliticasTDto ObtenerIncluirPoliticas(ParametrosConsultaDto parametrosConsulta);
        IncluirPoliticasTDto ObtenerIncluirPoliticasPreview();
        void Guardar(ParametrosGuardarDto<IncluirPoliticasTDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<IncluirPoliticasTDto> ConstruirParametrosGuardado(HttpRequestMessage request, IncluirPoliticasTDto contenido);
    }
}
