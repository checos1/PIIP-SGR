namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

    public class CostosActividadesServiciosMock : ICostosActividadesServicios
    {
        public ParametrosGuardarDto<CostosActividadesDto> ConstruirParametrosGuardado(HttpRequestMessage request, CostosActividadesDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<CostosActividadesDto>();

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

        public void Guardar(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {

        }

        public CostosActividadesDto ObtenerCostosActividades(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin.Equals("202000000000005"))
            {
                return new CostosActividadesDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",

                    vigencias = new List<Vigencia> {
                        new Vigencia()
                        {
                          vigencia = 2020
                        }
                    },
                
            };
            }

            return null;
        }

        public CostosActividadesDto ObtenerCostosActividadesPreview()
        {
            return new CostosActividadesDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
               
                vigencias = new List<Vigencia> {
                        new Vigencia()
                        {
                          vigencia = 2020
                        }
                    },
            };
        }
    }
}
