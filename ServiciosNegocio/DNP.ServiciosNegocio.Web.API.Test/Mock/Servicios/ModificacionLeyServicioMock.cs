using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class ModificacionLeyServicioMock : IModificacionLeyServicio
    {
        public string ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public string ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public TramitesResultado GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto InformacionPresupuestal, string usuario,string origen)
        {
            var resultado = new TramitesResultado();

            if (InformacionPresupuestal.TramiteProyectoId != 0)
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
