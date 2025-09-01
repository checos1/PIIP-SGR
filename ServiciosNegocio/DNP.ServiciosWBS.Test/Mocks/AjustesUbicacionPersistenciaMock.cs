namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class AjustesUbicacionPersistenciaMock : IAjustesUbicacionPersistencia
    {
        public AjustesUbicacionDto ObtenerAjustesUbicacion(string bpin)
        {
            var localizacionDto = new AjustesUbicacionDto();

            if (bpin.Equals("202000000000005"))
            {
                return new AjustesUbicacionDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005"
                };
            }
            else
            {
                return new AjustesUbicacionDto();
            }
        }

        public AjustesUbicacionDto ObtenerAjustesUbicacionPreview()
        {
            var nuevaUbicacionDto = new List<NuevaUbicacionDto>();

            nuevaUbicacionDto.Add(new NuevaUbicacionDto()
            {
                LocalizacionId = 1247,
                RegionId = 5,
                Region = "Orinoquía",
                DepartamentoId = 23,
                Departamento = "Meta",
                MunicipioId = 1018,
                Municipio = "Mesetas",
                TipoAgrupacionId = 2,
                TipoAgrupacion = "Resguardo",
                AgrupacionId = 332,
                Agrupacion = "Ondas Del Cafre"
            });

            return new AjustesUbicacionDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                NuevaLocalizacion = nuevaUbicacionDto
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesUbicacionDto> parametrosGuardar, string usuario)
        {

        }
    }
}
