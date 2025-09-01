using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.ModificacionLey;

namespace DNP.ServiciosTransaccional.Test.Mock
{
    public class ModificacionLeyPersistenciaMock : IModificacionLeyPersistencia
    {
        public object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }
    }
}
