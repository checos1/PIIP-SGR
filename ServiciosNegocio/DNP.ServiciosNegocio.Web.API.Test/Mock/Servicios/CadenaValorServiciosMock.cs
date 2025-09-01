using DNP.ServiciosNegocio.Servicios.Interfaces.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using System;
using System.Linq;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class CadenaValorServiciosMock : ICadenaValorServicios
    {
        public CadenaValorDto ObtenerCadenaValor(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017011000236")
            {
                return new CadenaValorDto();
            }

            return null;
        }
        public object ObtenerCadenaValorPreview()
        {
            return new CadenaValorDto();
        }
        public void Guardar(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {
            
        }

        public ParametrosGuardarDto<CadenaValorDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, CadenaValorDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<CadenaValorDto>();

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
    }
}
