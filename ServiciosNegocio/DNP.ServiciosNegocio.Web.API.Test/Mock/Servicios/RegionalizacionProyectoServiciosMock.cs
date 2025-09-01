using DNP.ServiciosNegocio.Servicios.Interfaces.Formulario;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Comunes;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Dominio.Dto.Formulario;

    public class RegionalizacionProyectoServiciosMock: IRegionalizacionProyectoServicios
    {
        public RegionalizacionProyectoDto ObtenerRegionalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017011000236")
            {
                return new RegionalizacionProyectoDto()
                       {
                           Vigencias = new List<VigenciaDto>()
                                       {
                                           new VigenciaDto()
                                       }
                       };
            }

            return null;
        }
        public RegionalizacionProyectoDto ObtenerRegionalizacionPreview()
        {
            return new RegionalizacionProyectoDto();
        }
        public void Guardar(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {
            
        }
        public ParametrosGuardarDto<RegionalizacionProyectoDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, RegionalizacionProyectoDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<RegionalizacionProyectoDto>();

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
