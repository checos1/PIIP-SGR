using DNP.ServiciosNegocio.Persistencia.Interfaces;
using System;
using System.Linq;
using DNP.ServiciosNegocio.Persistencia.Interfaces.AdministradorEntidad;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using System.Data.SqlClient;
using DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using System.Data.Entity.Core.Objects;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System.Data;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.AdministradorEntidad
{
    public class AdministradorEntidadSgpPersistencia : Persistencia, IAdministradorEntidadSgpPersistencia
    {
        public AdministradorEntidadSgpPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public string ObtenerSectores()
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("dbo.UspGetSectoresSGP").SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerFlowCatalog()
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("dbo.UspGetFlowCatalogSGP").SingleOrDefault();
            return jsonConsulta;
        }

        public List<ConfiguracionMatrizEntidadDestinoSGRDto> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            var response = new List<ConfiguracionMatrizEntidadDestinoSGRDto>();

            foreach (var item in dto.ListMatrizEntidad)
            {
                List<MatrizEntidadDestinoSGRDto> resultado = Contexto.Database.SqlQuery<MatrizEntidadDestinoSGRDto>("dbo.spGetMatrizEntidadDestinoSGP @EntidadResponsableId,@ResourceGroupId,@JsonDataSector, @JsonDataEntidad",
                                    new SqlParameter("EntidadResponsableId", item.EntidadResponsableId),
                                    new SqlParameter("ResourceGroupId", item.ResourceGroupId),
                                    new SqlParameter("JsonDataSector", JsonUtilidades.ACadenaJson(item.ListSectorId)),
                                    new SqlParameter("JsonDataEntidad", JsonUtilidades.ACadenaJson(item.ListEntidadDestinoId))
                                     ).ToList();

                ConfiguracionMatrizEntidadDestinoSGRDto datos = new ConfiguracionMatrizEntidadDestinoSGRDto();

                datos.Respuesta = resultado;
                if(resultado.Count > 0) datos.FlowId = resultado[0].TipoFlujo;
                datos.EntidadResponsableId = item.EntidadResponsableId;

                response.Add(datos);
            }

            return response;
        }

        public RespuestaGeneralDto ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            var json = JsonUtilidades.ACadenaJson(dto);

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "errorValidacionNegocio",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostMatrizEntidadDestinoSGP @json,@Usuario,@errorValidacionNegocio output",
                                            new SqlParameter("json", json),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );

                    if (outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = "OK";
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}
