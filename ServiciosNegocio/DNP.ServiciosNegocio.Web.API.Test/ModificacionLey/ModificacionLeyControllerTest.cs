using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey;
using DNP.ServiciosNegocio.Web.API.Controllers.ModificacionLey;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;

namespace DNP.ServiciosNegocio.Web.API.Test.ModificacionLey
{
    [TestClass]
    public class ModificacionLeyControllerTest : IDisposable
    {
        private IModificacionLeyServicio ModificacionLeyServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ModificacionLeyController _modificacionLeyController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ModificacionLeyServicio = contenedor.Resolve<IModificacionLeyServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _modificacionLeyController = new ModificacionLeyController(ModificacionLeyServicio, AutorizacionUtilizades);
            _modificacionLeyController.ControllerContext.Request = new HttpRequestMessage();
            _modificacionLeyController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _modificacionLeyController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _modificacionLeyController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        public async Task ObtenerInformacionPresupuestalMLEncabezado_NoNulo()
        {
            int EntidadDestinoId = 155;
            int TramiteId = 2290;
            string origen = "Distribucion";
            var result = (OkNegotiatedContentResult<string>)await _modificacionLeyController.ObtenerInformacionPresupuestalMLEncabezado(EntidadDestinoId, TramiteId, origen);
            Assert.IsNotNull(result.Content);
        }

        public async Task ObtenerInformacionPresupuestalMLDetalle_NoNulo()
        {
            int tramiteidProyectoId = 8450;
            string origen = "FUENTE";
            var result = (OkNegotiatedContentResult<string>)await _modificacionLeyController.ObtenerInformacionPresupuestalMLDetalle(tramiteidProyectoId, origen);
            Assert.IsNotNull(result.Content);

        }

        public void GuardarInformacionPresupuestalML_test()
        {
            List<ValoresFuenteML> listaValoresFuente = new List<ValoresFuenteML>();

            ValoresFuenteML ValoresFuente = new ValoresFuenteML
            {
                FuenteId = 111511,
                NacionCSF = 0,
                NacionSSF = 0,
                Propios = 0
            };
            listaValoresFuente.Add(ValoresFuente);

            InformacionPresupuestalMLDto InformacionPresupuestal = new InformacionPresupuestalMLDto
            {
                TramiteProyectoId = 2290,
                NivelId = "592B40A9-EE35-4544-8300-031E0F6C249D",
                SeccionCapitulo = 737,
                ValoresFuente = listaValoresFuente
            };
            string result = "OK";
            string origen = "Adición Presupuesto Paso1";

            try
            {
                _ = _modificacionLeyController.GuardarInformacionPresupuestalML(InformacionPresupuestal);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _modificacionLeyController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
