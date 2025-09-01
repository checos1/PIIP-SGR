using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using System;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Transversales
{
    public class TransversalRecursoServicio : ITransversalRecursoServicio
    {
        private readonly ITransversalRecursoPersistencia _transversalRecursoPersistencia;

        public TransversalRecursoServicio(ITransversalRecursoPersistencia transversalRecursoPersistencia)
        {
            _transversalRecursoPersistencia = transversalRecursoPersistencia;
        }
        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionSgr(string bpin)
        {
            return _transversalRecursoPersistencia.ObtenerDesagregarRegionalizacionSgr(bpin);
        }
        //public DatosGeneralesProyectosDto ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId)
        //{
        //    return _transversalRecursoPersistencia.ObtenerDatosGeneralesProyectoSgr(pProyectoId, pNivelId);
        //}
        public FocalizacionPoliticaSgrDto ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin)
        {
            FocalizacionPoliticaSgrDto infoPersistencia = _transversalRecursoPersistencia.ObtenerFocalizacionPoliticasTransversalesFuentesSgr(bpin);
            return infoPersistencia;
        }
        public RespuestaGeneralDto GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            return _transversalRecursoPersistencia.GuardarFocalizacionCategoriasAjustesSgr(focalizacionCategoriasAjuste, usuario);
        }
        //public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente)
        //{
        //    return _transversalRecursoPersistencia.ObtenerPoliticasTransversalesCrucePoliticasSgr(bpin, IdFuente);
        //}
        //public IndicadoresPoliticaDto ObtenerDatosIndicadoresPoliticaSgr(string bpin)
        //{
        //    return _transversalRecursoPersistencia.ObtenerDatosIndicadoresPoliticaSgr(bpin);
        //}
        //public IndicadoresPoliticaDto ObtenerDatosCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId)
        //{
        //    return _transversalRecursoPersistencia.ObtenerDatosCategoriaProductosPoliticaSgr(bpin, fuenteId, politicaId);
        //}
        public string ObtenerPoliticasTransversalesProyectoSgr(string Bpin)
        {
            return _transversalRecursoPersistencia.ObtenerPoliticasTransversalesProyectoSgr(Bpin);
        }
        public RespuestaGeneralDto EliminarPoliticasProyectoSgr(int proyectoId, int politicaId)
        {
            return _transversalRecursoPersistencia.EliminarPoliticasProyectoSgr(proyectoId, politicaId);
        }
        public string AgregarPoliticasTransversalesAjustesSgr(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            return _transversalRecursoPersistencia.AgregarPoliticasTransversalesAjustesSgr(parametrosGuardar, usuario);
        }
        public string ConsultarPoliticasCategoriasIndicadoresSgr(Guid instanciaId)
        {
            return _transversalRecursoPersistencia.ConsultarPoliticasCategoriasIndicadoresSgr(instanciaId);
        }
        public string ObtenerPoliticasTransversalesCategoriasSgr(Guid instanciaId)
        {
            return _transversalRecursoPersistencia.ObtenerPoliticasTransversalesCategoriasSgr(instanciaId);
        }
        public RespuestaGeneralDto EliminarCategoriasPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId)
        {
            return _transversalRecursoPersistencia.EliminarCategoriaPoliticasProyectoSgr(proyectoId, politicaId, categoriaId);
        }
        public ResultadoProcedimientoDto ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            return _transversalRecursoPersistencia.ModificarPoliticasCategoriasIndicadoresSgr(parametrosGuardar, usuario);
        }
        public string ObtenerCrucePoliticasAjustesSgr(Guid instanciaId)
        {
            return _transversalRecursoPersistencia.ObtenerCrucePoliticasAjustesSgr(instanciaId);
        }
        public string GuardarCrucePoliticasAjustesSgr(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            return _transversalRecursoPersistencia.GuardarCrucePoliticasAjustesSgr(parametrosGuardar, usuario);
        }
        public string ObtenerPoliticasTransversalesResumenSgr(Guid instanciaId)
        {
            return _transversalRecursoPersistencia.ObtenerPoliticasTransversalesResumenSgr(instanciaId);
        }
    }
}
