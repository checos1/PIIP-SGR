using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SGR;
using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
using DNP.Backbone.Dominio.Dto.SGR.Transversal;
using DNP.Backbone.Servicios.Interfaces.SGR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class ViabilidadServiciosMock : ISGRViabilidadServicios
    {
        private readonly ISGRViabilidadServicios _viabilidadServicios;

        public Task<ParametroDto> SGR_Transversal_LeerParametro(string parametro, string usuarioDNP)
        {
            return Task.FromResult(new ParametroDto());
        }

        public Task <List<ListaParametrosDto>> SGR_Transversal_LeerListaParametros(string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> SGR_Acuerdo_GuardarProyecto(AcuerdoSectorClasificadorDto obj, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<List<LstAcuerdoSectorClasificadorDto>> SGR_Acuerdo_LeerProyecto(int proyectoId, Guid nivelId, string usuarioDNP)
        {
            return Task.FromResult(new List<LstAcuerdoSectorClasificadorDto>());
        }

        
        public Task<List<ListaDto>> SGR_Proyectos_LeerListas(Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP)
        {
            return Task.FromResult(new List<ListaDto>());
        }

        public Task<ResultadoProcedimientoDto> SGR_Viabilidad_GuardarInformacionBasica(InformacionBasicaViabilidadDto obj, string usuarioDNP)
        {
            return Task.FromResult(new ResultadoProcedimientoDto
            {
                Exito = true
            });
        }

        public Task<LeerInformacionGeneralViabilidadDto> SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode)
        {
            return Task.FromResult(new LeerInformacionGeneralViabilidadDto
            {
                Id = 582685,
                Nombre = "Proyecto de prueba",
                CodigoBPIN = "",
                Categorias = "",
                Fase = "Factibilidad",
                RegionSgrId = 1
            });
        }

        public Task<List<LeerParametricasViabilidadDto>> SGR_Viabilidad_LeerParametricas(int proyectoId, Guid nivelId, string usuarioDNP)
        {
            return Task.FromResult(new List<LeerParametricasViabilidadDto>
                    {
                        new LeerParametricasViabilidadDto
                        {
                            RegionesSgr = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Caribe"
                                }
                            },
                            CategoriasSgr = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Servicios"
                                }
                            },
                            Sectores = new List<ListaGenericaDto>
                            {
                                new ListaGenericaDto()
                                {
                                    Id = 1,
                                    Nombre = "Planeación"
                                }
                            }
                        }
                    });
        }

        Task<string> ISGRViabilidadServicios.SGR_Viabilidad_ObtenerPuntajeProyecto(Guid instanciaId, int entidadId, string usuarioDNP)
        {
            return Task.FromResult("{}");
        }

        Task<ResultadoProcedimientoDto> ISGRViabilidadServicios.SGR_Viabilidad_GuardarPuntajeProyecto(string puntajesProyecto, string usuarioDNP)
        {
            return Task.FromResult(new ResultadoProcedimientoDto
            {
                Exito = true
            });
        }
    }
}
