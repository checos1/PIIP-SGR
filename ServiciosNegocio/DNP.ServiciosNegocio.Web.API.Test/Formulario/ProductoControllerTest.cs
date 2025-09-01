namespace DNP.ServiciosNegocio.Web.API.Test.Formulario
{
    using Comunes.Autorizacion;
    using Controllers.Formulario;
    using Dominio.Dto.Proyectos;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Formulario;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
        public sealed class ProductoControllerTest : IDisposable
        {
            private ProductoController _productoController;
            private IProductosServicio ProductosServicio { get; set; }
            private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }

            [TestInitialize]
            public void Init()
            {
                var contenedor = Configuracion.UnityConfig.Container;
                ProductosServicio = contenedor.Resolve<IProductosServicio>();
                AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();

                _productoController =
                    new ProductoController(ProductosServicio, AutorizacionUtilidades)
                    {
                        Request =
                            new HttpRequestMessage()
                    };
            }


        [TestMethod]
        public async Task ConsultarProductoServicio__RetornaDto()
        {

            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=2017730010016");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            _productoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            var result = (OkNegotiatedContentResult<ProyectoDto>)await _productoController.Consultar();
            Assert.IsNotNull(result.Content);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada BPIN inválido.")]
        public async Task ConsultarProductoServicioConBpinInvalido__RetornaMensajeInvalido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin='PRUEBA'");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            _productoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            HttpUtility.ParseQueryString(_productoController.Request.RequestUri.Query).Set("bpin", "PRUEBA");

            await _productoController.Consultar();

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro BPIN no recibido.")]
        public async Task ConsultarProductoServicioConBpinVacio__RetornaMensajeNoRecibido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            _productoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            await _productoController.Consultar();

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task ConsultarProductoServicioConidAccionInvalido__RetornaMensajeInvalido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=2017730010016");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _productoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            HttpUtility.ParseQueryString(_productoController.Request.RequestUri.Query).Set("bpin", "2017730010016 ");

            await _productoController.Consultar();

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task ConsultarProductoServicioConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=2017730010016");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", "");
            _productoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            HttpUtility.ParseQueryString(_productoController.Request.RequestUri.Query).Set("bpin", "2017730010016 ");

            await _productoController.Consultar();

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-InstanciaId inválido.")]
        public async Task ConsultarProductoServicioConInstanciaIdInvalido__RetornaMensajeInvalido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=2017730010016");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _productoController.Request.Headers.Add("piip-idAccion", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            HttpUtility.ParseQueryString(_productoController.Request.RequestUri.Query).Set("bpin", "2017730010016 ");

            await _productoController.Consultar();

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-InstanciaId no recibido.")]
        public async Task ConsultarProductoServicioConInstanciaIdVacio__RetornaMensajeNoRecibido()
        {
            _productoController.Request.RequestUri = new Uri("http://localhost:49686/api/Producto/?bpin=2017730010016");
            _productoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _productoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _productoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            _productoController.Request.Headers.Add("piip-InstanciaId", "");
            HttpUtility.ParseQueryString(_productoController.Request.RequestUri.Query).Set("bpin", "2017730010016 ");

            await _productoController.Consultar();

        }


        [TestMethod]
            public async Task ConsultarProductoServicioPeview__RetornaDto()
            {
                _productoController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                _productoController.User =
                    new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

                var result = (OkNegotiatedContentResult<ProyectoDto>) await _productoController.Preview();
                Assert.IsNotNull(result.Content);

            }



            [TestMethod]
            public async Task ConsultarProductoPreviewServicio__RetornaDto()
            {
                _productoController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                _productoController.User =
                    new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

                var result = (OkNegotiatedContentResult<ProyectoDto>) await _productoController.Preview();
                Assert.IsNotNull(result.Content);

            }


            [TestMethod]
            public async Task ConsultarProductoServicioPreview__RetornaTrue()
            {
                _productoController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                _productoController.User =
                    new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

                var result = (OkNegotiatedContentResult<ProyectoDto>) await _productoController.Preview();
                Assert.IsNotNull(result.Content);
            }


            [TestMethod]
            public async Task ProductoController_Temporal__RetornaTrue()
            {
                _productoController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                _productoController.User =
                    new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

                var productoDto = new ProyectoDto();

                _productoController.Request.Headers.Add("piip-idAplicacion", Guid.NewGuid().ToString());
                //_productoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
                //_productoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

                var result = (OkNegotiatedContentResult<bool>) await _productoController.Temporal(productoDto);
                Assert.IsNotNull(result.Content);
            }


            [TestMethod]
            public async Task ProductoController_Definitivo__RetornaTrue()
            {
                _productoController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                _productoController.User =
                    new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

                _productoController.Request.Headers.Add("piip-idAplicacion", Guid.NewGuid().ToString());
                var result = (OkNegotiatedContentResult<bool>) await _productoController.Definitivo(new ProyectoDto());
                Assert.IsNotNull(result.Content);
            }

            public void Dispose() { _productoController.Dispose(); }
        }
    
}