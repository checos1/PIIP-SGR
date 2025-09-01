namespace DNP.EncabezadoPie.Servicios.Implementaciones.DefinirAlcance
{
    using System;
    using System.Collections.Generic;
    using Interfaces.DefinirAlcance;
    using Dominio.Dto.DefinirAlcance;
    using Persistencia.Interfaces.DefinirAlcance;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using Newtonsoft.Json;
    using DNP.EncabezadoPie.Persistencia.Interfaces.Genericos;

    public class DefinirAlcanceServicio : ServicioBase<AlcanceDto>, IDefinirAlcanceServicio
    {
        private readonly IDefinirAlcancePersistencia _definirAlcanceoPersistencia;

        public DefinirAlcanceServicio(IDefinirAlcancePersistencia definirAlcancePersistencia, IPersistenciaTemporal persistenciaTemporal) : base(persistenciaTemporal)
        {
            _definirAlcanceoPersistencia = definirAlcancePersistencia;
        }

        public AlcanceDto ObtenerAlcance(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public AlcanceDto ObtenerAlcancePreview()
        {
            return _definirAlcanceoPersistencia.ObtenerAlcancePreview();
        }

        protected override AlcanceDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AlcanceDto infoPersistencia = _definirAlcanceoPersistencia.ObtenerAlcance(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }
    }
}
