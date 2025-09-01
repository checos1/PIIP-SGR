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
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

    public class FocalizacionServiciosMock: IFocalizacionServicios
    {
        public FocalizacionProyectoDto ObtenerProyectoFocalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017011000236")
            {
                return new FocalizacionProyectoDto()
                       {
                           Politicas = new List<PoliticaDto>()
                                       {
                                           new PoliticaDto()
                                       }
                       };
            }

            return null;
        }
        public FocalizacionProyectoDto ObtenerProyectoFocalizacionPreview()
        {
            return new FocalizacionProyectoDto();
        }
        public void Guardar(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {
            
        }
        public ParametrosGuardarDto<FocalizacionProyectoDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, FocalizacionProyectoDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<FocalizacionProyectoDto>();

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
