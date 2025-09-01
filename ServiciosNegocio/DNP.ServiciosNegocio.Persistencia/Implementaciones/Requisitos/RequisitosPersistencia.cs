using System;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Collections.Generic;
using AutoMapper;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Requisitos
{
    using System.Linq;
    using Comunes.Dto.Formulario;
    using Comunes.Utilidades;
    using Interfaces.Requisitos;
    using Dominio.Dto.Requisitos;

    public class RequisitosPersistencia : Persistencia, IRequisitosPersistencia
    {
        #region Incializacion

        public RequisitosPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }
        #endregion

        #region Consulta
        public List<Atributo> ObtenerRequisitos(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId)
        {
            List<Atributo> listadoRetorno = new List<Atributo>();
            var listadoDesdeBd = Contexto.uspGetPreguntasRequisitosAdicionales(bPin, nivelId, instanciaId, formularioId);

            if (listadoDesdeBd == null)
                return listadoRetorno;
            var listaResultadoSp = listadoDesdeBd.ToList();

            foreach (var atributo in listaResultadoSp)
            {
                listadoRetorno.Add(MapearAtributos(atributo));
            }
            return listadoRetorno;
        }
        #endregion

        #region Guardar

        public void GuardarDefinitivamente(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar, string usuario)
        {
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPreguntasRequisitosAdicionales(parametrosGuardar.Contenido.Bpin, parametrosGuardar.Contenido.IdNivel, JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.FormularioId);
                    Contexto.SaveChanges();
                    dbContextTransaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
        #endregion

        #region Metodos utilitarios
        private Atributo MapearAtributos(object atributo)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspGetPreguntasRequisitosAdicionales_Result, Atributo>());
            return Mapper.Map<Atributo>(atributo);
        }
        #endregion
    }
}
