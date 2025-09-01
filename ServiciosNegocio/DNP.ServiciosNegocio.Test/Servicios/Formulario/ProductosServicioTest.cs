namespace DNP.ServiciosNegocio.Test.Servicios.Formulario
{
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Configuracion;
    using Dominio.Dto.Proyectos;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiciosNegocio.Servicios.Interfaces.Formulario;
    using System;
    using System.Net.Http;
    using Unity;

    [TestClass]
    public class ProductosServicioTest
    {
        private IProductosServicio ProductosServicio { get;  set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            ProductosServicio = contenedor.Resolve<IProductosServicio>();
        }


        [TestMethod]
        public void ObtenerProductoTemporal___RetornaDto()
        {
            var parametrosConsultaProducto = new ParametrosConsultaDto() { Bpin = "2017005950118", InstanciaId = Guid.Parse("F27AFE2E-9031-4149-B9D9-063346991654"), AccionId = Guid.Parse("F27AFE2E-9031-4149-B9D9-063346991654") };
            var result = ProductosServicio.Obtener(parametrosConsultaProducto);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ObtenerProductoDefinitivo___RetornaDto()
        {
            var parametrosConsultaProducto = new ParametrosConsultaDto() { Bpin = "2017115950118", InstanciaId = Guid.Parse("F27AFE2E-9031-4149-B9D9-063346991654"), AccionId = Guid.Parse("F27AFE2E-9031-4149-B9D9-063346991654") };
            var result = ProductosServicio.Obtener(parametrosConsultaProducto);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerProductoPreview___RetornaDto()
        {

            var result = ProductosServicio.ObtenerProductosPreview();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoflujo no recibido.")]
        public void ProductosServicio_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            //Escenario: InstanciaId no enviado
            var contenido = new ProyectoDto();
            
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void ProductosServicio_Guardar_IdAccionNoEnviado_Excepcion()
        {
            //Escenario: AccionId no enviado
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void ProductosServicio_Guardar_ProductoDtoNoEnviado_Excepcion()
        {
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoflujo inválido")]
        public void ProductosServicio_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            //Escenario: InstanciaId inválido
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.Empty.ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void ProductosServicio_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            //Escenario: AccionId inválido
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void ProductosServicio_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<ProyectoDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new ProyectoDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            ProductosServicio.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }
    }
}