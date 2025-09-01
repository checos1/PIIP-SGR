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
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
	using DNP.ServiciosNegocio.Dominio.Dto.Tramites;

	public class LocalizacionPersistenciaMock : ILocalizacionPersistencia
    {
        public LocalizacionProyectoDto Obtenerlocalizacion(string bpin)
        {
            var localizacionDto = new LocalizacionProyectoDto();

            if (bpin.Equals("202000000000005"))
            {
                return new LocalizacionProyectoDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005"
                };
            }
            else
            {
                return new LocalizacionProyectoDto();
            }
        }

        public LocalizacionProyectoDto ObtenerlocalizacionPreview()
        {
            var localizacionDto = new List<LocalizacionNuevaDto>();

            localizacionDto.Add(new LocalizacionNuevaDto()
            {
                RegionId = 4,
                Region = "Centro Oriente",
                DepartamentoId = 17,
                Departamento = "Boyacá",
                MunicipioId = 600,
                Municipio = "Labranzagrande",
                TipoAgrupacionId = null,
                TipoAgrupacion = null,
                AgrupacionId = null,
                Agrupacion = null
            });

            return new LocalizacionProyectoDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                NuevaLocalizacion = localizacionDto
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<LocalizacionProyectoDto> parametrosGuardar, string usuario)
        {

        }

        public ResultadoProcedimientoDto GuardarLocalizacion(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
            ResultadoProcedimientoDto obj = new ResultadoProcedimientoDto();
            obj = null;
            return obj;
        }
    }
}
