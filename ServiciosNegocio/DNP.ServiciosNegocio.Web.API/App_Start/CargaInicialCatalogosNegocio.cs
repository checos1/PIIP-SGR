namespace DNP.ServiciosNegocio.Web.API
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes.Enum;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Catalogos;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Catalogos;
    using Servicios.Implementaciones.Catalogos;
    using Servicios.Implementaciones.Transversales;
    using Servicios.Interfaces.Transversales;
    using Unity;
    using Unity.WebApi;

    public class CargaInicialCatalogosNegocio : IDisposable
    {

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {


            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                _unityContainer.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

        }

        private ICatalogoPersistencia CatalogoPersistencia { get; set; }
        private ICacheServicio CacheServicio { get; set; }
        private CatalogoServicio CatalogoServicio { get; set; }
        private IUnityContainer _unityContainer;
        public CargaInicialCatalogosNegocio()
        {
            ConfigurarUnity();

            CatalogoPersistencia = _unityContainer.Resolve<ICatalogoPersistencia>();
            CacheServicio = _unityContainer.Resolve<ICacheServicio>();
            CatalogoServicio = new CatalogoServicio(CatalogoPersistencia, CacheServicio);
        }

        public async Task IniciarCargaCache()
        {
            await CargarCatalogo(CatalogoEnum.Entidades.ToString());
            await CargarCatalogo(CatalogoEnum.TiposRecursos.ToString());
            await CargarCatalogo(CatalogoEnum.TiposEntidades.ToString());
            await CargarCatalogo(CatalogoEnum.Sectores.ToString());
            await CargarCatalogo(CatalogoEnum.Regiones.ToString());
            await CargarCatalogo(CatalogoEnum.Departamentos.ToString());
            await CargarCatalogo(CatalogoEnum.Municipios.ToString());
            await CargarCatalogo(CatalogoEnum.Resguardos.ToString());
            await CargarCatalogo(CatalogoEnum.Programas.ToString());
            await CargarCatalogo(CatalogoEnum.Productos.ToString());
            await CargarCatalogo(CatalogoEnum.Alternativas.ToString());
            await CargarCatalogo(CatalogoEnum.ClasificacionesRecursos.ToString());
            await CargarCatalogo(CatalogoEnum.Etapas.ToString());
            await CargarCatalogo(CatalogoEnum.TiposAgrupaciones.ToString());
            await CargarCatalogo(CatalogoEnum.Agrupaciones.ToString());
            await CargarCatalogo(CatalogoEnum.GruposRecursos.ToString());
            await CargarCatalogo(CatalogoEnum.Politicas.ToString());
            await CargarCatalogo(CatalogoEnum.PoliticasNivel1.ToString());
            await CargarCatalogo(CatalogoEnum.PoliticasNivel2.ToString());
            await CargarCatalogo(CatalogoEnum.PoliticasNivel3.ToString());
            await CargarCatalogo(CatalogoEnum.IndicadoresPoliticas.ToString());
            await CargarCatalogo(CatalogoEnum.TiposCofinanciaciones.ToString());
            await CargarCatalogo(CatalogoEnum.Entregables.ToString());
        }

        private async Task CargarCatalogo(string nombreCatalogo)
        {
            try
            {
                var usuario = ConfigurationManager.AppSettings["UsuarioGenericoServiciosNegocio"];
                var autorizacion = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{usuario}:1234"));
                //var tokenAutorizacion = new AuthenticationHeaderValue("Basic", autorizacion);

                await CatalogoServicio.ObtenerCatalogo(nombreCatalogo, autorizacion);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        [ExcludeFromCodeCoverage]
        private void ConfigurarUnity()
        {
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<ICatalogoPersistencia, CatalogoPersistencia>();
            _unityContainer.RegisterType<ICacheServicio, CacheServicio>();
            _unityContainer.RegisterType<IContextoFactory, ContextoFactory>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(_unityContainer);

        }
    }
}