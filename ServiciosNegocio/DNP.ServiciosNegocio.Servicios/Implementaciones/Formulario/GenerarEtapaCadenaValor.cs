using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario
{
    public class GenerarEtapaCadenaValor
    {
        public EtapaDto Etapa { get; set; }

        public GenerarEtapaCadenaValor(uspGetCadenaValor_Result cadenaValor, VigenciaDto vigencia)
        {
            Etapa = new EtapaDto();

            vigencia.GranTotalPorVigencia = vigencia.GranTotalPorVigencia + cadenaValor.TotalEtapaApropiacionVigente;

            Etapa.Id = cadenaValor.ActividadEtapaId;
            Etapa.NombreEtapa = cadenaValor.ActividadEtapa;
            Etapa.TotalEtapaValorSolicitado = cadenaValor.TotalEtapaValorSolicitado;
            Etapa.TotalEtapaApropiacionInicial = cadenaValor.TotalEtapaApropiacionInicial;
            Etapa.TotalEtapaApropiacionVigente = cadenaValor.TotalEtapaApropiacionVigente;
            Etapa.Actividades = new List<ActividadDto>();
        }
    }
}
