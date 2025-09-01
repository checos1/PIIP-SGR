using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using System;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System.Collections.Generic;
    using Comunes;
    using Comunes.Dto.Formulario;
    using Comunes.Utilidades;
    using Persistencia.Modelo;

    public class CadenaValorPersistenciaMock : ICadenaValorPersistencia
    {
        public object ObtenerCadenaValorPreview()
        {
            CadenaValorDto cadenaValor = SerializadorJson.SerializarJsonObjeto<CadenaValorDto>(Environment.CurrentDirectory + RutasPreviewRecursos.PreviewCadenaValor);
            return cadenaValor;
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario) {  }

        public IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin)
        {
            var resultado = new uspGetCadenaValor_Result()
            {
                ProyectoBPIN = "2017002700002",
                Vigencia = 2017,
                Actividad = "actividad",
                ActividadEtapa = "actividadEtapa",
                ActividadId = 1,
                ActividadEtapaId = 1,
                ActividadInsumoId = 1,
                EjecucionCompromiso = 1,
                EjecucionGrupoRecurso = "ejeGruRec",
                EjecucionGrupoRecursoId = 1,
                Valor_Vigente = 100,
                Valor_Inicial = 100,
                EjecucionMes = 0,
                EjecucionObligacion = 0,
                EjecucionPago = 0,
                EjecucionValorInicial = 0,
                EjecucionValorVigente = 0,
                InsumoId = 1,
                InsumoNombre = "insumo",
                ObjetivoEspecifico = "objetivo",
                ObjetivoEspecificoId = 1,
                Producto = "prducto",
                ProductoId = 1,
                ProyectoId = 1,
                TotalEtapaApropiacionInicial = 1,
                TotalEtapaApropiacionVigente = 1,
                TotalEtapaValorSolicitado = 1,
                Valor_Socilitado = 100
            };
            var retorno = new List<uspGetCadenaValor_Result>();
            retorno.Add(resultado);
            return retorno;
        }
    }
}
