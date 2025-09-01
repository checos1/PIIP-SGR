using System;

namespace DNP.ServiciosNegocio.Comunes.Interfaces
{
    public interface IAccionUtilidades
    {
        bool ExisteInstancia(Guid idInstancia, string idUsuario);
        bool ExisteAccionActiva(Guid idAccion, string idUsuario);
    }
    
}
