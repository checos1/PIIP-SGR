
(function () {
    'use strict';
    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').constant("constantesAutorizacion", {
        // Configuraciones Rol-Sector
        apiBackboneObtenerConfiguracionesRolSector: 'api/AutorizacionNegocio/ObtenerConfiguracionesRolSector',
        apiBackboneObtenerRolesPorEntidadTerritorial: 'api/AutorizacionNegocio/ObtenerRolesPorEntidadTerritorial',
        apiBackboneObtenerSectoresPorEntidadTerritorial: 'api/AutorizacionNegocio/ObtenerSectoresPorEntidadTerritorial',
        apiBackboneObtenerEntidadesPorSectorTerritorial: 'api/AutorizacionNegocio/ObtenerEntidadesPorSectorTerritorial',
        apiBackboneGuardarConfiguracionRolSector: 'api/AutorizacionNegocio/GuardarConfiguracionRolSector',
        apiBackboneEditarConfiguracionRolSector: 'api/AutorizacionNegocio/EditarConfiguracionRolSector',
        apiBackboneCambiarEstadoConfiguracionRolSector: 'api/AutorizacionNegocio/CambiarEstadoConfiguracionRolSector',

        tipoEntidadNacional: 'Nacional',
        tipoEntidadTerritorial: 'Territorial',
        tipoEntidadSGR: 'SGR',
        tipoEntidadPrivadas: 'Privadas',
        tipoEntidadPublicas: 'Publicas',

        /*opciones botones y campos*/
        btnOcultarAccionesProyecto : 'Inactivar conjunto acciones  proyectos_accion',
        btnNoMostrarBpin: 'No mostrar valor BPIN',

        idEtapaViabilidadRegistro: 'D8C0C353-3EA7-4BC5-9CA6-CC8AA0F7BB8B'
    });
})();


