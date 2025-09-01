using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using Newtonsoft.Json;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Administracion
{
    using System.Data;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Xml.Linq;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
    using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AdministrarDocumentoPersistencia : Persistencia, IAdministrarDocumentoPersistencia
    {
        public AdministrarDocumentoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }
        public string AdministrarDocumentoConsultar(string NombreDocumento)
        {
            try
            {
                var parameter = new SqlParameter("NombreDocumento",
                  string.IsNullOrEmpty(NombreDocumento) ? (object)DBNull.Value : NombreDocumento);

                var obj = Contexto.Database.SqlQuery<string>("Transversal.uspGetDocumentosConsultar @NombreDocumento",
                parameter
                ).ToList();

                return obj.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public string AdministrarDocumentoCrear(AdministracionDocumentoDto Documento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1,
                    Direction = ParameterDirection.Output,
                };

                Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostDocumentosCrear @NombreDocumento, @Codigo, @Activo, @ModificadoPor ,@ResultadoJSON output",
                new SqlParameter("NombreDocumento", Documento.NombreDocumento),
                new SqlParameter("Codigo", Documento.Codigo),
                new SqlParameter("Activo", Documento.Activo),
                new SqlParameter("ModificadoPor", Documento.ModificadoPor),
                outParam);
                return outParam.Value?.ToString();

            }
            catch (ServiciosNegocioException)
            {
                throw; // Re-lanza la excepción para manejarla en un nivel superior
            }
        }
        public string AdministrarDocumentoActualizar(AdministracionDocumentoDto Documento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1,
                    Direction = ParameterDirection.Output,
                };

                Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostDocumentosActualizar @Id, @NombreDocumento, @Codigo, @Activo, @ModificadoPor ,@ResultadoJSON output",
                new SqlParameter("Id", Documento.Id),
                new SqlParameter("NombreDocumento", Documento.NombreDocumento),
                new SqlParameter("Codigo", Documento.Codigo),
                new SqlParameter("Activo", Documento.Activo),
                new SqlParameter("ModificadoPor", Documento.ModificadoPor),
                outParam);
                return outParam.Value?.ToString();
            }
            catch (ServiciosNegocioException)
            {
                throw; // Re-lanza la excepción para manejarla en un nivel superior
            }
        }
        public string AdministrarDocumentoEliminar(string IdDocumento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1, // Ajustar el tamaño si es necesario
                    Direction = ParameterDirection.Output,
                };

                Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostDocumentosEliminar @IdDocumento, @ResultadoJSON output",
                new SqlParameter("IdDocumento", IdDocumento),
                outParam);
                return outParam.Value?.ToString();

            }
            catch (ServiciosNegocioException)
            {
                throw; // Re-lanza la excepción para manejarla en un nivel superior
            }
        }
        public string AdministrarDocumentoEstado(AdministracionDocumentoDto Documento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1,
                    Direction = ParameterDirection.Output,
                };

                Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostDocumentosEstado @Id, @Activo, @ModificadoPor ,@ResultadoJSON output",
                new SqlParameter("Id", Documento.Id),
                new SqlParameter("Activo", Documento.Activo),
                new SqlParameter("ModificadoPor", Documento.ModificadoPor),
                outParam);
                return outParam.Value?.ToString();
            }
            catch (ServiciosNegocioException)
            {
                throw; // Re-lanza la excepción para manejarla en un nivel superior
            }
        }
        public string AdministrarDocumentoReferencias()
        {
            try
            {
                var jsonString = Contexto.Database.SqlQuery<string>("Exec Transversal.uspGetDocumentosReferencias"
                 ).ToList();

                return jsonString.FirstOrDefault();
            }
            catch (Exception e)
            {

                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        /** Usos Documento */
        public string AdministrarDocumentoConsultarUso()
        {
            try
            {
                // Llama al procedimiento almacenado sin parámetros específicos
                var jsonString = Contexto.Database.SqlQuery<string>(
                    "Exec Transversal.uspGetDocumentosConsultarUso"
                ).ToList();

                return jsonString.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public string AdministrarCrearUsoDocumento(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1,
                    Direction = ParameterDirection.Output,
                };
                string rolesJson = JsonConvert.SerializeObject(Documento.Roles);

                Contexto.Database.ExecuteSqlCommand(
                    "Exec Transversal.uspPostDocumentosAsignarUso @IdFase, @ObligatoriedadId, @Obligatorio, @RolesJson, @TipoDocumentoId ,@TipoTramiteId ,@ValidacionId ,@Visible ,@ResultadoJSON output",

                    new SqlParameter("IdFase", Documento.IdFase),
                    new SqlParameter("ObligatoriedadId", Documento.ObligatoriedadId ?? (object)DBNull.Value),
                    new SqlParameter("Obligatorio", Documento.Obligatorio ?? (object)DBNull.Value),
                    new SqlParameter("RolesJson", rolesJson),
                    new SqlParameter("TipoDocumentoId", Documento.TipoDocumentoId),
                    new SqlParameter("TipoTramiteId", Documento.TipoTramiteId),
                    new SqlParameter("ValidacionId", Documento.ValidacionId ?? (object)DBNull.Value),
                    new SqlParameter("Visible", Documento.Visible),
                    outParam
                );

                return outParam.Value?.ToString();
            }
            catch (ServiciosNegocioException ex)
            {
                // Registrar el error o realizar otra acción específica
                throw new ServiciosNegocioException("Ocurrió un error al intentar asignar el uso al documento.", ex);
            }
        }

        public string AdministrarActualizarUsoDocumento(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = -1,
                    Direction = ParameterDirection.Output,
                };
                string rolesJson = JsonConvert.SerializeObject(Documento.Roles);

                // Ejecutar el procedimiento almacenado
                Contexto.Database.ExecuteSqlCommand(
                    "EXEC Transversal.uspPostDocumentosActualizarUso @Id, @IdFase, @ObligatoriedadId, @Obligatorio, @RolesJson, @TipoDocumentoId, @TipoTramiteId, @ValidacionId, @Visible, @ResultadoJSON output",
                    new SqlParameter("Id", Documento.Id),
                    new SqlParameter("IdFase", Documento.IdFase),                    
                    new SqlParameter("ObligatoriedadId", Documento.ObligatoriedadId),
                    new SqlParameter("Obligatorio", Documento.Obligatorio),
                    new SqlParameter("RolesJson", rolesJson),
                    new SqlParameter("TipoDocumentoId", Documento.TipoDocumentoId),
                    new SqlParameter("TipoTramiteId", Documento.TipoTramiteId),
                    new SqlParameter("ValidacionId", Documento.ValidacionId),
                    new SqlParameter("Visible", Documento.Visible),
                    outParam
                );

                return outParam.Value?.ToString();
            }
            catch (ServiciosNegocioException ex)
            {
                // Manejar la excepción específica
                throw new ServiciosNegocioException("Ocurrió un error al intentar actualizar el uso del documento.", ex);
            }
        }


        public string AdministrarDocumentoUsoEliminar(string Id)
        {
            try
            {
                var outParam = new SqlParameter
                {
                    ParameterName = "ResultadoJSON",
                    SqlDbType = SqlDbType.VarChar,
                    Size = -1, // Ajustar el tamaño si es necesario
                    Direction = ParameterDirection.Output,
                };
                Contexto.Database.ExecuteSqlCommand("Exec Transversal.uspPostDocumentosEliminarUso @Id, @ResultadoJSON output",
                new SqlParameter("Id", Id),
                outParam);
                return outParam.Value?.ToString();
            }
            catch (ServiciosNegocioException)
            {
                throw; // Re-lanza la excepción para manejarla en un nivel superior
            }
        }
    }

}

