namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Collections.Generic;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;

    public interface IValidarViabilidadCompletarInfoPersistencia
    {
        ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsultaDto);
        ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar, string usuario);
       
    }
}
