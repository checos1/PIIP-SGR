namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using Modelo;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using System.Collections.Generic;

    public interface ICadenaValorPersistencia
    {
        IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin);
        CadenaValorDto ObtenerCadenaValorPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
