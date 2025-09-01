using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
using DNP.ServiciosWBS.Servicios.Implementaciones;
using DNP.ServiciosWBS.Servicios.Interfaces.Transversales;



namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class PoliticasTransversalesCrucePoliticasServicios : ServicioBase<PoliticasTCrucePoliticasDto>, IPoliticasTransversalesCrucePoliticasServicios

    {
        private readonly IPoliticasTransversalesCrucePoliticasPersistencia _politicasTransversalesCrucePoliticasPersistencia;
        public PoliticasTransversalesCrucePoliticasServicios(IPoliticasTransversalesCrucePoliticasPersistencia politicasTransversalesCrucePoliticasPersistencia,
                        IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
       base(persistenciaTemporal, auditoriaServicios)
        {
            _politicasTransversalesCrucePoliticasPersistencia = politicasTransversalesCrucePoliticasPersistencia;
        }
        public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticas(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }
        protected override PoliticasTCrucePoliticasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PoliticasTCrucePoliticasDto infoPersistencia = _politicasTransversalesCrucePoliticasPersistencia.ObtenerPoliticasTransversalesCrucePoliticas(parametrosConsultaDto.Bpin, parametrosConsultaDto.IdFuente);
            return infoPersistencia;
        }
        public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasPreview()
        {
            return _politicasTransversalesCrucePoliticasPersistencia.ObtenerPoliticasTransversalesCrucePoliticasPreview();

        }
        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticasTCrucePoliticasDto> parametrosGuardar, string usuario)
        {
            _politicasTransversalesCrucePoliticasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
