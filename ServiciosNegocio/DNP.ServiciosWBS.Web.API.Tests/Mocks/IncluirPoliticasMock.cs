using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas;
using DNP.ServiciosWBS.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    public class IncluirPoliticasMock : IIncluirPoliticasServicios
    {
        public ParametrosGuardarDto<IncluirPoliticasTDto> ConstruirParametrosGuardado(HttpRequestMessage request, IncluirPoliticasTDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<IncluirPoliticasTDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idInstanciaFlujo"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idInstanciaFlujo"));

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idAccion"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idAccion"));

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }

        public void Guardar(ParametrosGuardarDto<IncluirPoliticasTDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            
        }

        public IncluirPoliticasTDto ObtenerIncluirPoliticas(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "202000000000005")
            {
                return new IncluirPoliticasTDto()
                {
                    Politicas = new List<Politicas>()
                    {
                         new Politicas()
                    }
                };
            }

            return null;
        }

        public IncluirPoliticasTDto ObtenerIncluirPoliticasPreview()
        {
            return new IncluirPoliticasTDto();
        }
    }
}
