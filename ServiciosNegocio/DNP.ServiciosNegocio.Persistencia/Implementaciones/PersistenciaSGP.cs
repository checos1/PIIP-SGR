using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class PersistenciaSGP
    {

        private readonly IContextoFactory _contextoFactory;        
        private ModelOnlySPEntities _contextoOnlySP;
        private readonly IContextoFactorySGR _contextoFactorySGR;
        private MGAWebContextoSGR _contextoSGR;

        protected PersistenciaSGP(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR)
        {
            _contextoFactory = contextoFactory;
            _contextoFactorySGR = contextoFactorySGR;
        }
        public ModelOnlySPEntities ContextoOnlySP
        {
            get => _contextoOnlySP ?? (_contextoOnlySP = _contextoFactory.CrearContextoConConexionOnlySP(ConfigurationManager.ConnectionStrings["ModelOnlySPEntities"].ConnectionString));
            set => _contextoOnlySP = value;
        }
        public MGAWebContextoSGR ContextoSGR
        {
            get => _contextoSGR ?? (_contextoSGR = _contextoFactorySGR.CrearContextoConConexionSGR(ConfigurationManager.ConnectionStrings["MGAWebContextoSGR"].ConnectionString));
            set => _contextoSGR = value;
        }

    }
}
