namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{
    using System.Collections.Generic;
    using Dominio.Dto.Proyectos;
    using Comunes.Dto.Formulario;
    public interface IDefinirAlcancePersistencia
    {
        AlcanceDto ObtenerDefinirAlcance(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AlcanceDto ObtenerDefinirAlcancePreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, string usuario);
    }
}
