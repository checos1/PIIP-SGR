namespace DNP.ServiciosWBS.Persistencia.Implementaciones.Transversales
{
    using Interfaces;
    using Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Modelo;

    public class PersistenciaTemporal : Persistencia, IPersistenciaTemporal
    {
        public PersistenciaTemporal(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarTemporalmente<T>(ParametrosGuardarDto<T> parametrosGuardar)
        {
            AlmacenamientoTemporal[] objetos =
            {
                new AlmacenamientoTemporal
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    InstanciaId =
                        parametrosGuardar.
                            InstanciaId.Value,
                    // ReSharper disable once PossibleInvalidOperationException
                    AccionId =
                        parametrosGuardar.AccionId.Value,
                    Json =
                        JsonUtilidades.
                            ACadenaJson(parametrosGuardar.
                                            Contenido)
                }
            };

            Contexto.AlmacenamientoTemporal.AddOrUpdate(objetos);
            GuardarCambios();
        }

        public AlmacenamientoTemporal ObtenerTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            var resultado = Contexto.AlmacenamientoTemporal.FirstOrDefault(t => t.InstanciaId == parametrosConsultaDto.InstanciaId && t.AccionId == parametrosConsultaDto.AccionId);

            return resultado;
        }
    }
}
