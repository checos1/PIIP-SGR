using System;
using DNP.ServiciosNegocio.Comunes.Enum;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales
{
   public  interface IAuditoriaServicios
    {
        void RegistrarErrorAuditoria(Exception exception, string ip, string usuario);
        void RegistrarTrazabilidadAuditoriaServiciosNegocio(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje);
    }
}
