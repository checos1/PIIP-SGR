using System;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using Comunes.Enum;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    class AuditoriaServiciosMock : IAuditoriaServicios
    {
        public void RegistrarErrorAuditoria(Exception exception, string ip, string usuario)
        {
        }
        public void RegistrarTrazabilidadAuditoriaServiciosNegocio(object entity, string ip, string usuario,
                                                                   TipoMensajeEnum tipoMensaje)
        {
        }
    }
}
