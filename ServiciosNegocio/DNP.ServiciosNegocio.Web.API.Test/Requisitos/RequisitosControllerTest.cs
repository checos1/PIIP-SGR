using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;

namespace DNP.ServiciosNegocio.Web.API.Test.Requisitos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http;
    using Controllers.Requisitos;
    using Dominio.Dto.Requisitos;
    using Servicios.Interfaces.Requisitos;

    [TestClass]
    public class RequisitosControllerTest : IDisposable
    {

        private IRequisitosServicio _requisitosServicio;
        private IAutorizacionUtilidades _autorizacionUtilizades;
        private RequisitosController _requisitosController;
        private IUnityContainer _container;

        [TestInitialize]
        public void Init()
        {
            _container = Configuracion.UnityConfig.Container;
            _requisitosServicio = _container.Resolve<IRequisitosServicio>();
            _autorizacionUtilizades = _container.Resolve<IAutorizacionUtilidades>();
            _requisitosController = new RequisitosController(_requisitosServicio, _autorizacionUtilizades);
            _requisitosController.ControllerContext.Request = new HttpRequestMessage();
            _requisitosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _requisitosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _requisitosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "0bac70cd-f17f-4b24-bf91-78afd37d514e");
            _requisitosController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarRequisitos_NoNulo()
        {
            _requisitosController.Request.RequestUri = new Uri("http://localhost:49686/api/Requisitos?bpin=2017184790011&IdNivel=332A0F8B-84E7-419F-B262-D449365167F7");
            var result =(OkNegotiatedContentResult<ServicioAgregarRequisitosDto>) await _requisitosController.Consultar();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ConsultarRequisitos_ExcepcionGuid()
        {
            _requisitosController.Request.RequestUri = new Uri("http://localhost:49686/api/Requisitos?bpin=2017184790011&IdNivel=");
            await _requisitosController.Consultar();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ConsultarRequisitos_ExcepcionBpin()
        {
            _requisitosController.Request.RequestUri = new Uri("http://localhost:49686/api/Requisitos?bpin=&IdNivel=332A0F8B-84E7-419F-B262-D449365167F7");
            await _requisitosController.Consultar();
        }

        [TestMethod]
        public async Task RequisitosController_Definitivo__RetornaTrue()
        {
            _requisitosController.ControllerContext.Request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

            _requisitosController.User =
                new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var contenido = new ServicioAgregarRequisitosDto();
            contenido.Bpin = "2017184790011";
            contenido.IdNivel = Guid.NewGuid();
            contenido.ListadoAtributos = new List<Atributo>()
                                         {
                                             new Atributo()
                                             {
                                                 Nombre= "Sector",
                                                 IdValor= 33,
                                                 Valor= "Cultura",
                                                 AgregadoPorRequisito= false
                                             },
                                             new Atributo()
                                             {
                                                 Nombre= "Programa",
                                                 IdValor= 1127,
                                                 Valor= "3399  - Fortalecimiento de la gestión y dirección del Sector Cultura",
                                                 AgregadoPorRequisito= false
                                             }
                                         };

            var result = (OkNegotiatedContentResult<bool>)await _requisitosController.Definitivo(contenido);
            Assert.IsNotNull(result.Content);
        }

        [SuppressMessage("ReSharper", "UseNullPropagation")]
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_requisitosController != null)
                _requisitosController.Dispose();
            _container?.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
