using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SGP;
using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
using DNP.Backbone.Dominio.Dto.SGP.Transversal;
using DNP.Backbone.Servicios.Interfaces.SGP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class SGPViabilidadServiciosMock : ISGPViabilidadServicios
    {
        
        public Task<ParametroDto> SGPTransversalLeerParametro(string parametro, string usuarioDNP)
        {
            return Task.FromResult(new ParametroDto());
        }

        public Task<string> SGPAcuerdoGuardarProyecto(AcuerdoSectorClasificadorSGPDto obj, string usuarioDNP)
        {
            //prueba
            throw new NotImplementedException();
        }

        public Task<List<LstAcuerdoSectorClasificadorDto>> SGPAcuerdoLeerProyecto(int proyectoId, Guid nivelId, string usuarioDNP)
        {
            return Task.FromResult(new List<LstAcuerdoSectorClasificadorDto>());
        }

        
        public Task<List<ListaDto>> SGPProyectosLeerListas(Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP)
        {
            return Task.FromResult(new List<ListaDto>());
        }

        public Task<ResultadoProcedimientoDto> SGPViabilidadGuardarInformacionBasica(InformacionBasicaViabilidadSGPDto obj, string usuarioDNP)
        {
            return Task.FromResult(new ResultadoProcedimientoDto
            {
                Exito = true
            });
        }

        public Task<LeerInformacionGeneralViabilidadDto> SGPViabilidadLeerInformacionGeneral(int proyectoId, Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode)
        {
            return Task.FromResult(new LeerInformacionGeneralViabilidadDto
            {
                Id = 665588,
                Nombre = "Test",
                CodigoBPIN = "",
                Categorias = "",
                Fase = "PreFactibilidad",
                RegionSgrId = 1
            });
        }

        public Task<List<LeerParametricasViabilidadDto>> SGPViabilidadLeerParametricas(int proyectoId, Guid nivelId, string usuarioDNP)
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

        public Task<string> GuardarProyectoViabilidadInvolucradosSGP(ProyectoViabilidadInvolucradosSGPDto objProyectoViabilidadInvolucradosDto, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarProyectoViabilidadInvolucradosSGP(int id, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProyectoViabilidadInvolucradosSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProyectoViabilidadInvolucradosFirmaSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
    }
}
