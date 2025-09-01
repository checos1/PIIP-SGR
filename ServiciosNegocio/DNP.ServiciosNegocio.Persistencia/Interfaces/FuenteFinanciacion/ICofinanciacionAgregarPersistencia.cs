using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    public interface ICofinanciacionAgregarPersistencia
    {
        CofinanciacionProyectoDto ObtenerCofinanciacionAgregar(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        CofinanciacionProyectoDto ObtenerCofinanciacionAgregarPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<CofinanciacionProyectoDto> parametrosGuardar, string usuario);
    }
}
