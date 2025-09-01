namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICambiosRelacionPlanificacionServicio
    {
        Task<List<RelacionPlanificacionDto>> ConsultarCambiosConpes(int IdProyecto);
        Task<bool> GuardarJustificacionCambios(CapituloModificado capituloModificado, string usuario);
        Task<bool> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada capituloModificado, string usuario);
    }
}
