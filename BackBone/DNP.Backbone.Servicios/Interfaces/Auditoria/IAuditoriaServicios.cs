namespace DNP.Backbone.Servicios.Interfaces.Auditoria
{
    using System;

    public interface IAuditoriaServicios
    {
        void RegistrarErrorAuditoria(Exception exception, string ip, string usuario);
    }
}
