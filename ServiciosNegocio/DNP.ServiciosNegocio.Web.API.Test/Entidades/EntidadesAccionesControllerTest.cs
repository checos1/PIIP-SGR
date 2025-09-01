using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;

namespace DNP.ServiciosNegocio.Web.API.Test.Entidades
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http;
    using Controllers.Entidades;
    using Dominio.Dto.Entidades;
    using Servicios.Interfaces.Entidades;

    [TestClass]
    public class EntidadesAccionesControllerTest : IDisposable
    {

        private IEntidadAccionesServicio _entidadAccionesServicio;
        private IAutorizacionUtilidades _autorizacionUtilizades;
        private EntidadesAccionesController _entidadesAccionesController;
        private IUnityContainer _container;
        private EntidadAccionesEntrada ObjetoEntrada { get; set; }
        private List<RolDto> ListaRol { get; set; }

        [TestInitialize]
        public void Init()
        {
            ObjetoEntrada = new EntidadAccionesEntrada();
            ListaRol = new List<RolDto>()
                       {
                           new RolDto()
                           {
                               IdRol = Guid.Parse("6F7C8930-6962-4E6A-9FB4-E4F7CA0DDAC3"),
                               NombreRol = "Viabilizador"
                           },
                           new RolDto()
                           {
                               IdRol = Guid.Parse("56828712-69C6-4D7C-9169-8D7BD18854CC"),
                               NombreRol = "Formulador"
                           }
                       };

            ObjetoEntrada.Bpin = "2017761220010";
            ObjetoEntrada.ListadoRoles = ListaRol;

            _container = Configuracion.UnityConfig.Container;
            _entidadAccionesServicio = _container.Resolve<IEntidadAccionesServicio>();
            _autorizacionUtilizades = _container.Resolve<IAutorizacionUtilidades>();

            _entidadesAccionesController =
                new EntidadesAccionesController(_entidadAccionesServicio, _autorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _entidadesAccionesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _entidadesAccionesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _entidadesAccionesController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarEntidadesAcciones_NoNulo()
        {
            var result = (OkNegotiatedContentResult<EntidadAcciones>)await _entidadesAccionesController.Definitivo(ObjetoEntrada);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ConsultarEntidadesAccionesTest_FalloValidacionInternaContenido_NoParametrosRecibidos()
        {
            try
            {
                _entidadesAccionesController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _entidadesAccionesController.Definitivo(new EntidadAccionesEntrada());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }
        [TestMethod]
        public async Task ConsultarEntidadesAccionesTest_FalloValidacionInternaContenido_NoBpin()
        {
            try
            {
                _entidadesAccionesController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                ObjetoEntrada.Bpin = string.Empty;

                await _entidadesAccionesController.Definitivo(ObjetoEntrada);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }
        [TestMethod]
        public async Task ConsultarEntidadesAccionesTest_FalloValidacionInternaContenido_NoListaRoles()
        {
            try
            {
                _entidadesAccionesController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                ObjetoEntrada.ListadoRoles = null;

                await _entidadesAccionesController.Definitivo(ObjetoEntrada);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        [SuppressMessage("ReSharper", "UseNullPropagation")]
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_entidadesAccionesController != null)
                _entidadesAccionesController.Dispose();
            _container?.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
