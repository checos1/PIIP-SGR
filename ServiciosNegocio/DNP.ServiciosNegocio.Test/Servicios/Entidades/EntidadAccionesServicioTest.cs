using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.ServiciosNegocio.Test.Servicios.Entidades
{
    using System;
    using System.Collections.Generic;
    using Configuracion;
    using Dominio.Dto.Entidades;
    using ServiciosNegocio.Servicios.Interfaces.Entidades;
    using Unity;

    [TestClass]
    public class EntidadAccionesServicioTest
    {
        private IEntidadAccionesServicio _entidadAccionesServicio;
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
            var contenedor = UnityConfig.Container;
            _entidadAccionesServicio = contenedor.Resolve<IEntidadAccionesServicio>();
        }
        [TestMethod]
        public void ObtenerEntidadAccionesServicioExitosoTest()
        {
            var result = _entidadAccionesServicio.ObtenerEntidadesAcciones(ObjetoEntrada);
            Assert.IsTrue(result.ListadoEntidadDestino.Count > 0);
        }
        [TestMethod]
        public void ObtenerEntidadAccionesServicioNoExitosoTest()
        {
            ObjetoEntrada.Bpin = string.Empty;
            var result = _entidadAccionesServicio.ObtenerEntidadesAcciones(ObjetoEntrada);
            Assert.IsFalse(result.ListadoEntidadDestino.Count > 0);
        }
    }
}