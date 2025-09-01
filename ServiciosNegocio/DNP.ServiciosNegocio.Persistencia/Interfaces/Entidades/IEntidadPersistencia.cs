using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Entidades
{
    public interface IEntidadPersistencia
    {
        #region Inicializacion
        void GuardarCambios();
        #endregion

        #region EntidadBase

        void InsertarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto);

        void ActualizarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto);
        OpcionCatalogoTipoEntidadDto ConsultarOpcionCatalogoTipoEntidadPorId(int id);
        List<OpcionCatalogoTipoEntidadDto> ConsultarOpcionCatalogoTipoEntidadTodos();
        void BorrarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto);

        #endregion
    }
}
