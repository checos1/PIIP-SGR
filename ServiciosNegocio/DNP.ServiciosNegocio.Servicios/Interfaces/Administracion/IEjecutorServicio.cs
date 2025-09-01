using DNP.ServiciosNegocio.Dominio.Dto.Administracion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Administracion
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IEjecutorServicio
    {
        EjecutorDto ConsultarEjecutor(string nit);
        bool GuardarEjecutor(EjecutorDto ObjDto);
    }
}
