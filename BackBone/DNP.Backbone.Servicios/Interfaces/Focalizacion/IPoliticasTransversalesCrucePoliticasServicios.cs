using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Focalizacion
{
    public interface IPoliticasTransversalesCrucePoliticasServicios
    {
        Task<string> ObtenerPoliticasTransversalesCrucePoliticas(string Bpin,int IdFuente, string IdUsuario, string tokenAutorizacion);
        Task<RespuestaGeneralDto> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros, string usuarioDnp);

    }
}
