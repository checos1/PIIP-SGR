namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using Dominio.Dto.Proyectos;
    using Persistencia.Interfaces.Proyectos;

    [ExcludeFromCodeCoverage]
    public class ProyectoPersistenciaMock : IProyectoPersistencia
    {
        public ProyectoDto ObtenerProyectoPreview() { return new ProyectoDto() { Bpin = "1234567890" }; }
        public List<ProyectoEntidadDto> ObtenerProyectosEntidad(List<int> idEntidad, List<string> estados)
        {
            if (idEntidad.Equals(636))
            {
                return new List<ProyectoEntidadDto>()
                       {
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "1234",
                               EntidadId = 636,
                               EntidadNombre = "Entidad 636",
                               ProyectoId = 1,
                               ProyectoNombre = "Proyecto 1",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           },
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "5678",
                               EntidadId = 636,
                               EntidadNombre = "Entidad 636",
                               ProyectoId = 2,
                               ProyectoNombre = "Proyecto 2",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           },
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "9012",
                               EntidadId = 636,
                               EntidadNombre = "Entidad 636",
                               ProyectoId = 3,
                               ProyectoNombre = "Proyecto 3",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           }
                       };
            }

            if (idEntidad.Equals(79))
            {
                return new List<ProyectoEntidadDto>()
                       {
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "1234",
                               EntidadId = 79,
                               EntidadNombre = "Entidad 79",
                               ProyectoId = 1,
                               ProyectoNombre = "Proyecto 1",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           },
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "5678",
                               EntidadId = 79,
                               EntidadNombre = "Entidad 79",
                               ProyectoId = 2,
                               ProyectoNombre = "Proyecto 2",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           },
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "9012",
                               EntidadId = 79,
                               EntidadNombre = "Entidad 79",
                               ProyectoId = 3,
                               ProyectoNombre = "Proyecto 3",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           }
                       };
            }

            return null;
        }


        public List<ProyectoEntidadDto> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            //if (idEntidad.Equals(636))
            //{
            //    return new List<ProyectoEntidadDto>()
            //           {
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "1234",
            //                   EntidadId = 636,
            //                   EntidadNombre = "Entidad 636",
            //                   ProyectoId = 1,
            //                   ProyectoNombre = "Proyecto 1",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               },
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "5678",
            //                   EntidadId = 636,
            //                   EntidadNombre = "Entidad 636",
            //                   ProyectoId = 2,
            //                   ProyectoNombre = "Proyecto 2",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               },
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "9012",
            //                   EntidadId = 636,
            //                   EntidadNombre = "Entidad 636",
            //                   ProyectoId = 3,
            //                   ProyectoNombre = "Proyecto 3",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               }
            //           };
            //}

            //if (idEntidad.Equals(79))
            //{
            //    return new List<ProyectoEntidadDto>()
            //           {
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "1234",
            //                   EntidadId = 79,
            //                   EntidadNombre = "Entidad 79",
            //                   ProyectoId = 1,
            //                   ProyectoNombre = "Proyecto 1",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               },
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "5678",
            //                   EntidadId = 79,
            //                   EntidadNombre = "Entidad 79",
            //                   ProyectoId = 2,
            //                   ProyectoNombre = "Proyecto 2",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               },
            //               new ProyectoEntidadDto()
            //               {
            //                   CodigoBpin = "9012",
            //                   EntidadId = 79,
            //                   EntidadNombre = "Entidad 79",
            //                   ProyectoId = 3,
            //                   ProyectoNombre = "Proyecto 3",
            //                   SectorId = 1,
            //                   SectorNombre = "Sector 1"
            //               }
            //           };
            //}

            return null;
        }

        public List<EntidadDto> ObtenerEntidadesPorIds(List<string> idsEntidades)
        {
            return null;
        }

        public List<ProyectoEntidadDto> ObtenerProyectosPorEstados(List<int> idsProyectos,
                                                                   List<string> nombresEstadosProyectos)
        {
            if (idsProyectos.Contains(1) && nombresEstadosProyectos.Contains("Disponible"))
            {
                return new List<ProyectoEntidadDto>()
                       {
                           new ProyectoEntidadDto()
                           {
                               CodigoBpin = "1234",
                               EntidadId = 636,
                               EntidadNombre = "Entidad 636",
                               ProyectoId = 1,
                               ProyectoNombre = "Proyecto 1",
                               SectorId = 1,
                               SectorNombre = "Sector 1"
                           }
                       };
            }

            return null;
        }

        public List<ProyectoEntidadDto> ObtenerProyectosPorIds(List<int> ids)
        {
            return null;
        }

        public List<CrTypeDto> ObtenerCRType()
        {
            return new List<CrTypeDto>();
        }

        public List<FaseDto> ObtenerFase()
        {
            return new List<FaseDto>();
        }
        public List<MatrizEntidadDestinoAccionDto> ObtenerMatrizFlujo(int entidadResponsableId)
        {
            return new List<MatrizEntidadDestinoAccionDto>();
        }

        public RespuestaGeneralDto MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            return new RespuestaGeneralDto { Exito = false };
        }

        ProyectoDto IProyectoPersistencia.ObtenerProyectoPreview()
        {
            throw new System.NotImplementedException();
        }

        List<ProyectoEntidadDto> IProyectoPersistencia.ObtenerProyectosEntidad(List<int> idEntidades, List<string> estados)
        {
            throw new System.NotImplementedException();
        }

        List<ProyectoEntidadDto> IProyectoPersistencia.ObtenerProyectosPorEstados(List<int> idsEntidades, List<string> nombresEstadosProyectos)
        {
            throw new System.NotImplementedException();
        }

        List<ProyectoEntidadDto> IProyectoPersistencia.ObtenerProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            throw new System.NotImplementedException();
        }

        List<EntidadDto> IProyectoPersistencia.ObtenerEntidadesPorIds(List<string> idsEntidades)
        {
            throw new System.NotImplementedException();
        }

        List<ProyectoEntidadDto> IProyectoPersistencia.ObtenerProyectosPorIds(List<int> ids)
        {
            throw new System.NotImplementedException();
        }

        List<CrTypeDto> IProyectoPersistencia.ObtenerCRType()
        {
            return new List<CrTypeDto>()
                {
                    new CrTypeDto {
                        Id = 1,
                        Description = "PGN no Cofinanciadas"
                    },
                    new CrTypeDto {
                        Id = 2,
                        Description = "SGR"
                    }
            };
        }

        List<FaseDto> IProyectoPersistencia.ObtenerFase()
        {
            return new List<FaseDto>()
                {
                    new FaseDto {
                        Id = 1,
                        NombreFase = "Planeación",
                        FaseGUID = new Guid("F73990EF-04B5-4123-B87F-38DA445B6888")
                    },
                    new FaseDto {
                        Id = 2,
                        NombreFase = "Transferencia",
                        FaseGUID = new Guid("04F6E830-E64F-4FB8-A731-179A83350CC9")
                    }
            };
        }

        RespuestaGeneralDto IProyectoPersistencia.MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            throw new System.NotImplementedException();
        }

        List<MatrizEntidadDestinoAccionDto> IProyectoPersistencia.ObtenerMatrizFlujo(int EntidadResponsableId)
        {
            throw new System.NotImplementedException();
        }

        short IProyectoPersistencia.InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad)
        {
            throw new System.NotImplementedException();
        }

        List<AuditoriaEntidadDto> IProyectoPersistencia.ObtenerAuditoriaEntidad(int proyectoId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto)
        {
            List<ProyectoCreditoDto> proyectos = new List<ProyectoCreditoDto>();
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121212", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121232", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121234", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });

            return proyectos;
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto)
        {
            List<ProyectoCreditoDto> proyectos = new List<ProyectoCreditoDto>();
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121212", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121232", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121234", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });

            return proyectos;
        }

        public CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso, Guid NivelId, Guid FlujoId)
        {
            throw new NotImplementedException();
        }

        public RespuestaGeneralDto ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            throw new NotImplementedException();
        }

        RespuestaGeneralDto IProyectoPersistencia.AdicionarProyectoConpes(CapituloConpes Conpes, string usuario)
        {
            throw new NotImplementedException();
        }

        public List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId)
        {
            throw new NotImplementedException();
        }

        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin)
        {
            throw new NotImplementedException();
        }

        void IProyectoPersistencia.GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario)
        {
            throw new NotImplementedException();
        }

        void IProyectoPersistencia.AgregarEntregable(AgregarEntregable[] entregables, string usuario)
        {
            throw new NotImplementedException();
        }

        void IProyectoPersistencia.EliminarEntregable(EntregablesActividadesDto entregable)
        {
            throw new NotImplementedException();
        }

        ObjectivosAjusteJustificacionDto IProyectoPersistencia.ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
        {
            throw new NotImplementedException();
        }

        public LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto)
        {
            throw new NotImplementedException();
        }

        public List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(Guid InstanciaId, string BPIN)
        {
            throw new NotImplementedException();
        }

        string IProyectoPersistencia.ObtenerProyectosBeneficiarios(string Bpin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuario"></param> 
        /// </summary> 
        string IProyectoPersistencia.ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario)
        {
            throw new NotImplementedException();
        }

        string IProyectoPersistencia.ObtenerJustificacionProyectosBeneficiarios(string Bpin)
        {
            return string.Empty;
        }

        void IProyectoPersistencia.GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario)
        {
            if (beneficiario == null || string.IsNullOrWhiteSpace(beneficiario.BPIN) || beneficiario.NumeroPersonalAjuste < 0 || beneficiario.ProyectoId < 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoPersistencia.GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.InterventionLocalizationTypeId < 0 || beneficiario.PersonasBeneficiaros <= 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoPersistencia.GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId < 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoPersistencia.GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId < 0)
            {
                throw new NotImplementedException();
            }
        }

        public List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            throw new NotImplementedException();
        }

        public RespuestaGeneralDto ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            throw new NotImplementedException();
        }

        public string GetCategoriasSubcategorias_JSON(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
            throw new NotImplementedException();
        }

        public List<ProyectoEntidadDto> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros)
        {
            List<ProyectoEntidadDto> lProyectos = new List<ProyectoEntidadDto>();
            ProyectoEntidadDto proyecto = new ProyectoEntidadDto()
            {
                SectorId = 33,
                SectorNombre = "Cultura",
                EntidadId = 177,
                EntidadNombre = "MINISTERIO DE CULTURA - GESTION GENERAL",
                ProyectoId = 98071,
                ProyectoNombre = "Prestación de servicio de apoyo profesional para el fortalecimiento del sistema municipal de cultura \"hagamos de la cultura un empre Bogotá",
                CodigoBpin = "202200000000180",
                Estado = "En Ejecucion",
                EstadoId = 6,
                HorizonteInicio = 2022,
                HorizonteFin = 2026
            };
            lProyectos.Add(proyecto);
            proyecto = new ProyectoEntidadDto()
            {
                SectorId = 12,
                SectorNombre = "Justicia y del derecho",
                EntidadId = 159,
                EntidadNombre = "INSTITUTO NACIONAL PENITENCIARIO Y CARCELARIO - INPEC",
                ProyectoId = 98149,
                ProyectoNombre = "Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional",
                CodigoBpin = "202200000000219",
                Estado = "En Ejecucion",
                EstadoId = 6,
                HorizonteInicio = 2022,
                HorizonteFin = 2025
            };
            lProyectos.Add(proyecto);



            return lProyectos;

        }

        public RespuestaGeneralDto GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario)
        {
            RespuestaGeneralDto respuesta = new RespuestaGeneralDto();
            if (reprogramacionValores != null && reprogramacionValores[0].ProductoId > 0)
            {
                respuesta.Exito = true;
                respuesta.Mensaje = "Se guadó la información";
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "No se guada la información";
            }
            return respuesta;
        }
        public SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos)
        {
            SoportesDto soportes = new SoportesDto();
            return soportes;
        }

        public string ObtenerProyectosBeneficiariosDetalle(string Json)
        {
            return string.Empty;
        }

        public PlanNacionalDesarrolloDto ObtenerPND(int idProyecto)
        {
            return new PlanNacionalDesarrolloDto();
        }

        List<ProyectoPriorizarDto> IProyectoPersistencia.ObtenerProyectosPriorizar(string IdUsuarioDNP)
        {
            return new List<ProyectoPriorizarDto> { };
        }
    }
}

