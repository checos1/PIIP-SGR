namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

    public class RegionalizaFuentesServicioMock : IRegionalizaFuentesServicio
    {
        public ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, FuenteFinanciacionRegionalizacionDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto>();

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

        public void Guardar(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {

        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin.Equals("202000000000005"))
            {
                return new FuenteFinanciacionRegionalizacionDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                    CR = 2,
                    Regionalizacion = new List<RegionalizacionDto> {
                        new RegionalizacionDto()
                        {
                           FuenteId = 660,
                           ProgramacionFuenteId = 1045,
                           FuenteFinanciacion = "Territorial - Boyacá - Propios",
                           Vigencia = 2019,
                           ValorSolicitado = 427352437
                        }
                    },
                };
            }

            return null;
        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview()
        {
            return new FuenteFinanciacionRegionalizacionDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                CR = 2,
                Regionalizacion = new List<RegionalizacionDto> {
                        new RegionalizacionDto()
                        {
                           FuenteId = 660,
                           ProgramacionFuenteId = 1045,
                           FuenteFinanciacion = "Territorial - Boyacá - Propios",
                           Vigencia = 2019,
                           ValorSolicitado = 427352437
                        }
                    },
            };
        }
    }
}
