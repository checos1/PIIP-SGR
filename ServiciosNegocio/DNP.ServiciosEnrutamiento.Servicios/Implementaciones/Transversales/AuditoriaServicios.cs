using DNP.ServiciosEnrutamiento.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Auditoria;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Auditoria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Servicios.Implementaciones.Transversales
{
    public class AuditoriaServicios: IAuditoriaServicios
    {
        public void RegistrarErrorAuditoria(Exception exception, string ip, string usuario)
        {
            var evento = Enum.GetName(typeof(TipoEventoEnum), TipoEventoEnum.Sistema);
            var mensaje = Enum.GetName(typeof(TipoMensajeEnum), TipoMensajeEnum.Error);
            var auditoria = new AuditoriaDto
            {
                Ip = "::1".Equals(ip) ? "127.0.0.1" : ip,
                //TO-DO CAmbiar el id de aplicacion en el archivo de configuracion
                Aplicacion = ConfigurationManager.AppSettings["IdAplicacionServicioEnrutamiento"],
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

        public void RegistrarTrazabilidadAuditoriaServiciosEnrutamiento(object entity, string ip, string usuario, TipoMensajeEnum tipoMensaje)
        {
            RegistrarTrazabilidadAuditoria(entity, ip, usuario, tipoMensaje, ConfigurationManager.AppSettings["IdAplicacionServicioEnrutamiento"]);
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
        }

    }
}
