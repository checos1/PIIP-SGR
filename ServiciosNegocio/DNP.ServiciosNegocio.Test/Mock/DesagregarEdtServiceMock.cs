using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock
{
    public class DesagregarEdtServiceMock : IDesagregarEdtPersistencia
    {
        public void EliminarActividad(string usuario, List<RegistroEntregable> nivelesNuevos)
        {
            throw new NotImplementedException();
        }

        public string ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
        {
            return "{\"ProyectoId\":97869,\"BPIN\":\"202200000000083\",\"Objetivos\":[{\"ObjetivoEspecificoId\":939,\"ObjetivoEspecifico\":\"Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL\",\"Productos\":[{\"ProductoId\":1495,\"NombreProducto\":\"1.1.Infraestructura penitenciaria y carcelaria construida -\",\"IndicadorPrincipal\":\"Cupos penitenciarios y carcelarios entregados(nacionales y territoriales) \",\"IndicadorId\":1769,\"UnidadMedidaProducto\":\"Número de cupos\",\"Cantidad\":11284.0000,\"EsAcumulativo\":\"NO\",\"EntregablesNivel1\":[{\"ActividadId\":4707,\"DeliverableCatalogId\":9,\"NombreEntregable\":\"Redes\",\"Deliverable\":true,\"Costo\":0.00,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":9,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1495},{\"DeliverableCatalogId\":2,\"Nivel\":\"Nivel2\",\"NombreEntregable\":\"Preliminares\",\"parentId\":9,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1495},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel3\",\"NombreEntregable\":\"Interventoria\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1495},{\"DeliverableCatalogId\":3,\"Nivel\":\"Nivel3\",\"NombreEntregable\":\"Movimiento de Tierra\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1495},{\"DeliverableCatalogId\":1,\"Nivel\":\"Nivel3\",\"NombreEntregable\":\"Pavimentación\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1495}]},{\"ActividadId\":4708,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0.00,\"CatalogoEntregables\":null},{\"ActividadId\":4709,\"DeliverableCatalogId\":8,\"NombreEntregable\":\"Obra civil\",\"Deliverable\":true,\"Costo\":0.00,\"CatalogoEntregables\":null}]}]}]}";
        }

        public void RegistrarActividad(string usuario, List<RegistroEntregable> nivelesNuevos)
        {
            throw new NotImplementedException();
        }


        public void RegistrarNivel(string usuario, List<RegistroEntregable> nivelesNuevos)
        {
            throw new NotImplementedException();
        }

        public string ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid)
        {
            return "{'ProyectoId':98059,'BPIN':'202200000000175','Preguntas':[{'PreguntaId':3752,'Pregunta':'¿Fecha programada de inicio de las actividades?','OpcionesRespuesta':'[{\'OpcionId\':13}]','Respuesta':'2022-09-21T00:00:00','ObligaObservacion':null,'ObservacionPregunta':null,'TipoPregunta':'Fecha','SoloLectura':'SI'},{'PreguntaId':3856,'Pregunta':'¿Fecha real de inicio de las actividades?','OpcionesRespuesta':null,'Respuesta':null,'ObligaObservacion':null,'ObservacionPregunta':null,'TipoPregunta':'Fecha','SoloLectura':'NO'},{'PreguntaId':3857,'Pregunta':'¿El avance del flujo de caja es igual al avance presupuestal?','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':null,'ObligaObservacion':null,'ObservacionPregunta':null,'TipoPregunta':'Opcion','SoloLectura':'NO'}],'VigenciaPeriodo':[{'PeriodoProyectoId':0,'Vigencia':0,'PeriodoPeriodicidadId':0,'Mes':'','VigenciaPeriodo':'','EjecucionPeriodo':[{'FuenteId':0,'FuenteFinanciacion':'','ApropiacionInicial':0,'ApropiacionVigente':0,'AcumuladoCompromisos':0,'AcumuladoObligacion':0,'AcumuladoPagos':0,'HabilitaApropiacionInicial':0,'HabilitaApropiacionVigente':0,'HabilitaAcumuladoCompromisos':0,'HabilitaAcumuladoObligacion':0,'HabilitaAcumuladoPagos':0}],'ReservaPresupuestal':[{'FuenteId':0,'FuenteFinanciacion':'','ApropiacionInicial':0,'ApropiacionVigente':0,'AcumuladoCompromisos':0,'AcumuladoObligacion':0,'AcumuladoPagos':0,'HabilitaApropiacionInicial':0,'HabilitaApropiacionVigente':0,'HabilitaAcumuladoCompromisos':0,'HabilitaAcumuladoObligacion':0,'HabilitaAcumuladoPagos':0}]}]}";
        }

        public string GuardarPreguntasAvanceFinanciero(ParametrosGuardarDto<List<PreguntasReporteAvanceFinancieroDto>> parametrosGuardar, string usuario)
        {
            return "OK";
        }

        public string ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId)
        {
            return "{'ProyectoId':98027,'BPIN':'202200000000164','PeriodosActivos':[{'Vigencia':2022,'PeriodosPeriodicidadId':10,'FechaDesde':'2022-10-01T00:00:00','FechaHasta':'2022-11-15T00:00:00'},{'Vigencia':2022,'PeriodosPeriodicidadId':11,'FechaDesde':'2022-11-01T00:00:00','FechaHasta':'2022-11-30T00:00:00'}],'Vigencias':[{'PeriodoProyectoId':19650,'Vigencia':2022,'VigenciaActiva':0},{'PeriodoProyectoId':19652,'Vigencia':2023,'VigenciaActiva':0},{'PeriodoProyectoId':19653,'Vigencia':2024,'VigenciaActiva':0},{'PeriodoProyectoId':19651,'Vigencia':2025,'VigenciaActiva':0}],'Periodos':[{'PeriodosPeriodicidadId':1,'Mes':'Enero','MesActivo':0},{'PeriodosPeriodicidadId':2,'Mes':'Febrero','MesActivo':0},{'PeriodosPeriodicidadId':3,'Mes':'Marzo','MesActivo':0},{'PeriodosPeriodicidadId':4,'Mes':'Abril','MesActivo':0},{'PeriodosPeriodicidadId':5,'Mes':'Mayo','MesActivo':0},{'PeriodosPeriodicidadId':6,'Mes':'Junio','MesActivo':0},{'PeriodosPeriodicidadId':7,'Mes':'Julio','MesActivo':0},{'PeriodosPeriodicidadId':8,'Mes':'Agosto','MesActivo':0},{'PeriodosPeriodicidadId':9,'Mes':'Septiembre','MesActivo':0},{'PeriodosPeriodicidadId':10,'Mes':'Octubre','MesActivo':0},{'PeriodosPeriodicidadId':11,'Mes':'Noviembre','MesActivo':0},{'PeriodosPeriodicidadId':12,'Mes':'Diciembre','MesActivo':0}],'Fuentes':[{'FuenteId':2293,'NombreFuente':'Inversión - Entidades Presupuesto Nacional - PGN - MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL - PGN - Nación - Inversión','RecursosVigentes':[{'PeriodoproyectoId':19650,'PeriodoPeriodicidadId':10,'Vigencia':2022,'RecursosVigentesApropiacionInicial':800000.0000,'RecursosVigentesApropiacionVigente':800000.0000,'RecursosVigentesAcumuladoCompromisos':0.0000,'RecursosVigentesAcumuladoObligaciones':0.0000,'RecursosVigentesAcumuladoPagos':0.0000,'HabilitaApropiacionInicial':false,'HabilitaApropiacionVigente':false,'HabilitaAcumuladoCompromisos':false,'HabilitaAcumuladoObligacion':false,'HabilitaAcumuladoPagos':false}],'RecursosPresupuestales':[{'PeriodoproyectoId':19650,'PeriodoPeriodicidadId':10,'Vigencia':2022,'ReservaPresupuestalApropiacionInicial':0.0000,'ReservaPresupuestalApropiacionVigente':0.0000,'ReservaPresupuestalAcumuladoCompromisos':0.0000,'ReservaPresupuestalAcumuladoObligaciones':0.0000,'ReservaPresupuestalAcumuladoPagos':0.0000,'HabilitaApropiacionInicial':false,'HabilitaApropiacionVigente':false,'HabilitaAcumuladoCompromisos':false,'HabilitaAcumuladoObligacion':false,'HabilitaAcumuladoPagos':false}]}]}";
        }

        public string GuardarAvanceFinanciero(ParametrosGuardarDto<AvanceFinancieroDto> parametrosGuardar, string usuario)
        {
            return "OK";
        }
    }
}
