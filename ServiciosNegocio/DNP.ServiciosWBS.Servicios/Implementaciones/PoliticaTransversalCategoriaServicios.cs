namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class PoliticaTransversalCategoriaServicios : ServicioBase<PoliticaTCategoriasDto>, IPoliticaTransversalCategoriaServicios
    {
        private readonly IPoliticaTransversalCategoriaPersistencia _politicaTransversalCategoriaPersistencia;

        public PoliticaTransversalCategoriaServicios(IPoliticaTransversalCategoriaPersistencia politicaTransversalCategoriaPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _politicaTransversalCategoriaPersistencia = politicaTransversalCategoriaPersistencia;
        }

        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoria(ParametrosConsultaDto parametrosConsultaDto)
        {
            _politicaTransversalCategoriaPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override PoliticaTCategoriasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PoliticaTCategoriasDto infoPersistencia = _politicaTransversalCategoriaPersistencia.ObtenerPoliticaTransversalCategoria(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoriaPreview()
        {
            return _politicaTransversalCategoriaPersistencia.ObtenerPoliticaTransversalCategoriaPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTCategoriasDto> parametrosGuardar, string usuario)
        {
            _politicaTransversalCategoriaPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
