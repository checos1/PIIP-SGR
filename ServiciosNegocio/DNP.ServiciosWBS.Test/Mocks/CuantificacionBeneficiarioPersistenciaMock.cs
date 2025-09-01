using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class CuantificacionBeneficiarioPersistenciaMock : ICuantificacionBeneficiarioPersistencia
    {
        public PoblacionDto ObtenerCuantificacionBeneficiario(string bpin)
        {
            var cuantificacionBeneficiarioDto = new PoblacionDto();

            if (bpin.Equals("202000000000005"))
            {
                var auxVigencia = new List<PoblacionVigenciasDto>();
                var auxLocalizacion = new List<PoblacionVigenciaLocalizacionDto>();

                auxLocalizacion.Add(new PoblacionVigenciaLocalizacionDto()
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
                    NumeroBeneficiarios = 0
                });

                auxVigencia.Add(new PoblacionVigenciasDto()
                {
                    Vigencia = 2020,
                    Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
                });

                return new PoblacionDto()
                {
                    BPIN = "202000000000005",
                    PoblacionAfectada = 1250,
                    PoblacionObjetivo = 1250,
                    ValorTotalProyecto = 0,
                    Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new PoblacionDto();
            }
        }

        public PoblacionDto ObtenerCuantificacionBeneficiarioPreview()
        {
            var cuantificacionBeneficiarioDto = new PoblacionDto();
            var auxVigencia = new List<PoblacionVigenciasDto>();
            var auxLocalizacion = new List<PoblacionVigenciaLocalizacionDto>();

            auxLocalizacion.Add(new PoblacionVigenciaLocalizacionDto()
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
                NumeroBeneficiarios = 0
            });

            auxLocalizacion.Add(new PoblacionVigenciaLocalizacionDto()
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
                NumeroBeneficiarios = 0
            });

            auxVigencia.Add(new PoblacionVigenciasDto()
            {
                Vigencia = 2020,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            auxVigencia.Add(new PoblacionVigenciasDto()
            {
                Vigencia = 2021,
                Localizacion = auxLocalizacion.OrderBy(ip => ip.RegionId).ToList()
            });

            return new PoblacionDto()
            {
                BPIN = "202000000000005",
                PoblacionAfectada = 1250,
                PoblacionObjetivo = 1250,
                ValorTotalProyecto = 0,
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
