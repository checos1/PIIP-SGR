namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;

    public class FocalizacionPersistenciaMock : IFocalizacionProyectoPersistencia
    {
        public FocalizacionProyectoDto ObtenerFocalizacion(string bpin)
        {
            if (bpin.Equals("2017011000236"))
            {
                var auxVigencias = new List<VigenciaFocalizacionDto>();
                var auxFuentes = new List<FuenteDto>();
                var auxPoliticas = new List<PoliticaDto>();
                var auxDimension = new List<DimensionDto>();
                var auxEjecucionMes = new List<EjecucionMesDto>();
   

                auxDimension.Add(new DimensionDto()
                {
                    DimensionId = 2,
                    Dimension = "Desplazados",
                    Solicitado = 200,
                    ApropiacionInicial = 300,
                    ApropiacionVigente = 400,
                    FocalizacionRecursosId = 4

                });
                auxPoliticas.Add(new PoliticaDto()
                {
                    PoliticaId = 4,
                    Politica = "Desplazados",
                    
                    Dimensiones = auxDimension.OrderBy(r => r.DimensionId).ToList()
                });
                auxFuentes.Add(new FuenteDto()
                {
                    FuenteId = 156,
                    Fuente = "Municipios - MORELIA - Propios",
                    FInicial = 500,
                   FSolicitado = 600,
                    FVigente = 700,
                    Politicas = auxPoliticas.OrderBy(f => f.PoliticaId).ToList()
                });
                auxVigencias.Add(new VigenciaFocalizacionDto()
                {
                    Vigencia = 2017,
                     Fuentes = auxFuentes.OrderBy(f => f.FuenteId).ToList()
                });

                return new FocalizacionProyectoDto()
                {
                    Bpin = "2017011000236",
                    Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new FocalizacionProyectoDto();
            }
        }
        public FocalizacionProyectoDto ObtenerFocalizacionPreview()
        {
            var auxVigencias = new List<VigenciaFocalizacionDto>();
            var auxFuentes = new List<FuenteDto>();
            var auxPoliticas = new List<PoliticaDto>();
            var auxDimension = new List<DimensionDto>();
            var auxEjecucionMes = new List<EjecucionMesDto>();


           

            auxDimension.Add(new DimensionDto()
            {
                DimensionId = 2,
                Dimension = "Desplazados",
                Solicitado = 200,
                ApropiacionInicial = 300,
                ApropiacionVigente = 400
                
            });
            auxPoliticas.Add(new PoliticaDto()
            {
                PoliticaId = 4,
                Politica = "Desplazados",
                Dimensiones = auxDimension.OrderBy(r => r.DimensionId).ToList()
            });
            auxFuentes.Add(new FuenteDto()
            {
                FuenteId = 156,
              
                Fuente = "Municipios - MORELIA - Propios",
                Politicas = auxPoliticas.OrderBy(f => f.PoliticaId).ToList()
            });
            auxVigencias.Add(new VigenciaFocalizacionDto()
            {
                Vigencia = 2017,
                Fuentes = auxFuentes.OrderBy(f => f.FuenteId).ToList()
            });

            return new FocalizacionProyectoDto()
            {
                Bpin = "2017011000236",
                Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
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
