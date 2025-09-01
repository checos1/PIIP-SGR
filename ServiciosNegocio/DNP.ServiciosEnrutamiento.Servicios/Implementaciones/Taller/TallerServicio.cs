using DNP.ServiciosEnrutamiento.Persistencia.Interfaces;
using DNP.ServiciosEnrutamiento.Servicios.Interfaces;
using DNP.ServiciosEnrutamiento.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Auditoria;
using DNP.ServiciosNegocio.Dominio.Dto.Enrutamiento;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Servicios.Implementaciones
{
    public class TallerServicio: ITallerServicio
    {
        private readonly ITallerPersistencia _tallerPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public TallerServicio(ITallerPersistencia tallerPersistencia, IAuditoriaServicios auditoriaServicios) 
        {
            _tallerPersistencia = tallerPersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public ResultadoEnrutamiento EjecutarReglaTaller(ObjetoNegocio objetoNegocio, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var disponibleCerrar = _tallerPersistencia.ObtenerEstadoProyecto(objetoNegocio.ObjetoNegocioId);
            ResultadoEnrutamiento enrutamiento = new ResultadoEnrutamiento();

            if (disponibleCerrar)
                enrutamiento.Enrutamiento = 1;
            else
                enrutamiento.Enrutamiento = 2;

            _auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosEnrutamiento(objetoNegocio, 
                parametrosAuditoria.Ip, 
                parametrosAuditoria.Usuario, 
                TipoMensajeEnum.Informacion);     

            return enrutamiento;
        }

        public List<ReglaEnrutamiento> ObtenerReglasTaller()
        {
            List<ReglaEnrutamiento> reglas = new List<ReglaEnrutamiento>();

            reglas.Add(new ReglaEnrutamiento()
            {
                Regla = 1,
                Descripcion = "Proyecto Disponible para Cerrar"
            });

            reglas.Add(new ReglaEnrutamiento()
            {
                Regla = 2,
                Descripcion = "Proyecto en Ejecución (No esta disponible para cerrar)"
            });

            return reglas;
        }



    }
}
