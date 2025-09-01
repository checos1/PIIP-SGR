using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces
{
    public interface IModificacionLeyServicio
    {
        object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
    }
}
