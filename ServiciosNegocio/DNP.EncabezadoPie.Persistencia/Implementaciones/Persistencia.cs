using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DNP.EncabezadoPie.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.EncabezadoPie.Persistencia.Modelo;

namespace DNP.EncabezadoPie.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class Persistencia
    {

        private readonly IContextoFactory _contextoFactory;
        private MGAWebContextoEncabezado _contexto;

        protected Persistencia(IContextoFactory contextoFactory)
        {
            _contextoFactory = contextoFactory;
        }
        public MGAWebContextoEncabezado Contexto
        {
            get => _contexto ?? (_contexto = _contextoFactory.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString));
            set => _contexto = value;
        }

        public void GuardarCambios()
        {
            try
            {
                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new AccionException(ServiciosNegocio.Comunes.Properties.Resources.ErrorPersistencia, ex);
            }
        }

        public async Task GuardarCambiosAsyn()
        {
            await Contexto.SaveChangesAsync();
        }

    }
}
