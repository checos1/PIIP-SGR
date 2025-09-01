namespace DNP.ServiciosWBS.Test.Mocks
{

    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class CuantificacionLocalizacionPersistenciaMock: ICuantificacionLocalizacionPersistencia
    {

        public PoblacionDto ObtenerCuantificacionLocalizacion(string bpin)
        {
            var cuantificacionLocalizacionDto = new PoblacionDto();
            
            if (bpin.Equals("2017761220016"))
            {
                var auxVigencia = new List<PoblacionVigenciasDto>();
                var auxLocalizacion = new List<PoblacionVigenciaLocalizacion>();               

                auxLocalizacion.Add(new PoblacionVigenciaLocalizacion()
                {
                    LocalizacionId = 3,
                    RegionId = 3,
                    NombreRegion = "Occidente",
                    CodigoRegion = "03",
                    DepartamentoId = 16,
                    NombreDepartamento = "Valle del Cauca",
                    CodigoDepto = "0376",
                    MunicipioId = 521,
                    NombreMunicipio = "Caicedonia",
                    CodigoMunicipio = "76122",
                    AgrupacionId = null,
                    NombreAgrupacion = null,
                    CodigoAgrupacion = null,
                    CantidadLocalizada = 20
                });

                auxVigencia.Add(new PoblacionVigenciasDto()
                {
                    Vigencia = 2019,                 
                    Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
                });

                return new PoblacionDto()
                {
                    Bpin = "2017761220016",
                    CantidadPoblacion = 500,
                    Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new PoblacionDto();
            }

        }

        public PoblacionDto ObtenerCuantificacionLocalizacionPreview()
        {
            var cuantificacionLocalizacionDto = new PoblacionDto();
            var auxVigencia = new List<PoblacionVigenciasDto>();
            var auxLocalizacion = new List<PoblacionVigenciaLocalizacion>();

            auxLocalizacion.Add(new PoblacionVigenciaLocalizacion()
            {
                LocalizacionId = 3,
                RegionId = 3,
                NombreRegion = "Occidente",
                CodigoRegion = "03",
                DepartamentoId = 16,
                NombreDepartamento = "Valle del Cauca",
                CodigoDepto = "0376",
                MunicipioId = 521,
                NombreMunicipio = "Caicedonia",
                CodigoMunicipio = "76122",
                AgrupacionId = null,
                NombreAgrupacion = null,
                CodigoAgrupacion = null,
                CantidadLocalizada = 20
            });

            auxLocalizacion.Add(new PoblacionVigenciaLocalizacion()
            {
                LocalizacionId = 3,
                RegionId = 4,
                NombreRegion = "Oriente",
                CodigoRegion = "03",
                DepartamentoId = 16,
                NombreDepartamento = "Meta",
                CodigoDepto = "0254",
                MunicipioId = 521,
                NombreMunicipio = "Acacias",
                CodigoMunicipio = "45879",
                AgrupacionId = null,
                NombreAgrupacion = null,
                CodigoAgrupacion = null,
                CantidadLocalizada = 10
            });


            auxVigencia.Add(new PoblacionVigenciasDto()
            {
                Vigencia = 2019,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            auxVigencia.Add(new PoblacionVigenciasDto()
            {
                Vigencia = 2020,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            return new PoblacionDto()
            {
                Bpin = "2017761220016",
                CantidadPoblacion = 500,
                Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
            };
          

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario)
        {

        }
    }
}
