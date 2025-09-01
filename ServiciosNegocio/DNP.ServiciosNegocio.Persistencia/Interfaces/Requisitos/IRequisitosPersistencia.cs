namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Requisitos
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Requisitos;

    public interface IRequisitosPersistencia
    {
        List<Atributo> ObtenerRequisitos (string bPin, Guid nivelId, Guid instanciaId, Guid formularioId);
        void GuardarDefinitivamente(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar, string usuario);
    }
}
