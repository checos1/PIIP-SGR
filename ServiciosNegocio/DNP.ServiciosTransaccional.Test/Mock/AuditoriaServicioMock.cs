namespace DNP.ServiciosTransaccional.Test.Mock
{
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosTransaccional.Servicios.Interfaces.Transversales;
    using System;
    public class AuditoriaServicioMock: IAuditoriaServicios
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