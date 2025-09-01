using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.ModificacionLey
{
    public interface IModificacionLeyPersistencia
    {
        object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
    }
}
