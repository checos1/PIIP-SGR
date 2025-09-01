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
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;

    public class RegionalizacionIndicadorAgregarServiciosMock : IRegionalizacionIndicadorAgregarServicios
    {

        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017761220016")
            {
                return new RegionalizacionIndicadorAgregarDto()
                {
                    Objetivos = new List<ObjetivosRegionalizacionIndicadorDto>()
                                       {
                                           new ObjetivosRegionalizacionIndicadorDto()
                                       }


                };
            }

            return null;
        }


        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview()
        {
            return new RegionalizacionIndicadorAgregarDto();
        }


        public ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, RegionalizacionIndicadorAgregarDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto>();

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

        public void Guardar(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {

        }

    }
}
