using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Administracion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Administracion
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class EjecutorServicio :  IEjecutorServicio
    {
        private readonly IEjecutorPersistencia _datosPersistencia;

        public EjecutorServicio(IEjecutorPersistencia datosPersistencia) 
        {
            _datosPersistencia = datosPersistencia;
        }

        public EjecutorDto ConsultarEjecutor(string Nit)
        {
            return _datosPersistencia.ConsultarEjecutor(Nit);
        }

        public bool GuardarEjecutor(EjecutorDto ObjDto)
        {
            return _datosPersistencia.GuardarEjecutor(ObjDto);
        }

    }
}
