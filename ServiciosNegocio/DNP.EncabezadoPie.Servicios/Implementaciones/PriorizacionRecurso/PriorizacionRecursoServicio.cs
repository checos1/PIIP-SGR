namespace DNP.EncabezadoPie.Servicios.Implementaciones.PriorizacionRecurso
{
    using System;
    using System.Collections.Generic;
    using Interfaces.PriorizacionRecurso;
    using Dominio.Dto.PriorizacionRecurso;
    using Persistencia.Interfaces.PriorizacionRecurso;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using Newtonsoft.Json;
    using DNP.EncabezadoPie.Persistencia.Interfaces.Genericos;

    public class PriorizacionRecursoServicio : ServicioBase<PriorizacionRecursoDto>, IPriorizacionRecursoServicio
    {
        private readonly IPriorizacionRecursoPersistencia _priorizacionRecursoPersistencia;

        public PriorizacionRecursoServicio(IPriorizacionRecursoPersistencia priorizacionRecursoPersistencia, IPersistenciaTemporal persistenciaTemporal) : base (persistenciaTemporal)
        {
            _priorizacionRecursoPersistencia = priorizacionRecursoPersistencia;
        }

        public PriorizacionRecursoDto ObtenerPriorizacionRecurso(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public PriorizacionRecursoDto ObtenerPriorizacionRecursoPreview()
        {
            return _priorizacionRecursoPersistencia.ObtenerPriorizacionRecursoPreview();
        }

        protected override PriorizacionRecursoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PriorizacionRecursoDto infoPersistencia = _priorizacionRecursoPersistencia.ObtenerPriorizacionRecurso(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }
    }
}
