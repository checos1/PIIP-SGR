using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    
    public class FuentesAprobacionControllerTest
    {

        private IFuentesAprobacionServicio _fuentesAprobacionServicio;
        private IAutorizacionServicios _autorizacionUtilidades;

        [TestInitialize]
        public void Init()
        {
            _fuentesAprobacionServicio  = Config.UnityConfig.Container.Resolve<IFuentesAprobacionServicio>(); 
            _autorizacionUtilidades =  Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
        }

        [TestMethod]
        public void ObtenerPreguntasAprobacionRol()
        {
            PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
            objPreguntasSeguimientoProyectoDto.nivelId = new Guid();
            objPreguntasSeguimientoProyectoDto.tipoTramiteId = 0;
            objPreguntasSeguimientoProyectoDto.proyectoId = 0;
            Guid guid = new Guid();

            var actionResult = _fuentesAprobacionServicio.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto,"CC505050", guid.ToString());
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
        
        [TestMethod]
        public void GuardarPreguntasAprobacionRol()
        {
            PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
            objPreguntasSeguimientoProyectoDto.nivelId = new Guid();
            objPreguntasSeguimientoProyectoDto.tipoTramiteId = 0;
            objPreguntasSeguimientoProyectoDto.proyectoId = 0;

            Guid guid = new Guid();

            var actionResult = _fuentesAprobacionServicio.GuardarPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto,"CC505050", guid.ToString());
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }

        [TestMethod]
        public void ObtenerPreguntasAprobacionJefe()
        {
            PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
            objPreguntasSeguimientoProyectoDto.nivelId = new Guid();
            objPreguntasSeguimientoProyectoDto.tipoTramiteId = 0;
            objPreguntasSeguimientoProyectoDto.proyectoId = 0;
            Guid guid = new Guid();

            var actionResult = _fuentesAprobacionServicio.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, "CC505050", guid.ToString());
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }

        [TestMethod]
        public void GuardarPreguntasAprobacionJefe()
        {
            PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
            objPreguntasSeguimientoProyectoDto.nivelId = new Guid();
            objPreguntasSeguimientoProyectoDto.tipoTramiteId = 0;
            objPreguntasSeguimientoProyectoDto.proyectoId = 0;
            Guid guid = new Guid();

            var actionResult = _fuentesAprobacionServicio.GuardarPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, "CC505050", guid.ToString());
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
    }
}
