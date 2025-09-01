namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Beneficiarios;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;

    [TestClass]
    public class ProyectoControllerTest
    {
        private IProyectoServicios _proyectoServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private ProyectoController _proyectoController;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IFlujoServicios _flujoServicios;

        [TestInitialize]
        public void Init()
        {
            _proyectoServicios = Config.UnityConfig.Container.Resolve<IProyectoServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();

            _proyectoController = new ProyectoController(_proyectoServicios, _autorizacionServicios, _serviciosNegocioServicios, _flujoServicios);
            _proyectoController.ControllerContext.Request = new HttpRequestMessage();
            _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _proyectoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _proyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }


        [TestMethod]
        public void ObtenerProyecto_Ok()
        {
            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            var actionResult = _proyectoController.ObtenerProyectos(instancia).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<Dominio.Dto.Proyecto.ProyectoDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public async Task ObtenerContracredito_Ok()
        {
            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                        new List<Guid>()
                        {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                        }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };

            var result = await _proyectoController.ObtenerContracredito(new ProyectoCreditoParametroDto
            {
                TipoEntidad = "Nacional",
                IdEntidad = 41,
                IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"),
                BPIN = "2017011000332",
                NombreProyecto = null
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull((result as OkNegotiatedContentResult<IEnumerable<ProyectoCreditoDto>>).Content);
        }

        [TestMethod]
        public async Task ObtenerCredito_Ok()
        {
            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                        new List<Guid>()
                        {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                        }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };

            var result = await _proyectoController.ObtenerCredito(new ProyectoCreditoParametroDto
            {
                TipoEntidad = "Nacional",
                IdEntidad = 41,
                IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"),
                BPIN = "2017011000332",
                NombreProyecto = null
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull((result as OkNegotiatedContentResult<IEnumerable<ProyectoCreditoDto>>).Content);
        }

        [TestMethod]
        public void GuardarBeneficiarioTotales_Test()
        {
            BeneficiarioTotalesDto beneficiarioTotalesDto = new BeneficiarioTotalesDto();

            beneficiarioTotalesDto.BPIN = "202200000000110";
            beneficiarioTotalesDto.NumeroPersonalAjuste = 1;
            beneficiarioTotalesDto.ProyectoId = 4;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioTotales(beneficiarioTotalesDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProducto_Test()
        {
            BeneficiarioProductoDto beneficiarioDto = new BeneficiarioProductoDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.InterventionLocalizationTypeId = 1;
            beneficiarioDto.PersonasBeneficiaros = 1;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioProducto(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacion_Test()
        {
            BeneficiarioProductoLocalizacionDto beneficiarioDto = new BeneficiarioProductoLocalizacionDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.LocalizacionId = 1;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioProductoLocalizacion(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacionCaracterizacion_Test()
        {
            BeneficiarioProductoLocalizacionCaracterizacionDto beneficiarioDto = new BeneficiarioProductoLocalizacionCaracterizacionDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.LocalizacionId = 1;
            beneficiarioDto.Vigencia = DateTime.Now.Year;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioProductoLocalizacionCaracterizacion(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerTokenMGA_Ok()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "token");

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_usuarioLogeado_Null()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto();

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "token");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_bpin_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("", usuarioLogeado, "externo", "token");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_token_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_tipoUsuario_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "", "token");

            Assert.IsNull(actionResult);
        }
    }
}
