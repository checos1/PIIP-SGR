using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DNP.ServiciosTransaccional.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosTransaccional.Persistencia.Modelo;
using DNP.ServiciosTransaccional.Persistencia.ModeloSGR;

namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class PersistenciaSGR
    {

        private readonly IContextoFactorySGR _contextoFactory;
        private MGAWebContextoSGR _contexto;

        protected PersistenciaSGR(IContextoFactorySGR contextoFactory)
        {
            _contextoFactory = contextoFactory;
        }
        public MGAWebContextoSGR Contexto
        {
            get => _contexto ?? (_contexto = _contextoFactory.CrearContextoConConexionSGR(ConfigurationManager.ConnectionStrings["MGAWebContextoSGR"].ConnectionString));
            set => _contexto = value;
        }

        public void GuardarCambiosSGR()
        {
            try
            {
                Contexto.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new AccionException(ServiciosNegocio.Comunes.Properties.Resources.ErrorPersistencia, ex);
            }
        }

        public async Task GuardarCambiosSGRAsyn()
        {
            await Contexto.SaveChangesAsync();
        }

    }
}
