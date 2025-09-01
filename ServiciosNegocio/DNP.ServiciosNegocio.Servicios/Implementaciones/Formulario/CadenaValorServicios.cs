using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Interfaces.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario
{
    public class CadenaValorServicios : ServicioBase<CadenaValorDto>, ICadenaValorServicios
    {
        private readonly ICadenaValorPersistencia _cadenaValorPersistencia;

        public CadenaValorServicios(ICadenaValorPersistencia cadenaValorPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cadenaValorPersistencia = cadenaValorPersistencia;
        }

        public CadenaValorDto ObtenerCadenaValor(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public object ObtenerCadenaValorPreview()
        {
            return _cadenaValorPersistencia.ObtenerCadenaValorPreview();
        }

        protected override CadenaValorDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            List<uspGetCadenaValor_Result> cadenaValorList = _cadenaValorPersistencia.ObtenerCadenaValor(parametrosConsultaDto.Bpin).ToList();

            if (cadenaValorList.Count <= 0) return null;

            var cadenaValorDto = new CadenaValorDto();

            if (cadenaValorList.Count() == 0) return null;

            cadenaValorDto.Bpin = cadenaValorList.FirstOrDefault().ProyectoBPIN;
            cadenaValorDto.Vigencias = new List<VigenciaDto>();
            List<VigenciaDto> vigencias = new List<VigenciaDto>();

            CrearVigencias(cadenaValorDto, cadenaValorList, vigencias);
            CrearObjetivosEspecificos(cadenaValorDto, cadenaValorList);
            CrearProductos(cadenaValorDto, cadenaValorList);
            CrearEtapas(cadenaValorDto, cadenaValorList);
            CrearActividades(cadenaValorDto, cadenaValorList);
            CrearEjecucuciones(cadenaValorDto, cadenaValorList);

            return cadenaValorDto;
        }

        private static void CrearEjecucuciones(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.ObjetivosEspecificos.ForEach(q => q.Productos.ForEach(r => r.Etapas.ForEach(s => s.Actividades.ForEach(t => t.Ejecuciones.AddRange(
            cadenaValorList.Where(u => p.Vigencia == u.Vigencia && t.IdActividadPorInsumo == u.ActividadInsumoId && u.ActividadId == t.Id).
            Select(u => new EjecucionDto()
            {
                Mes = u.EjecucionMes,
                ApropiacionInicialMes = u.EjecucionValorInicial,
                ApropiacionVigenteMes = u.EjecucionValorVigente,
                Compromiso = u.EjecucionCompromiso,
                Obligacion = u.EjecucionObligacion,
                Pago = u.EjecucionPago,
                IdGrupoRecurso = u.EjecucionGrupoRecursoId,
                GrupoRecurso = u.EjecucionGrupoRecurso
            })))))));
        }

        private static void CrearActividades(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.ObjetivosEspecificos.ForEach(q => q.Productos.ForEach(r => r.Etapas.ForEach(s => s.Actividades.AddRange(cadenaValorList.Where(t => p.Vigencia == t.Vigencia && r.Id == t.ProductoId && t.ActividadEtapaId == s.Id).
            GroupBy(u => (u.ActividadInsumoId == null ? 0 : u.ActividadInsumoId) + u.ActividadId).Select(v => v.First()).
            Select(w => new ActividadDto()
            {
                Id = w.ActividadId,
                Nombre = w.Actividad,
                IdActividadPorInsumo = w.ActividadInsumoId,
                IdInsumo = w.InsumoId,
                NombreInsumo = w.InsumoNombre,
                ValorSolicitado = w.Valor_Socilitado,
                ApropiacionInicial = w.Valor_Inicial,
                ApropiacionVigente = w.Valor_Vigente,
                Ejecuciones = new List<EjecucionDto>()
            }))))));
        }

        private static void CrearEtapas(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.ObjetivosEspecificos.ForEach(q => q.Productos.ForEach(r => r.Etapas.AddRange(cadenaValorList.Where(s => p.Vigencia == s.Vigencia && s.ProductoId == r.Id).
            GroupBy(t => t.ActividadEtapaId).Select(u => u.First()).
            Select(v => new GenerarEtapaCadenaValor(v, p).Etapa)))));
        }

        private static void CrearProductos(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.ObjetivosEspecificos.ForEach(q => q.Productos.AddRange(cadenaValorList.Where(r => p.Vigencia == r.Vigencia && r.ObjetivoEspecificoId == q.Id).
            GroupBy(s => s.ProductoId).Select(t => t.First()).
            Select(u => new ProductoCadenaValorDto
            {
                Id = u.ProductoId,
                Producto = u.Producto,
                Etapas = new List<EtapaDto>()
            }))));
        }

        private static void CrearObjetivosEspecificos(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.ObjetivosEspecificos.AddRange(cadenaValorList.Where(q => p.Vigencia == q.Vigencia).
            GroupBy(r => r.ObjetivoEspecificoId).Select(s => s.First()).
            Select(t => new ObjetivoEspecificoCadenaValorDto
            {
                Id = t.ObjetivoEspecificoId,
                ObjetivoEspecifico = t.ObjetivoEspecifico,
                Productos = new List<ProductoCadenaValorDto>()
            })));
        }

        private static void CrearVigencias(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList, List<VigenciaDto> vigencias)
        {
            foreach (var cadenaValorItem in cadenaValorList)
            {
                vigencias.Add(new VigenciaDto()
                {
                    Vigencia = cadenaValorItem.Vigencia,
                    ObjetivosEspecificos = new List<ObjetivoEspecificoCadenaValorDto>(),
                    GranTotalPorVigencia = 0M
                });
            }

            cadenaValorDto.Vigencias.AddRange(vigencias.GroupBy(x => x.Vigencia).Select(y => y.First()));
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario)
        {
            _cadenaValorPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
