namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;    
    using Comunes.Dto.Formulario;

    public interface IDevolverProyectoPersistencia
    {
        DevolverProyectoDto ObtenerDevolverProyecto(string bpin);
        void GuardarDefinitivamente(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, string usuario);
    }
}
