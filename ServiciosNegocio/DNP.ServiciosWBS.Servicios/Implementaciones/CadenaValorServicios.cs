namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using Persistencia.Modelo;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using System.Collections.Generic;
    using System.Linq;

    public class CadenaValorServicios : ServicioBase<CadenaValorDto>, ICadenaValorServicios
    {
        private readonly ICadenaValorPersistencia _cadenaValorPersistencia;

        public CadenaValorServicios(ICadenaValorPersistencia cadenaValorPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cadenaValorPersistencia = cadenaValorPersistencia;
        }

        public CadenaValorDto ObtenerCadenaValor(ParametrosConsultaDto parametrosConsulta)
        {
            _cadenaValorPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public CadenaValorDto ObtenerCadenaValorPreview()
        {
            return _cadenaValorPersistencia.ObtenerCadenaValorPreview();
        }

        protected override CadenaValorDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            List<uspGetCadenaValor_Result> cadenaValorList = _cadenaValorPersistencia.ObtenerCadenaValor(parametrosConsultaDto.Bpin).ToList();

            if (cadenaValorList.Count <= 0) return null;

            var cadenaValorDto = new CadenaValorDto();

            if (cadenaValorList.Count() == 0) return null;

            cadenaValorDto.Bpin = cadenaValorList.FirstOrDefault().BPIN;
            cadenaValorDto.Vigencias = new List<VigenciaCadenaValorDto>();
            List<VigenciaCadenaValorDto> vigencias = new List<VigenciaCadenaValorDto>();

            CrearVigencias(cadenaValorDto, cadenaValorList, vigencias);
            CrearMeses(cadenaValorDto, cadenaValorList);
            CrearGrupoRecurso(cadenaValorDto, cadenaValorList);
            CrearObjetivosEspecificos(cadenaValorDto, cadenaValorList);
            CrearProductos(cadenaValorDto, cadenaValorList);
            CrearActividades(cadenaValorDto, cadenaValorList);



            return cadenaValorDto;
        }


        private static void CrearActividades(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(p => p.Mes.ForEach(q => q.GrupoRecurso.ForEach(r => r.ObjetivoEspecifico.ForEach(s =>
                s.Productos.ForEach(a => a.Actividad.AddRange(cadenaValorList.Where(t => p.Vigencia == t.Vigencia && q.Mes == t.MesId && s.Id == t.ObjetivoEspecificoId && r.GrupoRecursoId == t.GRGrupoRecursoId && a.Id == t.ProductoId).
            GroupBy(u => u.ActividadInsumoId).Select(v => v.First()).Where(w=>w.ActividadId != null).
            Select(w => new ActividadCadenaValorDto()
            {
                Id = w.ActividadInsumoId,
                ActividadId = w.ActividadId,
                NombreActividad = w.NombreActividad,
                FechaInicio = w.FechaInicio.Value.ToShortDateString(),
                FechaFin = w.FechaFin.Value.ToShortDateString(),
                Etapa = w.Etapa,
                TipoInsumoId = w.TipoInsumoId,
                ValorSolicitado = w.ValorSolicitado,
                ValorInicial = w.ValorInicial,
                ValorVigente = w.ValorVigente,
                Compromiso = w.Compromiso,
                Obligacion = w.Obligacion,
                Pago = w.Pago,
                Observacion = w.Observacion
            })))))));
        }


        private static void CrearProductos(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(m => m.Mes.ForEach(g => g.GrupoRecurso.ForEach(o => o.ObjetivoEspecifico.ForEach(p => p.Productos.AddRange(cadenaValorList.Where(r => m.Vigencia == r.Vigencia && r.ObjetivoEspecificoId == p.Id).
               GroupBy(s => s.ProductoId).Select(t => t.First()).
               Select(u => new ProductoCadenaValorDto
               {
                   Id = u.ProductoId,
                   CatalogoProductoId = u.CatalogoProductoId,
                   Nombre = u.Nombre,
                   TipoMedidaId = u.TipoMedidaId,
                   Cantidad = u.Cantidad,                   
                   Actividad = new List<ActividadCadenaValorDto>()
               }))))));
        }

        private static void CrearObjetivosEspecificos(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(m => m.Mes.ForEach(g => g.GrupoRecurso.ForEach(o => o.ObjetivoEspecifico.AddRange(cadenaValorList.Where(q => m.Vigencia == q.Vigencia & o.GrupoRecursoId == q.GRGrupoRecursoId).
              GroupBy(r => r.ObjetivoEspecificoId).Select(s => s.First()).
              Select(t => new ObjetivoEspecificoCadenaValorDto
              {
                  Id = t.ObjetivoEspecificoId,
                  ObjetivoEspecifico = t.ObjetivoEspecifico,
                  Productos = new List<ProductoCadenaValorDto>()
              })))));
        }

        private static void CrearGrupoRecurso(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(m => m.Mes.ForEach(g => g.GrupoRecurso.AddRange(cadenaValorList.Where(h => m.Vigencia == h.Vigencia && h.MesId == g.Mes && g.NombreMes == h.Mes ).
             GroupBy(r => r.GRGrupoRecursoId).Select(s => s.First()).
             Select(t => new GrupoRecursoDto()
             {
                 GrupoRecursoId = t.GRGrupoRecursoId,
                 GrupoRecurso = t.GRGrupoRecurso,
                 ValorSolicitado = t.GRValorSolicitado,
                 ValorInicial = t.GRValorInicial,
                 ValorVigente = t.GRValorVigente,
                 Compromiso = t.GRCompromiso,
                 Obligacion = t.GRObligacion,
                 Pago = t.GRPago,
                 ObjetivoEspecifico = new List<ObjetivoEspecificoCadenaValorDto>()
             }))));
        }
        private static void CrearMeses(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList)
        {
            cadenaValorDto.Vigencias.ForEach(m => m.Mes.AddRange(cadenaValorList.Where(q => m.Vigencia == q.Vigencia).
            GroupBy(r => r.MesId).Select(s => s.First()).
            Select(t => new MesCadenaValorDto()
            {
                Mes = t.MesId,
                NombreMes = t.Mes,
                GrupoRecurso = new List<GrupoRecursoDto>()
            })));
        }

        private static void CrearVigencias(CadenaValorDto cadenaValorDto, IEnumerable<uspGetCadenaValor_Result> cadenaValorList, List<VigenciaCadenaValorDto> vigencias)
        {
            foreach (var cadenaValorItem in cadenaValorList)
            {
                vigencias.Add(new VigenciaCadenaValorDto()
                {
                    Vigencia = cadenaValorItem.Vigencia,
                    Mes = new List<MesCadenaValorDto>(),
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
