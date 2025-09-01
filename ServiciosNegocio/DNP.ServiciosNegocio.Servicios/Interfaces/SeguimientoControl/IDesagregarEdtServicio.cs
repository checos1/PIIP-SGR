using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl
{
    public interface IDesagregarEdtServicio
    {
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        Task<ReponseHttp> RegistrarNivel(string usuario, RegistroModel nivelesNuevos);
        Task<ReponseHttp> EliminarNivel(string usuario, RegistroModel nivelesNuevos);
        string ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid);
        string GuardarPreguntasAvanceFinanciero(ParametrosGuardarDto<List<PreguntasReporteAvanceFinancieroDto>> parametrosGuardar, string name);
        string ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId);
        string GuardarAvanceFinanciero(ParametrosGuardarDto<AvanceFinancieroDto> parametrosGuardar, string name);
    }
}
