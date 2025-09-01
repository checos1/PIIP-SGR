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
    public class AjustesPoliticaTransversalBeneficiarioPersistenciaMock : IAjustesPoliticaTransversalBeneficiarioPersistencia
    {
        public AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiario(string bpin)
        {
            var politicaTransversalBeneficiarioDto = new AjustesPoliticaTBeneficiarioDto();

            if (bpin.Equals("202000000000005"))
            {
                var auxVigencia = new List<VigenciaFAjustesPoliticaTBeneficiarioDto>();
                var auxBeneficiario = new List<BeneficiarioAjustesPoliticaTBeneficiarioDto>();
                var auxLocalizaciones = new List<LocalizacioFAjustesPoliticaTBeneficiarioDto>();
                var auxFocalizacion = new List<FocalizacionAjustesPoliticaTBeneficiarioDto>();

                auxLocalizaciones.Add(new LocalizacioFAjustesPoliticaTBeneficiarioDto()
                {
                    LocalizacionId = 1204,
                    Ubicacion = "Orinoquía-Meta--",
                    BeneficiariosenFirme = 5,
                    BeneficiariosenAjuste = 10
                });

                auxLocalizaciones.Add(new LocalizacioFAjustesPoliticaTBeneficiarioDto()
                {
                    LocalizacionId = 1205,
                    Ubicacion = "Orinoquía-Meta--",
                    BeneficiariosenFirme = 15,
                    BeneficiariosenAjuste = 10
                });

                auxVigencia.Add(new VigenciaFAjustesPoliticaTBeneficiarioDto()
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

                auxFocalizacion.Add(new FocalizacionAjustesPoliticaTBeneficiarioDto()
                {
                    PoliticaId = 11,
                    FocalizacionPoliticaId = 1,
                    Politica_Transversal = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                    Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
                });

                auxBeneficiario.Add(new BeneficiarioAjustesPoliticaTBeneficiarioDto()
                {
                    Vigencias = new List<VigenciaAjustesPoliticaTBeneficiarioDto>()
                });

                return new AjustesPoliticaTBeneficiarioDto()
                {
                    ProyectoId = 40519,
                    BPIN = "2017005500186",
                    Beneficiarios = auxBeneficiario,
                    Focalizacion_Beneficiarios_y_Recursos = auxFocalizacion.OrderBy(v => v.PoliticaId).ToList()
                };
            }
            else
            {
                return new AjustesPoliticaTBeneficiarioDto();
            }
        }

        public AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiarioPreview()
        {
            var auxVigencia = new List<VigenciaFAjustesPoliticaTBeneficiarioDto>();
            var auxBeneficiario = new List<BeneficiarioAjustesPoliticaTBeneficiarioDto>();
            var auxLocalizaciones = new List<LocalizacioFAjustesPoliticaTBeneficiarioDto>();
            var auxFocalizacion = new List<FocalizacionAjustesPoliticaTBeneficiarioDto>();

            auxLocalizaciones.Add(new LocalizacioFAjustesPoliticaTBeneficiarioDto()
            {
                LocalizacionId = 1204,
                Ubicacion = "Orinoquía-Meta--",
                BeneficiariosenFirme = 5,
                BeneficiariosenAjuste = 10
            });

            auxLocalizaciones.Add(new LocalizacioFAjustesPoliticaTBeneficiarioDto()
            {
                LocalizacionId = 1205,
                Ubicacion = "Orinoquía-Meta--",
                BeneficiariosenFirme = 5,
                BeneficiariosenAjuste = 10
            });

            auxVigencia.Add(new VigenciaFAjustesPoliticaTBeneficiarioDto()
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

            auxFocalizacion.Add(new FocalizacionAjustesPoliticaTBeneficiarioDto()
            {
                PoliticaId = 11,
                FocalizacionPoliticaId = 1,
                Politica_Transversal = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                Vigencias = auxVigencia.OrderBy(v => v.Vigencia).ToList()
            });

            auxBeneficiario.Add(new BeneficiarioAjustesPoliticaTBeneficiarioDto()
            {
                Vigencias = new List<VigenciaAjustesPoliticaTBeneficiarioDto>()
            });

            return new AjustesPoliticaTBeneficiarioDto()
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

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTBeneficiarioDto> parametrosGuardar, string usuario)
        {

        }
    }
}
