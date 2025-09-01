using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IDesagregarEdtServicio
    {
        Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto);
        Task<ReponseHttp> RegistrarNivel(string UsuarioDNP, RegistroModel NivelesNuevos);
        Task<ReponseHttp> EliminarNivel(string UsuarioDNP, RegistroModel NivelesNuevos);
        Task<string> ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid, string usuarioDNP);
        Task<string> GuardarPreguntasAvanceFinanciero(List<PreguntasReporteAvanceFinancieroDto> PreguntasReporteAvanceFinanciero);
        Task<string> ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId, string usuarioDNP);
        Task<string> GuardarAvanceFinanciero(AvanceFinancieroDto reporteAvanceFinanciero);
    }
}
