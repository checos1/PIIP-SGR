using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Test.Mock
{
    public class FuentesAprobacionPersistenciaMock : IFuentesAprobacionPersistencia
    {
        private readonly IFuentesAprobacionPersistencia _IFuentesSeguimientoProyectoPersistencia;


        public FuentesAprobacionPersistenciaMock(IFuentesAprobacionPersistencia fuentesAprobacionPersistencia)
        {
            _IFuentesSeguimientoProyectoPersistencia = fuentesAprobacionPersistencia;
        }

        public string ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            return _IFuentesSeguimientoProyectoPersistencia.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto);
        }

        public string GuardarPreguntasAprobacionRol(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objProgramacionValorFuenteDto, string usuario)
        {
            return _IFuentesSeguimientoProyectoPersistencia.GuardarPreguntasAprobacionRol(objProgramacionValorFuenteDto, usuario);
        }

        public string ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            return _IFuentesSeguimientoProyectoPersistencia.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto);
        }
        public string GuardarPreguntasAprobacionJefe(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario)
        {
            return _IFuentesSeguimientoProyectoPersistencia.GuardarPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuario);
        }
    }
}
