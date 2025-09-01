using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class ProgramacionDistribucionServicioMock : IProgramacionDistribucionServicio
    {
        public string ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId)
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

        public string ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public string ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
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
    }
}
