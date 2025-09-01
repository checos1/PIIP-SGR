using System.Collections.Generic;
using DNP.ServiciosNegocio.Servicios.Interfaces.Entidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Entidades;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades
{
    public class OpcionCatalogoTipoEntidadServicios : IEntidadServicios
    {
        #region Objetos

        private readonly IEntidadPersistencia _entidadPersistencia;

        #endregion

        #region Inicializacion 

        public OpcionCatalogoTipoEntidadServicios(IEntidadPersistencia entidadPersistencia)
        {
            _entidadPersistencia = entidadPersistencia;
        }

        #endregion

        #region Metodos OpcionCatalogoTipoEntidadDto
        public void InsertarEntidadBase(EntidadBase entidadDto)
        {
            _entidadPersistencia.InsertarOpcionCatalogoTipoEntidad((OpcionCatalogoTipoEntidadDto)entidadDto);
        }
        
        public void ActualizarEntidadBase(EntidadBase entidadDto)
        {
            _entidadPersistencia.ActualizarOpcionCatalogoTipoEntidad((OpcionCatalogoTipoEntidadDto)entidadDto);
        }

        public EntidadBase ConsultarEntidadBasePorId(int id)
        {
            return _entidadPersistencia.ConsultarOpcionCatalogoTipoEntidadPorId(id);
        }

        public void BorrarEntidadBase(EntidadBase entidadDto)
        {
            _entidadPersistencia.BorrarOpcionCatalogoTipoEntidad((OpcionCatalogoTipoEntidadDto)entidadDto);
        }

        public List<EntidadBase> ConsultarEntidadBaseTodos()
        {
            return new List<EntidadBase>(_entidadPersistencia.ConsultarOpcionCatalogoTipoEntidadTodos());
        }
        #endregion

    }
}
