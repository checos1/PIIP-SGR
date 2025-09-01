namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transferencias
{
    using System;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Comunes.Enum;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
    using Dominio.Dto.Auditoria;
    using Interfaces.Transferencias;
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using Persistencia.Interfaces.Transferencias;

    public class TransferenciaServicio : ITransferenciaServicio
    {
        private readonly ITransferenciaPersistencia _transferenciaPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public TransferenciaServicio(ITransferenciaPersistencia transferenciaPersistencia, IAuditoriaServicios auditoriaServicios)
        {
            _transferenciaPersistencia = transferenciaPersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public TransferenciaEntidadDto IdentificarEntidadDestino(int proyectoId, int entidadTransfiereId, ParametrosAuditoriaDto parametrosAuditoriaDto)
        {
            var resultado = _transferenciaPersistencia.IdentificarEntidadDestino(proyectoId, entidadTransfiereId);
            GenerarAuditoriaTransferencia(resultado, parametrosAuditoriaDto);
            return resultado;
        }

        private void GenerarAuditoriaTransferencia(TransferenciaEntidadDto resultado, ParametrosAuditoriaDto parametrosAuditoriaDto)
        {
            var auditoria = new TransferenciaAuditoriaDto()
            {
                Mensaje = "Transferencia proyecto a entidad destino ",
                TransferenciaEntidad = resultado
            };
            var mensaje = JsonConvert.SerializeObject(auditoria);
            //se quitan las llaves adicionales que genero el newtonsoft
            var contenidoMensaje = mensaje.Substring(1, mensaje.Length - 2);
            Task.Run(() => _auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosNegocio(contenidoMensaje, parametrosAuditoriaDto.Ip, parametrosAuditoriaDto.Usuario, TipoMensajeEnum.Creacion));
            //_auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosNegocio(contenidoMensaje, parametrosAuditoriaDto.Ip, parametrosAuditoriaDto.Usuario, TipoMensajeEnum.Creacion);// Sirve para porbar la auditoria sincronicamente
        }
    }
}
