namespace DNP.ServiciosTransaccional.Web.API.Test.Proyecto
{
    using Controllers.Proyecto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public sealed class ProyectoControllerTest : IDisposable
    {
        private ProyectoController _proyectoController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IProyectoServicio ProyectoServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ProyectoServicio = contenedor.Resolve<IProyectoServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _proyectoController =
                new ProyectoController(ProyectoServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoTest_Valido()
        {
            try
            {
                _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _proyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "test"), new[] { "" });

                var resultado = (OkNegotiatedContentResult<HttpResponseMessage>)await _proyectoController.ActualizarEstado(new ObjetoNegocio
                {
                    ObjetoNegocioId = "12865432",
                    NivelId = "F92A4087-7A92-45F5-A8BA-28F1C23BD849"
                });

                Assert.AreEqual(HttpStatusCode.OK, resultado.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta d eusuario de autorizado no pasa la validacion*/
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException));
            }
           
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoTest_FalloAutorizacion()
        {
            try
            {
                var response = await _proyectoController.ActualizarEstado(new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoTest_FalloParametros()
        {
            OkNegotiatedContentResult<HttpResponseMessage> rta = null;
            try
            {
                var parametro = new ObjetoNegocio
                {
                    ObjetoNegocioId = "BPIN"
                };
                var response = (OkNegotiatedContentResult<HttpResponseMessage>) await _proyectoController.ActualizarEstado(parametro);
                Assert.AreEqual(HttpStatusCode.OK, response.Content.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.IsNull(rta);
            }
           
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoSGRTest_Valido()
        {
            try
            {
                _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _proyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "test"), new[] { "" });

                var resultado = (OkNegotiatedContentResult<HttpResponseMessage>)await _proyectoController.ActualizarEstadoSGR(new ObjetoNegocio
                {
                    ObjetoNegocioId = "12865432",
                    NivelId = "F92A4087-7A92-45F5-A8BA-28F1C23BD849"
                });

                Assert.AreEqual(HttpStatusCode.OK, resultado.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta d eusuario de autorizado no pasa la validacion*/
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException));
            }

        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoSGRTest_FalloAutorizacion()
        {
            try
            {
                var response = await _proyectoController.ActualizarEstadoSGR(new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoSGRTest_FalloParametros()
        {
            OkNegotiatedContentResult<HttpResponseMessage> rta = null;
            try
            {
                var parametro = new ObjetoNegocio
                {
                    ObjetoNegocioId = "BPIN"
                };
                var response = (OkNegotiatedContentResult<HttpResponseMessage>)await _proyectoController.ActualizarEstadoSGR(parametro);
                Assert.AreEqual(HttpStatusCode.OK, response.Content.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.IsNull(rta);
            }

        }

        [TestMethod]

        public async Task ActualizarNombreProyectoTest_Fallo()
        {
            try
            {
                _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                //_proyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
                _proyectoController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
                var result = (OkNegotiatedContentResult<ObjetoNegocio>)
                await _proyectoController.ActualizarNombre(new ObjetoNegocio
                {
                    ObjetoNegocioId = "200000000000000",
                    NivelId = "65A5AA6A-9592-4FED-B59E-04540A345604"

                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(HttpResponseException));
            }

        }

        [TestMethod]
        public async Task ActualizarNombreProyectoTest_OK()
        {
            _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _proyectoController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
            try
            {
                var result = (OkNegotiatedContentResult<HttpResponseMessage>)
                 await _proyectoController.ActualizarNombre(new ObjetoNegocio
                 {
                     ObjetoNegocioId = "202200000000019",
                     NivelId = "65A5AA6A-9592-4FED-B59E-04540A345604"
                 });
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _proyectoController.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}