namespace DNP.ServiciosWBS.Servicios.Interfaces.Transversales
{
    using ServiciosNegocio.Comunes.Enum;
    using System;

    public interface IAuditoriaServicios
    {
        void RegistrarErrorAuditoria(Exception exception, string ip, string usuario);
        void RegistrarTrazabilidadAuditoriaServiciosNegocio(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje);
    }
}