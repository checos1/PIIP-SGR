using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class ProgramacionServicioMock : IProgramacionServicio
    {
        public bool ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Guid? nivelId, int? seccionCapituloId)
        {
            return false;
        }

        public string ObtenerCargaMasivaCreditos()
        {
            return string.Empty;
        }

        public string ObtenerProgramacionProyectosSinPresupuestal(int? sectorId, int? entidadId, string proyectoId)
        {
            return string.Empty;
        }

        public string ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId)
        {
            return string.Empty;
        }

        public string ObtenerProgramacionSectores(int? sectorId)
        {
            return string.Empty;
        }

        public string ObtenerProgramacionEntidadesSector(int? sectorId)
        {
            return string.Empty;
        }

        public string ObtenerCalendarioProgramacion(Guid FlujoId)
        {
            return string.Empty;
        }

        public TramitesResultado RegistrarCargaMasivaCreditos(List<CargueCreditoDto> json, string usuario)
        {
            var resultado = new TramitesResultado();

            if (json != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionDistribucion.EntidadDestinoId != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerDatosProgramacionDetalle(int tramiteidProyectoId, string origen)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        

        public string ValidarCargaMasivaCreditos(List<CargueCreditoDto> json)
        {
            return string.Empty;
        }

        public TramitesResultado RegistrarCargaMasivaCuota(List<CargueCuotaDto> json, string usuario)
        {
            var resultado = new TramitesResultado();

            if (json != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> json, string usuario)
        {
            var resultado = new TramitesResultado();

            if (json != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> json, string usuario)
        {
            var resultado = new TramitesResultado();

            if (json != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionFuenteDto ProgramacionFuente, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionFuente.TramiteProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ValidarConsecutivoPresupuestal(List<ProyectoSinPresupuestalDto> json)
        {
            return string.Empty;
        }

        public string ValidarCargaMasivaCuotas(List<CargueCuotaDto> json)
        {
            return string.Empty;
        }

        public string ObtenerDatostProgramacionProducto(int tramiteiId)
        {
            var json = "{'TramiteId':2290,'Productos':[{'ProductCatalogId':1213,'NombreProducto':'Documentos de lineamientos técnicos ','indicador':'Documentos de lineamientos técnicos realizados','unidadMedida':'Número de documentos','recurso':5192479878.00,'Meta':1.0000},{'ProductCatalogId':2615,'NombreProducto':'Documentos de planeación','indicador':'Documentos de planeación realizados','unidadMedida':'Número de documentos','recurso':979097400.00,'Meta':4.0000}]}";
            return json;
        }

        public TramitesResultado GuardarDatosProgramacionProducto(ProgramacionProductoDto ProgramacionProductoDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionProductoDto.TramiteId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto ProgramacionIniciativa, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionIniciativa.TramiteProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto ProgramacionRegionalizacion, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionRegionalizacion.TramiteProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ConsultarPoliticasTransversalesProgramacion(string Bpin)
        {
            return string.Empty;
        }

        public TramitesResultado AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin)
        {
            return string.Empty;
        }
        public TramitesResultado EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId)
        {
            var resultado = new TramitesResultado();

            if (tramiteidProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (objIncluirPoliticasDto.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId)
        {
            var resultado = new TramitesResultado();

            if (proyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ObtenerCrucePoliticasProgramacion(string Bpin)
        {
            return string.Empty;
        }
        public string PoliticasSolicitudConceptoProgramacion(string Bpin)
        {
            return string.Empty;
        }
        public TramitesResultado GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public TramitesResultado SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            return string.Empty;
        }
        public string ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid,string bpin,string NombreProyecto)
        {
            var json = "{\"Proyectos\":[{\"ProyectoId\":115467,\"CodigoBPIN\":\"2018011000632\",\"NombreProyecto\":\"FORTALECIMIENTO DE LOS EQUIPOS DE ARMAMENTO, SEGURIDAD Y PROTECCIÓN, ORIENTADOS A CONSOLIDAR LA CONVIVENCIA Y SEGURIDAD CIUDADANA EN EL TERRITORIO   NACIONAL\"},{\"ProyectoId\":115510,\"CodigoBPIN\":\"2018011000696\",\"NombreProyecto\":\"MEJORAMIENTO DE LA MOVILIDAD ESTRATÉGICA, ORIENTADA AL SERVICIO DE POLICÍA EN EL TERRITORIO  NACIONAL\"}, {\"ProyectoId\":115512,\"CodigoBPIN\":\"2018011000708\",\"NombreProyecto\":\"FORTALECIMIENTO DE LA INFRAESTRUCTURA DE SOPORTE PARA EL BIENESTAR DE SOCIAL DE LOS FUNCIONARIOS DE LA POLICÍA NACIONAL\"}, {\"ProyectoId\":115517,\"CodigoBPIN\":\"2018011000618\",\"NombreProyecto\":\"FORTALECIMIENTO DE LAS MISIONES AÉREAS POLICIALES EN EL TERRITORIO NACIONAL\"},{\"ProyectoId\":115519,\"CodigoBPIN\":\"2018011000669\",\"NombreProyecto\":\"FORTALECIMIENTO DE LA INFRAESTRUCTURA ESTRATÉGICA OPERACIONAL ORIENTADA A CONSOLIDAR LA CONVIVENCIA Y SEGURIDAD CIUDADANA A NIVEL  NACIONAL\"}]";
        
            return json;
        }
        public TramitesResultado BorrarTramiteProyecto(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario) 
        {
            var resultado = new TramitesResultado();

            if (ProgramacionDistribucion.TramiteId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarDatosInclusion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            var resultado = new TramitesResultado();

            if (ProgramacionDistribucion.EntidadDestinoId != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }


         


        TramitesResultado IProgramacionServicio.GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (objIncluirPoliticasDto!= null && objIncluirPoliticasDto.DatosDimension.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        TramitesResultado IProgramacionServicio.EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (objIncluirPoliticasDto != null && objIncluirPoliticasDto.DimensionId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            return string.Empty;
        }
        TramitesResultado IProgramacionServicio.GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (objIncluirPoliticasDto != null && objIncluirPoliticasDto.DatosDimension.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin)
        {
            return string.Empty;
        }

        public TramitesResultado RegistrarCargaMasivaSaldos(int TipocargueId, string usuario)
        {
            var resultado = new TramitesResultado();

            if (TipocargueId  > 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId)
        {
            string resultado = null;
            if (TipoCargueDetalleId > 0 && CarguesIntegracionId > 0)
            {
                resultado = "exito";
            }
           

            return resultado;
        }

        public string ObtenerCargaMasivaSaldos(string TipoCargue)
        {
            string resultado = null;
            if (!string.IsNullOrEmpty(TipoCargue))
            {
                resultado = "exito";
            }
           
            return resultado;
        }


        public string ObtenerTipoCargaMasiva(string TipoCargue)
        {
            string resultado = null;
            if (!string.IsNullOrEmpty(TipoCargue))
            {
                resultado = "exito";
            }

            return resultado;
        }

        public TramitesResultado ValidarCargaMasiva(dynamic jsonListaRegistros, string usuario)
        {
            var resultado = new TramitesResultado();

            if (jsonListaRegistros != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerCargaMasivaSaldos(int? CargueId)
        {
            string resultado = null;
            if (CargueId.HasValue)
            {
                resultado = "exito";
            }

            return resultado;
        }

        public string ObtenerDetalleCargaMasivaSaldos(int? CargueId)
        {
            string resultado = null;
            if (CargueId.HasValue)
            {
                resultado = "exito";
            }

            return resultado;
        }


        public string ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            return string.Empty;
        }
        TramitesResultado IProgramacionServicio.GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuario)
        {
            var resultado = new TramitesResultado();

            if (proyectoId != 0 && politicaId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "Los parametros vienen sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

    }
}
