
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IFuentesAprobacionPersistencia
    {
        string ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto);

        string GuardarPreguntasAprobacionRol(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objProgramacionValorFuenteDto, string usuario);

        string ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto);

        string GuardarPreguntasAprobacionJefe(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario);
    }
}
