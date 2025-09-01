using DNP.Backbone.Dominio.Dto.Fichas;
using DNP.Backbone.Dominio.Dto.Transversal;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Fichas
{
    public interface IFichasServicios
    {
        Task<byte[]> GenerarFicha(RecibirParametrosDto parametros, string usuarioDNP);
        Task<ReporteDto> ObtenerIdFicha(string nombre, string usuarioDNP);
        Task<string> GenerarFichaManualSubFlujoSGR(ObjetoNegocio objObjetoNegocio, string usuario);
    }
}