
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IEjecutorPersistencia
    {
        EjecutorDto  ConsultarEjecutor(string Nit);
        bool GuardarEjecutor(EjecutorDto ObjDto);
    }


}
