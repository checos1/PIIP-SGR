using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesMetasServicios
    {
        PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversales(ParametrosConsultaDto parametrosConsulta);
        PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversalesPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTMetasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTMetasDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTMetasDto contenido);
    }
}
