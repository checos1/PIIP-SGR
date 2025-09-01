namespace DNP.EncabezadoPie.Persistencia.Interfaces.PriorizacionRecurso
{
    using Dominio.Dto.PriorizacionRecurso;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPriorizacionRecursoPersistencia
    {
        PriorizacionRecursoDto ObtenerPriorizacionRecurso(string bpin);
        PriorizacionRecursoDto ObtenerPriorizacionRecursoPreview();
    }
}
