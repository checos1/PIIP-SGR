namespace DNP.CargaArchivos.Web.API.Test.Configuracion
{
    using Servicios.Interfaces.CargaArchivo;
    using System.Diagnostics.CodeAnalysis;
    using Servicios.Implementaciones;
    using Unity;
    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();
                container.RegisterType<ICargaArchivo, CargaArchivo>();
                return container;
            }
        }
    }
}
