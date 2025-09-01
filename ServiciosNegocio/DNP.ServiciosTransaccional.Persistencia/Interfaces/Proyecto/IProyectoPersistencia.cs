namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Proyecto
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;

    public interface IProyectoPersistencia
    {
        object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
        object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
        string GetInstaciasProyectoSGP(string ObjetoNegocioId);
        object PostRecuperaDatosSGP(string idInstanciaAnterior, string idInstanciaDestino);
    }
}
