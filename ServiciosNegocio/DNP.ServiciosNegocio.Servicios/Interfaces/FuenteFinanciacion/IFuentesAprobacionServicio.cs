using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    public interface IFuentesAprobacionServicio
    {

        string ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto);

        string GuardarPreguntasAprobacionRol(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario);

        string ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto);

        string GuardarPreguntasAprobacionJefe(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario);
    }
}
