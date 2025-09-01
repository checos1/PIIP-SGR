using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Test.Configuracion;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Test.Servicios.Formulario
{



    [TestClass]
    public class CadenaValorServicioTest
    {
        private ICadenaValorPersistencia CadenaValorPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private CadenaValorServicios CadenaValorServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CadenaValorPersistencia = contenedor.Resolve<ICadenaValorPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CadenaValorServicio = new CadenaValorServicios(CadenaValorPersistencia, PersistenciaTemporal, AuditoriaServicio);
        }


        [TestMethod]
        public void CadenaValorPreview()
        {
            var result = CadenaValorServicio.ObtenerCadenaValorPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _cadenaValorServicio_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            //Escenario: InstanciaId no enviado
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void _cadenaValorServicio_Guardar_IdAccionNoEnviado_Excepcion()
        {
            //Escenario: AccionId no enviado
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void _cadenaValorServicio_Guardar_CadenaValorDtoNoEnviado_Excepcion()
        {
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void _cadenaValorServicio_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            //Escenario: InstanciaId inválido
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void _cadenaValorServicio_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            //Escenario: AccionId inválido
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void CadenaValor_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<CadenaValorDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new CadenaValorDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            CadenaValorServicio.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }

        [TestMethod]
        public void CadenaValor_ObtenerDefinitivo()
        {
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017002700002"
            };

            var resultado = CadenaValorServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CadenaValor_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<CadenaValorDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new CadenaValorDto()
                {
                    Vigencias = new List<VigenciaDto>()
                                                                    {
                                                                       new VigenciaDto() {
                                                                           Vigencia = 2017,
                                                                            GranTotalPorVigencia                 = 100,
                                                                                             Fuente = null,
                                                                                             ProblemaCentral = null,
                                                                                             ObjetivosEspecificos = new List<ObjetivoEspecificoCadenaValorDto>()
                                                                                                                    {
                                                                                                                        new ObjetivoEspecificoCadenaValorDto()
                                                                                                                        {
                                                                                                                            Id = 1,
                                                                                                                            ObjetivoEspecifico = "objetivo especifico",
                                                                                                                            Productos = new List<ProductoCadenaValorDto>()
                                                                                                                                        {
                                                                                                                                            new ProductoCadenaValorDto()
                                                                                                                                            {
                                                                                                                                                Id = 1,
                                                                                                                                                Producto = "administracion",
                                                                                                                                                Etapas = new List<EtapaDto>()
                                                                                                                                                         {
                                                                                                                                                             new EtapaDto()
                                                                                                                                                             {
                                                                                                                                                                 Id = 1,
                                                                                                                                                                 TotalEtapaApropiacionVigente = 100,
                                                                                                                                                                 TotalEtapaValorSolicitado = 100,
                                                                                                                                                                 TotalEtapaApropiacionInicial = 200,
                                                                                                                                                                 NombreEtapa = "etapa nombre",
                                                                                                                                                                 Actividades = new List<ActividadDto>()
                                                                                                                                                                               {
                                                                                                                                                                                   new ActividadDto()
                                                                                                                                                                                   {
                                                                                                                                                                                       Id = 1,
                                                                                                                                                                                       ValorSolicitado = 100,
                                                                                                                                                                                       ApropiacionInicial = 100,
                                                                                                                                                                                       ApropiacionVigente = 200,
                                                                                                                                                                                       IdActividadPorInsumo = 1,
                                                                                                                                                                                       IdInsumo = 1,
                                                                                                                                                                                       Nombre = "actividad",
                                                                                                                                                                                       NombreInsumo = "insumo",
                                                                                                                                                                                       Ejecuciones = new List<EjecucionDto>()
                                                                                                                                                                                                     {
                                                                                                                                                                                                         new EjecucionDto()
                                                                                                                                                                                                         {
                                                                                                                                                                                                             Mes = 1,
                                                                                                                                                                                                             Compromiso = 1,
                                                                                                                                                                                                             Obligacion = 1,
                                                                                                                                                                                                             Pago = 1,
                                                                                                                                                                                                             ApropiacionInicialMes = 1,
                                                                                                                                                                                                             ApropiacionVigenteMes = 1,
                                                                                                                                                                                                             GrupoRecurso = "grupoRecurso",
                                                                                                                                                                                                             IdGrupoRecurso = 1
                                                                                                                                                                                                         }
                                                                                                                                                                                                     }
                                                                                                                                                                                   }
                                                                                                                                                                               }
                                                                                                                                                             }
                                                                                                                                                         }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                        }
                                                                                                                    }
                                                                        }
                                                                    },
                    Bpin = "2017002700002",
                    ProyectoId = 1234
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };
            CadenaValorServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
    }
}
