namespace DNP.ServiciosWBS.Test.Mocks
{
    using System;
    using System.Collections.Generic;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public class CadenaValorPersistenciaMock : ICadenaValorPersistencia
    {
        public CadenaValorDto ObtenerCadenaValorPreview()
        {
            CadenaValorDto cadenaValor = SerializadorJson.SerializarJsonObjeto<CadenaValorDto>(Environment.CurrentDirectory + RutasPreviewRecursos.PreviewCadenaValor);
            return cadenaValor;
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario)
        {

        }

        public IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin)
        {
            var resultado = new uspGetCadenaValor_Result()
            {
                BPIN = "2017002700002",
                Vigencia = 2017,
                MesId = 1,
                Mes = "enero",
                GRGrupoRecursoId = 1,
                GRGrupoRecurso = "Territorial",
                GRValorSolicitado = 1100,
                GRValorInicial = 123,
                GRValorVigente = 2222,
                GRCompromiso = 3666,
                GRObligacion = 5454,
                GRPago = 4485,
                ObjetivoEspecifico = "objetivo",
                ObjetivoEspecificoId = 1,
                ProductoId = 1,
                CatalogoProductoId = 2,
                Nombre = "nombre",
                TipoMedidaId = 1,
                Cantidad = 200,
                Etapa = "Etapa",
                ActividadId = 1100,
                ActividadInsumoId = 1,
                TipoInsumoId = 3,
                ValorSolicitado = 215404,
                ValorInicial = 45555,
                ValorVigente = 4550,
                Compromiso = 899,
                Obligacion = 1215,
                Pago = 21548,
                Observacion = "Observacion"
            };
            var retorno = new List<uspGetCadenaValor_Result>();
            retorno.Add(resultado);
            return retorno;
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

    }
}
