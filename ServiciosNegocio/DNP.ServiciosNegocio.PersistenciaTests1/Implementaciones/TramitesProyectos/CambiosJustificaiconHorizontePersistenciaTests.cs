using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Persistencia.Interfaces;


namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos.Tests
{
	
	[TestClass()]
	public class CambiosJustificaiconHorizontePersistenciaTests
	{
		private readonly IContextoFactory _contextoFactory;
		private MGAWebContexto _contexto;

		protected CambiosJustificaiconHorizontePersistenciaTests(IContextoFactory contextoFactory)
		{
			_contextoFactory = contextoFactory;
		}
		//public MGAWebContexto Contexto
		//{

		//	//get => _contexto ?? (_contexto = _contextoFactory.CrearContextoConConexion(System.Configuration.ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString));
		//	//set => _contexto = value;
		//}

		[TestMethod()]
		public void ObtenerCambiosJustificacionHorizonteTest()
		{
			//var resultSp = Contexto.upsGetEstadoProyectoHorizonte(IdProyecto);
		}
	}
}