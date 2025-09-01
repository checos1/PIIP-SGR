using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion
{
    public interface IProgramarSolicitadoServicio
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuenteId"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ConsultarFuentesProgramarSolicitado(string bpin, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// funcion para Guardar Programar Solicitado Fuentes
        /// </summary>
        /// <param name="objProgramacionValorFuenteDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns></returns>
        Task<string> GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP);
        Task<string> guardarFuentesFinanciacionRecursosAjustes(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP);
    }
}
