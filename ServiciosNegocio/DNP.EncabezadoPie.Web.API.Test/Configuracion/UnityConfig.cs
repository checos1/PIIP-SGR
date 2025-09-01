using Unity;
using DNP.EncabezadoPie.Servicios.Implementaciones;
using DNP.EncabezadoPie.Servicios.Interfaces.EncabezadoPieBasico;

namespace DNP.EncabezadoPie.Web.API.Test.Configuracion
{
    using Servicios.Interfaces.EncabezadoPieBasico;
    using System.Diagnostics.CodeAnalysis;
    using Servicios.Implementaciones;
    using DNP.EncabezadoPie.Persistencia.Implementaciones.EncabezadoPie;
    using DNP.EncabezadoPie.Persistencia.Interfaces.EncabezadoPie;
    using DNP.EncabezadoPie.Persistencia.Interfaces;
    using DNP.EncabezadoPie.Persistencia.Implementaciones;

    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer(); 
                container.RegisterType<IContextoFactory, ContextoFactory>();
                container.RegisterType<IEncabezadoPiePersistencia, EncabezadoPiePersistencia>();
                container.RegisterType<IEncabezadPieoBasicoServicio, EncabezadoPieServicio>();
                return container;
            }
        }
    }
}
