using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    public interface IFuentesProgramarSolicitadoServicio
    {
        string ObtenerFuentesProgramarSolicitado(string bpin);

        string GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario);

    }
}
