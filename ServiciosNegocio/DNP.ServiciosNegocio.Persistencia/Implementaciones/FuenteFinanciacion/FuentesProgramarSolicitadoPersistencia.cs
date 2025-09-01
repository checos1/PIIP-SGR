using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.FuenteFinanciacion
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;

    public class FuentesProgramarSolicitadoPersistencia : Persistencia, IFuentesProgramarSolicitadoPersistencia
    {
        public FuentesProgramarSolicitadoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        public string ObtenerFuentesProgramarSolicitado(string bpin)
        {
            var listadoFuentesProgramarSolicitado = Contexto.uspGetFuentesProgramarSolicitado_JSON(bpin).FirstOrDefault();
            return listadoFuentesProgramarSolicitado;
        }

        public string GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    //string cadena = JsonUtilidades.ACadenaJson(objFuenteFinanciacionAgregarAjusteDto);
                    Contexto.uspPostProgramacionValorFuente(JsonUtilidades.ACadenaJson(objProgramacionValorFuenteDto), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                        //return mensajeError;
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

    }
}


