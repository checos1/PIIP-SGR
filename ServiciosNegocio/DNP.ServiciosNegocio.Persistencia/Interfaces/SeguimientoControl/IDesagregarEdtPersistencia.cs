using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl
{
    public interface IDesagregarEdtPersistencia
    {
        string ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        void RegistrarNivel(string usuario, List<RegistroEntregable> nivelesNuevos);
        void RegistrarActividad(string usuario, List<RegistroEntregable> nivelesNuevos);
        void EliminarActividad(string usuario, List<RegistroEntregable> nivelesNuevos);
        string ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid);
        string GuardarPreguntasAvanceFinanciero(ParametrosGuardarDto<List<PreguntasReporteAvanceFinancieroDto>> parametrosGuardar, string usuario);
        string ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId);
        string GuardarAvanceFinanciero(ParametrosGuardarDto<AvanceFinancieroDto> parametrosGuardar, string usuario);
    }
}
