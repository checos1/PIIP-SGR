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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class PoliticaTransversalBeneficiarioPersistenciaMock : IPoliticaTransversalBeneficiarioPersistencia
    {
        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiario(string bpin)
        {
            var politicaTransversalBeneficiarioDto = new PoliticaTBeneficiarioDto();

            if (bpin.Equals("202000000000005"))
            {
                var auxVigencia = new List<VigenciaFPoliticaTBeneficiarioDto>();
                var auxBeneficiario = new List<LocalizacionPoliticaTBeneficiarioDto>();
                var auxLocalizaciones = new List<LocalizacioDto>();
                var auxFocalizacion = new List<FocalizacionPoliticaTBeneficiarioDto>();

                auxLocalizaciones.Add(new LocalizacioDto()
                {
                    LocalizacionId = 1204,
                    Ubicacion = "Orinoquía-Meta--",
                    Beneficiarios = 5
                });

                auxLocalizaciones.Add(new LocalizacioDto()
                {
                    LocalizacionId = 1205,
                    Ubicacion = "Orinoquía-Meta--",
                    Beneficiarios = 15
                });

                auxVigencia.Add(new VigenciaFPoliticaTBeneficiarioDto()
                {
                    Vigencia = 2017,
                    Total_PGN_Nacion = 0,
                    Total_PGN_Propios = 0,
                    Total_SGR = 0,
                    Total_Territorio = 1390000000,
                    Total_Empresa = 0,
                    Total_Privado = 0,
                    Localizaciones = auxLocalizaciones.OrderBy(ip => ip.LocalizacionId).ToList()
                });

                auxFocalizacion.Add(new FocalizacionPoliticaTBeneficiarioDto()
                {
                    PoliticaId = 11,
                    FocalizacionPoliticaId = 1,
                    Politica_Transversal = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                    Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
                });

                auxBeneficiario.Add(new LocalizacionPoliticaTBeneficiarioDto()
                {
                    Vigencias = new List<VigenciaPoliticaTBeneficiarioDto>()
                });

                return new PoliticaTBeneficiarioDto()
                {
                    ProyectoId = 40519,
                    BPIN = "2017005500186",
                    Beneficiarios = auxBeneficiario,
                    Focalizacion_Beneficiarios_y_Recursos = auxFocalizacion.OrderBy(v => v.PoliticaId).ToList()
                };
            }
            else
            {
                return new PoliticaTBeneficiarioDto();
            }
        }

        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiarioPreview()
        {
            var auxVigencia = new List<VigenciaFPoliticaTBeneficiarioDto>();
            var auxBeneficiario = new List<LocalizacionPoliticaTBeneficiarioDto>();
            var auxLocalizaciones = new List<LocalizacioDto>();
            var auxFocalizacion = new List<FocalizacionPoliticaTBeneficiarioDto>();

            auxLocalizaciones.Add(new LocalizacioDto()
            {
                LocalizacionId = 1204,
                Ubicacion = "Orinoquía-Meta--",
                Beneficiarios = 5
            });

            auxLocalizaciones.Add(new LocalizacioDto()
            {
                LocalizacionId = 1205,
                Ubicacion = "Orinoquía-Meta--",
                Beneficiarios = 15
            });

            auxVigencia.Add(new VigenciaFPoliticaTBeneficiarioDto()
            {
                Vigencia = 2017,
                Total_PGN_Nacion = 0,
                Total_PGN_Propios = 0,
                Total_SGR = 0,
                Total_Territorio = 1390000000,
                Total_Empresa = 0,
                Total_Privado = 0,
                Localizaciones = auxLocalizaciones.OrderBy(ip => ip.LocalizacionId).ToList()
            });

            auxFocalizacion.Add(new FocalizacionPoliticaTBeneficiarioDto()
            {
                PoliticaId = 11,
                FocalizacionPoliticaId = 1,
                Politica_Transversal = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
            });

            auxBeneficiario.Add(new LocalizacionPoliticaTBeneficiarioDto()
            {
                Vigencias = new List<VigenciaPoliticaTBeneficiarioDto>()
            });

            return new PoliticaTBeneficiarioDto()
            {
                ProyectoId = 40519,
                BPIN = "2017005500186",
                Beneficiarios = auxBeneficiario,
                Focalizacion_Beneficiarios_y_Recursos = auxFocalizacion.OrderBy(v => v.PoliticaId).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTBeneficiarioDto> parametrosGuardar, string usuario)
        {

        }
    }
}
