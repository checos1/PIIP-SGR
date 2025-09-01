namespace DNP.EncabezadoPie.Servicios.Interfaces.DefinirAlcance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dominio.Dto.DefinirAlcance;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IDefinirAlcanceServicio
    {
        AlcanceDto ObtenerAlcance(ParametrosConsultaDto parametrosConsulta);
        AlcanceDto ObtenerAlcancePreview();
    }
}
