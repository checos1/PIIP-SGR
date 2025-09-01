namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class AjustesPoliticaTransversalCategoriaServicios : ServicioBase<AjustesPoliticaTCategoriasDto>, IAjustesPoliticaTransversalCategoriaServicios
    {
        private readonly IAjustesPoliticaTransversalCategoriaPersistencia _ajustesPoliticaTransversalCategoriaPersistencia;

        public AjustesPoliticaTransversalCategoriaServicios(IAjustesPoliticaTransversalCategoriaPersistencia ajustesPoliticaTransversalCategoriaPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _ajustesPoliticaTransversalCategoriaPersistencia = ajustesPoliticaTransversalCategoriaPersistencia;
        }

        public AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoria(ParametrosConsultaDto parametrosConsultaDto)
        {
            _ajustesPoliticaTransversalCategoriaPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override AjustesPoliticaTCategoriasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AjustesPoliticaTCategoriasDto infoPersistencia = _ajustesPoliticaTransversalCategoriaPersistencia.ObtenerAjustesPoliticaTransversalCategoria(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoriaPreview()
        {
            return _ajustesPoliticaTransversalCategoriaPersistencia.ObtenerAjustesPoliticaTransversalCategoriaPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesPoliticaTCategoriasDto> parametrosGuardar, string usuario)
        {
            _ajustesPoliticaTransversalCategoriaPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
