namespace DNP.ServiciosWBS.Test.Mocks
{

    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class FocalizacionCuantificacionLocalizacionPersistenciaMock : IFocalizacionCuantificacionLocalizacionPersistencia
    {

        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(string bpin)
        {
            if (bpin.Equals("2017011000047"))
            {
                var auxFocalizacion = new List<FocalizacionCuantificacionDto>();
                var auxVigencia = new List<VigenciasFocalizacionCuantificacionDto>();
                var auxLocalizacion = new List<LocalizacionFocalizacionCuantificacionDto>();

                auxLocalizacion.Add(new LocalizacionFocalizacionCuantificacionDto()
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
                    CantidadCuantificada = 20,
                    CantidadFocalizada = 10
                });

                auxVigencia.Add(new VigenciasFocalizacionCuantificacionDto()
                {
                    Vigencia = 2019,
                    Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
                });


                auxFocalizacion.Add(new FocalizacionCuantificacionDto()
                {                    
                    PoliticaId = 39,
                    NombreDescripcion = "Desplazados",
                    CantidadMGA = 41,
                    Vigencias = auxVigencia.OrderBy(ip => ip.Vigencia).ToList()
                });

                return new FocalizacionCuantificacionLocalizacionDto()
                {
                    Bpin = "2017761220016",
                    PoblacionObjetivo = 500,
                    Focalizacion = auxFocalizacion.OrderBy(v => v.NombreDescripcion).ToList()
                };
            }
            else
            {
                return new FocalizacionCuantificacionLocalizacionDto();
            }

        }


        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview()
        {
            var auxFocalizacion = new List<FocalizacionCuantificacionDto>();
            var auxVigencia = new List<VigenciasFocalizacionCuantificacionDto>();
            var auxLocalizacion = new List<LocalizacionFocalizacionCuantificacionDto>();

            auxLocalizacion.Add(new LocalizacionFocalizacionCuantificacionDto()
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
                CantidadCuantificada = 20,
                CantidadFocalizada = 10
            });

            auxLocalizacion.Add(new LocalizacionFocalizacionCuantificacionDto()
            {
                LocalizacionId = 4,
                RegionId = 4,
                NombreRegion = "Oriente",
                CodigoRegion = "04",
                DepartamentoId = 16,
                NombreDepartamento = "Cundinamarca",
                CodigoDepto = "0376",
                MunicipioId = 521,
                NombreMunicipio = "Viotá",
                CodigoMunicipio = "76122",
                AgrupacionId = null,
                NombreAgrupacion = null,
                CodigoAgrupacion = null,
                CantidadCuantificada = 20,
                CantidadFocalizada = 10
            });



            auxVigencia.Add(new VigenciasFocalizacionCuantificacionDto()
            {
                Vigencia = 2018,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            auxVigencia.Add(new VigenciasFocalizacionCuantificacionDto()
            {
                Vigencia = 2019,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });


            auxFocalizacion.Add(new FocalizacionCuantificacionDto()
            {
                PoliticaId = 40,
                NombreDescripcion = "Desplazados",
                CantidadMGA = 41,
                Vigencias = auxVigencia.OrderBy(ip => ip.Vigencia).ToList()
            });

            auxFocalizacion.Add(new FocalizacionCuantificacionDto()
            {
                PoliticaId = 41,
                NombreDescripcion = "Población Raizal",
                CantidadMGA = 41,
                Vigencias = auxVigencia.OrderBy(ip => ip.Vigencia).ToList()
            });

            return new FocalizacionCuantificacionLocalizacionDto()
            {
                Bpin = "2017761220016",
                PoblacionObjetivo = 500,
                Focalizacion = auxFocalizacion.OrderBy(v => v.NombreDescripcion).ToList()
            };

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar, string usuario)
        {

        }
    }
}
