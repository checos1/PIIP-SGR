namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Collections.Generic;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

    public interface IFocalizacionPersistencia
    {
        FocalizacionProyectoDto ObtenerFocalizacionProyecto(string bpin);
        FocalizacionProyectoDto ObtenerFocalizacionProyectoPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
