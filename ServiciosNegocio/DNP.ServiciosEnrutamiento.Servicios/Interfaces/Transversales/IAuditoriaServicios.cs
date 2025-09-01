using DNP.ServiciosNegocio.Comunes.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Servicios.Interfaces.Transversales
{
    public interface IAuditoriaServicios
    {
        void RegistrarErrorAuditoria(Exception exception, string ip, string usuario);
        void RegistrarTrazabilidadAuditoriaServiciosEnrutamiento(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje);

    }
}
