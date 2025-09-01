using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SeguimientoControl
{
    public class ProgramarProductosPersistencia : Persistencia, IProgramarProductosPersistencia
    {
        #region Constructor

        /// <summary>
        /// Constructor de DesagregarEdtPersistencia
        /// </summary>
        /// <param name=\\\\"contextoFactory\\\\"></param>
        public ProgramarProductosPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
           
        }

        #endregion

        #region Get
        public string ObtenerListadoObjProdNiveles(string bpin)
        {
            var resultSp = Contexto.uspGetProgramarProductos_JSON(bpin, null).SingleOrDefault();
            return resultSp;
            //return "{\"ProyectoId\":97649,\"BPIN\":\"202100000000007\",\"Objetivos\":[{\"ObjetivoEspecificoId\":699,\"ObjetivoEspecifico\":\"Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL\",\"Productos\":[{\"ProductoId\":1253,\"NombreProducto\":\"1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -\",\"IndicadorPrincipal\":\"Cupos penitenciarios y carcelarios entregados(nacionales y territoriales) \",\"IndicadorId\":1352,\"UnidadMedidaProducto\":\"Número de cupos\",\"Cantidad\":11284,\"EsAcumulativo\":null,\"EntregablesNivel1\":[{\"ActividadId\":4040,\"DeliverableCatalogId\":8,\"NombreEntregable\":\"Caso 1: Nivel 1 con solo Producto pero sin registros de Producto -Obra civil 1.1.Infraestructura penitenciaria y carcelaria construida\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":null},{\"ActividadId\":4041,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 pero sin registros en Nivel 2 - Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4041,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":null},{\"ActividadId\":4042,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con dos registros en Nivel 2 sin Producto -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":6,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"tercer entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":4,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":5,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":6,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":7,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 2\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":8,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 3\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"}]},{\"ActividadId\":4043,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con Nivel 3 con dos registros en Nivel 2 y Solo en uno nivel 3 sin Producto -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4043,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 3\",\"NombreEntregable\":\"Nivel 3 - Hijo de Caracterización Tramo Vial, prueba de VER MAS en nivel 3\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4043,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":4043,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"}]},{\"ActividadId\":4044,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con Nivel 3 con dos registros en Nivel 2 y Solo en uno nivel 3 sin Producto -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4044,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 3\",\"NombreEntregable\":\"Nivel 3 - Hijo de Caracterización Tramo Vial, prueba de VER MAS en nivel 3\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4044,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":4044,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":6,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":7,\"Nivel\":\"\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Actividad Nivel 3 - Caracterización Tramo Vial\",\"PadreId\":6,\"TipoEntregable\":\"Actividad\"},{\"IdEntregable\":12,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":10,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 2\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":11,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 3\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"}]}]}]}]}";
        }

        #endregion

        public string GuardarProgramarProducto(ParametrosGuardarDto<ProgramarProductoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostProgramarProductos(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
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

