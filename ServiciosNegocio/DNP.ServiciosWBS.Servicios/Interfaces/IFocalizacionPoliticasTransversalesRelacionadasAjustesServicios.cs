using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios
    {
        PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServicios(ParametrosConsultaDto parametrosConsulta);
        PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServiciosPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTransversalRelacionadaAjustesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTransversalRelacionadaAjustesDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTransversalRelacionadaAjustesDto contenido);

    }
}
