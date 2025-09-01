using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario
{
    using Comunes.Dto.Formulario;

    public interface ICadenaValorPersistencia
    {
        IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin);
        object ObtenerCadenaValorPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario);
    }
}
