namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    [ExcludeFromCodeCoverage]
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
        public CadenaValorDto ObtenerCadenaValorPreview()
        {
            return new CadenaValorDto();
        }
        public void Guardar(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,bool guardarTemporalmente)
        {
            
        }

        public ParametrosGuardarDto<CadenaValorDto> ConstruirParametrosGuardado(HttpRequestMessage request, CadenaValorDto contenido)
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
