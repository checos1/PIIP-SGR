namespace DNP.ServiciosWBS.Test.Mocks
{
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using System;

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
