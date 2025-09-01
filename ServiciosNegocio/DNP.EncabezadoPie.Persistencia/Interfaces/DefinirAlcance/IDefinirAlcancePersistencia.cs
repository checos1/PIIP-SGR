namespace DNP.EncabezadoPie.Persistencia.Interfaces.DefinirAlcance
{
    using Dominio.Dto.DefinirAlcance;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDefinirAlcancePersistencia
    {
        AlcanceDto ObtenerAlcance(string bpin);
        AlcanceDto ObtenerAlcancePreview();
    }
}
