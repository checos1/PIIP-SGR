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
    public class PoliticaTransversalCategoriaPersistenciaMock : IPoliticaTransversalCategoriaPersistencia
    {
        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoria(string bpin)
        {
            var politicaTransversalCategoriaDto = new PoliticaTCategoriasDto();

            if (bpin.Equals("202000000000005"))
            {               
                var auxPoliticas = new List<PoliticaCategoriaDto>();
                var auxCategorias = new List<CategoriaDto>();
                var auxVigencia = new List<VigenciaDto>();
                var auxLocalizaciones = new List<LocalizacionDto>();

                auxLocalizaciones.Add(new LocalizacionDto()
                {
                    LocalizacionId = 1204,
                    Ubicacion = "Orinoquía-Meta--",
                    Beneficiarios = 5
                });

                auxLocalizaciones.Add(new LocalizacionDto()
                {
                    LocalizacionId = 1205,
                    Ubicacion = "Orinoquía-Meta--",
                    Beneficiarios = 15
                });

                auxVigencia.Add(new VigenciaDto()
                {
                    Vigencia = 2020,
                    Localizacion = auxLocalizaciones.OrderBy(ip => ip.LocalizacionId).ToList()
                });

                auxCategorias.Add(new CategoriaDto()
                {
                    CategoriaId = 30,
                    NombreCategoria = "CRIC",
                    Vigencias = auxVigencia
                });

                auxPoliticas.Add(new PoliticaCategoriaDto()
                {
                    PoliticaId = 11,
                    NombrePolitica = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                    Categorias = auxCategorias.OrderBy(p => p.CategoriaId).ToList()
                });

                return new PoliticaTCategoriasDto()
                {
                    ProyectoId = 40519,
                    BPIN = "2017005500186",
                    Politicas = auxPoliticas.OrderBy(p => p.PoliticaId).ToList()
                };
            }
            else
            {
                return new PoliticaTCategoriasDto();
            }
        }

        public PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoriaPreview()
        {
            var auxPoliticas = new List<PoliticaCategoriaDto>();
            var auxCategorias = new List<CategoriaDto>();
            var auxVigencia = new List<VigenciaDto>();
            var auxLocalizaciones = new List<LocalizacionDto>();

            auxLocalizaciones.Add(new LocalizacionDto()
            {
                LocalizacionId = 1204,
                Ubicacion = "Orinoquía-Meta--",
                Beneficiarios = 5
            });

            auxLocalizaciones.Add(new LocalizacionDto()
            {
                LocalizacionId = 1205,
                Ubicacion = "Orinoquía-Meta--",
                Beneficiarios = 15
            });

            auxVigencia.Add(new VigenciaDto()
            {
                Vigencia = 2020,
                Localizacion = auxLocalizaciones.OrderBy(ip => ip.LocalizacionId).ToList()
            });

            auxCategorias.Add(new CategoriaDto()
            {
                CategoriaId = 30,
                NombreCategoria = "CRIC",
                Vigencias = auxVigencia
            });

            auxPoliticas.Add(new PoliticaCategoriaDto()
            {
                PoliticaId = 11,
                NombrePolitica = "GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA",
                Categorias = auxCategorias.OrderBy(p => p.CategoriaId).ToList()
            });

            return new PoliticaTCategoriasDto()
            {
                ProyectoId = 40519,
                BPIN = "2017005500186",
                Politicas = auxPoliticas.OrderBy(p => p.PoliticaId).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTCategoriasDto> parametrosGuardar, string usuario)
        {

        }
    }
}
