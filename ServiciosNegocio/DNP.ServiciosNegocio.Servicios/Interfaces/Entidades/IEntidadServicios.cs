using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Entidades
{
    public interface IEntidadServicios
    {
        #region EntidadBase

        void InsertarEntidadBase(EntidadBase entidadDto);

        void ActualizarEntidadBase(EntidadBase entidadDto);
        EntidadBase ConsultarEntidadBasePorId(int id);
        void BorrarEntidadBase(EntidadBase entidadDto);
        List<EntidadBase> ConsultarEntidadBaseTodos();
        #endregion
    }
}
