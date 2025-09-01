using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Test.Mocks
{
    public class FuentesAprobacionServicioMock : IFuentesAprobacionServicio
    {
        IFuentesAprobacionServicio _IFuentesAprobacionServicio;

        public FuentesAprobacionServicioMock(IFuentesAprobacionServicio fuentesAprobacionServicio)
        {
            _IFuentesAprobacionServicio = fuentesAprobacionServicio;
        }

        public Task<string> ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            return _IFuentesAprobacionServicio.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion);
        }
        
        public Task<string> GuardarPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            return _IFuentesAprobacionServicio.GuardarPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion);
        }

        public Task<string> ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            return _IFuentesAprobacionServicio.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion);
        }

        public Task<string> GuardarPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuario, string tokenAutorizacion)
        {
            return _IFuentesAprobacionServicio.GuardarPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuario, tokenAutorizacion);
        }
    }
}
