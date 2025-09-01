namespace DNP.EncabezadoPie.Servicios.Interfaces.PriorizacionRecurso
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dominio.Dto.PriorizacionRecurso;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IPriorizacionRecursoServicio
    {
        PriorizacionRecursoDto ObtenerPriorizacionRecurso(ParametrosConsultaDto parametrosConsulta);
        PriorizacionRecursoDto ObtenerPriorizacionRecursoPreview();
    }
}
