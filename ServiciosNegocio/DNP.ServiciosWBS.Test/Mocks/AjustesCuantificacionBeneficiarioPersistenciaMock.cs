namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class AjustesCuantificacionBeneficiarioPersistenciaMock : IAjustesCuantificacionBeneficiarioPersistencia
    {
        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiario(string bpin)
        {
            var cuantificacionBeneficiarioDto = new AjustesCuantificacionBeneficiarioDto();

            if (bpin.Equals("202000000000005"))
            {
                var auxVigencia = new List<VigenciasAjustesCuantificacionBeneficiarioDto>();
                var auxLocalizacion = new List<LocalizacionAjustesCuantificacionBeneficiarioDto>();

                auxLocalizacion.Add(new LocalizacionAjustesCuantificacionBeneficiarioDto()
                {
                    LocalizacionId = 1219,
                    RegionId = 4,
                    Region = "Centro Oriente",
                    DepartamentoId = 17,
                    Departamento = "Boyacá",
                    MunicipioId = 600,
                    Municipio = "Labranzagrande",
                    AgrupacionId = null,
                    NombreAgrupacion = null,
                    TipoAgrupacionId = null,
                    TipoAgrupacion = null,
                    NumeroBeneficiariosVigenteFirme = 0,
                    NumeroBeneficiariosVigenteAjuste = 0,
                    NumeroBeneficiariosSolicitadoFirme = 0,
                    NumeroBeneficiariosSolicitadoAjuste = 0
                });

                auxVigencia.Add(new VigenciasAjustesCuantificacionBeneficiarioDto()
                {
                    Vigencia = 2020,
                    Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
                });

                return new AjustesCuantificacionBeneficiarioDto()
                {
                    BPIN = "202000000000005",
                    PoblacionAfectada = 1250,
                    PoblacionObjetivo = 1250,
                    ValorTotalProyectoFirme = 0,
                    ValorTotalProyectoAjuste = 0,
                    Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new AjustesCuantificacionBeneficiarioDto();
            }
        }

        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiarioPreview()
        {
            var cuantificacionBeneficiarioDto = new AjustesCuantificacionBeneficiarioDto();
            var auxVigencia = new List<VigenciasAjustesCuantificacionBeneficiarioDto>();
            var auxLocalizacion = new List<LocalizacionAjustesCuantificacionBeneficiarioDto>();

            auxLocalizacion.Add(new LocalizacionAjustesCuantificacionBeneficiarioDto()
            {
                LocalizacionId = 1219,
                RegionId = 4,
                Region = "Centro Oriente",
                DepartamentoId = 17,
                Departamento = "Boyacá",
                MunicipioId = 600,
                Municipio = "Labranzagrande",
                AgrupacionId = null,
                NombreAgrupacion = null,
                TipoAgrupacionId = null,
                TipoAgrupacion = null,
                NumeroBeneficiariosVigenteFirme = 0,
                NumeroBeneficiariosVigenteAjuste = 0,
                NumeroBeneficiariosSolicitadoFirme = 0,
                NumeroBeneficiariosSolicitadoAjuste = 0
            });

            auxLocalizacion.Add(new LocalizacionAjustesCuantificacionBeneficiarioDto()
            {
                LocalizacionId = 1219,
                RegionId = 4,
                Region = "Centro Oriente",
                DepartamentoId = 17,
                Departamento = "Boyacá",
                MunicipioId = 600,
                Municipio = "Labranzagrande",
                AgrupacionId = null,
                NombreAgrupacion = null,
                TipoAgrupacionId = null,
                TipoAgrupacion = null,
                NumeroBeneficiariosVigenteFirme = 0,
                NumeroBeneficiariosVigenteAjuste = 0,
                NumeroBeneficiariosSolicitadoFirme = 0,
                NumeroBeneficiariosSolicitadoAjuste = 0
            });

            auxVigencia.Add(new VigenciasAjustesCuantificacionBeneficiarioDto()
            {
                Vigencia = 2020,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            auxVigencia.Add(new VigenciasAjustesCuantificacionBeneficiarioDto()
            {
                Vigencia = 2021,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            return new AjustesCuantificacionBeneficiarioDto()
            {
                BPIN = "202000000000005",
                PoblacionAfectada = 1250,
                PoblacionObjetivo = 1250,
                ValorTotalProyectoFirme = 0,
                ValorTotalProyectoAjuste = 0,
                Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> parametrosGuardar, string usuario)
        {

        }
    }
}
