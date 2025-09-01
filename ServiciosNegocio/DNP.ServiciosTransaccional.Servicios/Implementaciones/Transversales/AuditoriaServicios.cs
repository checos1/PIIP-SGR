using System;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Auditoria;
using System.Configuration;
using DNP.ServiciosNegocio.Comunes.Auditoria;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Transversales
{
    public class AuditoriaServicios : IAuditoriaServicios
    {
        public void RegistrarErrorAuditoria(Exception exception, string ip, string usuario)
        {
            var evento = Enum.GetName(typeof(TipoEventoEnum), TipoEventoEnum.Sistema);
            var mensaje = Enum.GetName(typeof(TipoMensajeEnum), TipoMensajeEnum.Error);
            var auditoria = new AuditoriaDto
            {
                Ip = "::1".Equals(ip) ? "127.0.0.1" : ip,
                //TO-DO CAmbiar el id de aplicacion en el archivo de configuracion
                Aplicacion = ConfigurationManager.AppSettings["IdAplicacionServicioNegocio"], 
                Usuario = usuario,
                TipoEvento = evento?.ToUpper(),
                TipoMensaje = mensaje?.ToUpper(),
                ContenidoMensaje = @"{ ""mensaje"": ""[[contenidoMensaje]]"" }",
                EntidadOrigen = null
            };
            auditoria.ContenidoMensaje = auditoria.ContenidoMensaje.Replace("[[contenidoMensaje]]", exception.Message + " " + exception.StackTrace);
            auditoria.ContenidoMensaje = auditoria.ContenidoMensaje.Replace("\\", "/");
            Task.Run(() => AuditoriaUtilidades.EnviarAuditoria(auditoria));
        }

        public void RegistrarTrazabilidadAuditoriaServiciosNegocio(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje)
        {
            RegistrarTrazabilidadAuditoria(entity, ip, usuario, tipoMensaje, ConfigurationManager.AppSettings["IdAplicacionServicioNegocio"]);
        }

        private void RegistrarTrazabilidadAuditoria(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje, string aplicacion)
        {
            var evento = Enum.GetName(typeof(TipoEventoEnum), TipoEventoEnum.Trazabilidad);
            var mensaje = Enum.GetName(typeof(TipoMensajeEnum), tipoMensaje);

            if (evento == null || mensaje == null) return;

            var auditoria = new AuditoriaDto
                            {
                                Ip = "::1".Equals(ip) ? "127.0.0.1" : ip,
                                Aplicacion = aplicacion,
                                Usuario = usuario,
                                TipoEvento = evento.ToUpper(),
                                TipoMensaje = mensaje.ToUpper(),
                                ContenidoMensaje = "{ " + entity + " }",
                                EntidadOrigen = entity.GetType().ToString()
                            };
            Task.Run(() => AuditoriaUtilidades.EnviarAuditoria(auditoria));
            // AuditoriaUtilidades.EnviarAditoria(auditoria);
        }
    }
}
