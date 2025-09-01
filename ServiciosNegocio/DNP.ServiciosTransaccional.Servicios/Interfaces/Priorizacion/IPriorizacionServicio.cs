using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Priorizacion
{
    public interface IPriorizacionServicio
    {
        Task<ResponseDto<bool>> RegistrarInstanciaPriorizacion(string usuarioDNP, ObjetoNegocio objetoNegocio);
    }
}
