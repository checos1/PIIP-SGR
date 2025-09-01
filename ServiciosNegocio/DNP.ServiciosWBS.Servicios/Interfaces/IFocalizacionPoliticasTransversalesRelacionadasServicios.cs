using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesRelacionadasServicios
    {
        PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasServicios(ParametrosConsultaDto parametrosConsulta);
        PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasServiciosPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTransversalRelacionadaDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTransversalRelacionadaDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTransversalRelacionadaDto contenido);
    }
}
