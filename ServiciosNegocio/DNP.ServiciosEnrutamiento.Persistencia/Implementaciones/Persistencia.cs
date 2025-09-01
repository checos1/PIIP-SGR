using DNP.ServiciosEnrutamiento.Persistencia.Interfaces;
using DNP.ServiciosEnrutamiento.Persistencia.Modelo;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia
{
    [ExcludeFromCodeCoverage]
    public class Persistencia
    {
        private readonly IContextoFactory _contextoFactory;
        private MGAWebContexto _contexto;

        protected Persistencia(IContextoFactory contextoFactory)
        {
            _contextoFactory = contextoFactory;
        }
        public MGAWebContexto Contexto
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
