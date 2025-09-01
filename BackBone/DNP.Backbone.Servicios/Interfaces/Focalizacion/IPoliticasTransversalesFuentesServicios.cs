using DNP.Backbone.Dominio.Dto.Focalizacion;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Focalizacion
{
    public interface IPoliticasTransversalesFuentesServicios
    {
        Task<string> ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin, string usuarioDnp, string tokenAutorizacion);
        

    }
}
