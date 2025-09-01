using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using System.Collections.Generic;


namespace DNP.ServiciosNegocio.Web.API.Test.Mock
{
    public class ProgramarProductoServiceMock : IProgramarProductosPersistencia
    {
        public string GuardarProgramarProducto(ParametrosGuardarDto<ProgramarProductoDto> parametrosGuardar, string usuario)
        {
            return "OK";
        }

        public string ObtenerListadoObjProdNiveles(string bpin)
        {
            return bpin;
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
                var mensajeError = "No existe el id del trámite para el proyecto.";
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
                var mensajeError = "No existe el id del proyecto.";
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
                var mensajeError = "No existe el id del proyecto.";
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
                var mensajeError = "No existe el id del proyecto.";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            return string.Empty;
        }

        public string ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            return string.Empty;
        }

        public TramitesResultado GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
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
        public string ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            return string.Empty;
        }
    }
}
