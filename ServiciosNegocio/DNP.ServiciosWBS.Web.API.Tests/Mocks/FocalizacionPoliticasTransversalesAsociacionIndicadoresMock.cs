using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
using DNP.ServiciosWBS.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    public class FocalizacionPoliticasTransversalesAsociacionIndicadoresMock : IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios
    {
        public ParametrosGuardarDto<PoliticaTIndicadoresDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTIndicadoresDto contenido)
        {
            throw new NotImplementedException();
        }

        public void Guardar(ParametrosGuardarDto<PoliticaTIndicadoresDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            throw new NotImplementedException();
        }

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(ParametrosConsultaDto parametrosConsulta)
        {
            throw new NotImplementedException();
        }

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview()
        {
            throw new NotImplementedException();
        }
    }
}
