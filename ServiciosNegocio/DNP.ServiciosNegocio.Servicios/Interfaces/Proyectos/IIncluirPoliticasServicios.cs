using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
    public interface IIncluirPoliticasServicios
    {
        IncluirPoliticasDto ObtenerIncluirPoliticas(ParametrosConsultaDto parametrosConsulta);
        IncluirPoliticasDto ObtenerIncluirPoliticasPreview();
        void Guardar(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<IncluirPoliticasDto> ConstruirParametrosGuardado(HttpRequestMessage request, IncluirPoliticasDto contenido);
    }
}
