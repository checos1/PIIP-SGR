namespace DNP.ServiciosWBS.Persistencia.Interfaces.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public interface IFasePersistencia
    {
        FaseDto ObtenerFaseByGuid(string guid);
    }
}
