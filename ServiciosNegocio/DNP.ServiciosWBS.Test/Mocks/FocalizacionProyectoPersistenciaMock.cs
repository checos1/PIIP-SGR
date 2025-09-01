namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

    public class FocalizacionProyectoPersistenciaMock : IFocalizacionPersistencia
    {
        public FocalizacionProyectoDto ObtenerFocalizacionProyecto(string bpin)
        {
            if (bpin.Equals("2017011000236"))
            {
                var auxPoliticas = new List<PoliticaDto>();
                var auxDimension = new List<DimensionDto>();
              
                auxDimension.Add(new DimensionDto()
                {
                    DimensionId = 2,
                    Dimension = "Desplazados",
                    FocalizacionProyectoId = 4

                });
                auxDimension.Add(new DimensionDto()
                {
                    DimensionId = 3,
                    Dimension = "Victimas",
                    FocalizacionProyectoId = 5

                });
                auxPoliticas.Add(new PoliticaDto()
                {
                    PoliticaId = 4,
                    Politica = "Desplazados",
                    
                    Dimensiones = auxDimension.OrderBy(r => r.DimensionId).ToList()
                });

                return new FocalizacionProyectoDto()
                {
                    Bpin = "2017011000236",
                    Politicas = auxPoliticas.OrderBy(v => v.PoliticaId).ToList()
                };
            }
            else
            {
                return new FocalizacionProyectoDto();
            }
        }
        public FocalizacionProyectoDto ObtenerFocalizacionProyectoPreview()
        {
            var auxPoliticas = new List<PoliticaDto>();
            var auxDimension = new List<DimensionDto>();

            auxDimension.Add(new DimensionDto()
            {
                DimensionId = 2,
                Dimension = "Desplazados",
                FocalizacionProyectoId = 4

            });
            auxDimension.Add(new DimensionDto()
            {
                DimensionId = 3,
                Dimension = "Victimas",
                FocalizacionProyectoId = 5

            });
            auxPoliticas.Add(new PoliticaDto()
            {
                PoliticaId = 4,
                Politica = "Desplazados",

                Dimensiones = auxDimension.OrderBy(r => r.DimensionId).ToList()
            });

            return new FocalizacionProyectoDto()
            {
                Bpin = "2017011000236",
                Politicas = auxPoliticas.OrderBy(v => v.PoliticaId).ToList()
            };
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            
        }
    }
}
