using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosTransaccional.Persistencia.Interfaces;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.ModificacionLey;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.ModificacionLey
{
    public class ModificacionLeyPersistencia : Persistencia, IModificacionLeyPersistencia
    {
        public ModificacionLeyPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoBpin = parametrosActualizar.Contenido.ObjetoNegocioId;
            var nivelId = parametrosActualizar.Contenido.NivelId;            

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostActualizarValoresPoliticasML @ObjetoNegocioId,@NivelId,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("ObjetoNegocioId", proyectoBpin),
                                            new SqlParameter("NivelId", nivelId),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || resultado >= 0)
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return true;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return false;
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
