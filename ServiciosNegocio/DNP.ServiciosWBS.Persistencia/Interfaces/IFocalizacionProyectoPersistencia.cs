namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Collections.Generic;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;

    public interface IFocalizacionProyectoPersistencia
    {
        FocalizacionProyectoDto ObtenerFocalizacion(string bpin);
        FocalizacionProyectoDto ObtenerFocalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
