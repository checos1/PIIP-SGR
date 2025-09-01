namespace DNP.ServiciosNegocio.Persistencia.ModeloSGR
{
    using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class MGAWebContextoSGR
    {
        public MGAWebContextoSGR(string connectionString) : base(connectionString)
        {

        }

        public virtual ObjectResult<AgregarPreguntasDto> SqlAgregarPreguntasDto()
        {

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<AgregarPreguntasDto>(@"
                SELECT 
                ap.Id AtributoId, ap.Atributo, ap.AtributoPadre,  preguntas.Id PreguntaId, Pregunta, Explicacion,op.ValorOpcion,op.ID OpcionId, cp.Valor Padre
                FROM [Preguntas].[AtributosPregunta] ap
                LEFT JOIN [Preguntas].[ConfiguracionPreguntas]  cp ON cp.AtributoId = ap.Id
                LEFT JOIN [Preguntas].[OpcionesPregunta] op ON op.Id = cp.OpcionId
                LEFT JOIN [Preguntas].[CatalogoPreguntas] preguntas ON preguntas.id = cp.PreguntaId
                WHERE (ap.AgregarRequisitos = 1 and (ap.ModificadoPor = 'NTConsult' OR ap.CreadoPor  = 'NTConsult'))
                ");
        }

    }
}
