using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion
{
    public interface IMensajeNotificacionServicio
    {
        Task<IEnumerable<UsuarioNotificacionDto>> OtenerMensajeNotificaciones(UsuarioNotificacionMensajesFiltroDto filtro, string usuarioLogado);
    }
}
