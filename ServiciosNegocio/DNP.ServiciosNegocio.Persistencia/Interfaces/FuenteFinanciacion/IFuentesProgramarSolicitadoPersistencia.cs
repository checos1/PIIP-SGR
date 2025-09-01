
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IFuentesProgramarSolicitadoPersistencia
    {
        string ObtenerFuentesProgramarSolicitado(string bpin);
        string GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario);

    }
}
