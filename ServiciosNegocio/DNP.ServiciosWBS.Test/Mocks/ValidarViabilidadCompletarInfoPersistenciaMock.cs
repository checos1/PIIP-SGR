namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;

    public class ValidarViabilidadCompletarInfoPersistenciaMock : IValidarViabilidadCompletarInfoPersistencia
    {
        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsultaDto)
        {
            if (parametrosConsultaDto.Bpin.Equals("2017011000236"))
            {
                var auxTematicas = new List<TematicaDto>();
                var auxMensajesError = new List<MensajeErrorDto>();



                auxMensajesError.Add(new MensajeErrorDto()
                {
                    MensajeError = "Para la vigencia 2017, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"
                    

                });
                auxMensajesError.Add(new MensajeErrorDto()
                {
                    MensajeError = "Para la  vigencia 2018, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"


                });
                auxTematicas.Add(new TematicaDto()
                {
                    Tematica = " Focalización x Actividades Actis",
                    MensajesError = auxMensajesError.ToList()
                });
                auxTematicas.Add(new TematicaDto()
                {
                    Tematica = " Focalización x Actividades Actis2",
                    MensajesError = auxMensajesError.ToList()
                });


                return new ValidarViabilidadCompletarInfoDto()
                {
                    Bpin = "2017011000236",
                    Mensaje = "Mensaje Prueba",
                    Tematicas = auxTematicas.ToList()
                };
            }
            else
            {
                return new ValidarViabilidadCompletarInfoDto();
            }
        }
        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview()
        {
            var auxTematicas = new List<TematicaDto>();
            var auxMensajesError = new List<MensajeErrorDto>();



            auxMensajesError.Add(new MensajeErrorDto()
            {
                MensajeError = "Para la vigencia 2017, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"


            });
            auxMensajesError.Add(new MensajeErrorDto()
            {
                MensajeError = "Para la  vigencia 2018, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"


            });
            auxTematicas.Add(new TematicaDto()
            {
                Tematica = " Focalización x Actividades Actis",
                MensajesError = auxMensajesError.ToList()
            });
            auxTematicas.Add(new TematicaDto()
            {
                Tematica = " Focalización x Actividades Actis2",
                MensajesError = auxMensajesError.ToList()
            });


            return new ValidarViabilidadCompletarInfoDto()
            {
                Bpin = "2017011000236",
                Mensaje = "Mensaje Prueba",
                Tematicas = auxTematicas.ToList()
            };
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar, string usuario)
        {

        }

      
    }
}
