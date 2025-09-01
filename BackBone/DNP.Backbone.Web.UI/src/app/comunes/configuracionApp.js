(function () {
    'use strict';
    angular.module('backbone').constant("constantesBackbone", {
        apiBackboneServicioBaseUri: 'http://localhost:3240/',
        apiBackboneObtenerProyectos: 'api/Proyectos/ObtenerProyectoPorResponsable?usuarioResponsable=' + usuarioDNP,
        apiBackboneObtenerTramites: 'api/Proyectos/ObtenerTramitesPorResponsable?usuarioResponsable=' + usuarioDNP,
        apiBackboneActualizarPrioridadTramite: ' api/Proyectos/ActualizarPrioridadTramite',
        apiBackboneObtenerNotificacionesPorResponsable: 'api/Proyectos/ObtenerNotificacionesPorResponsable',
        apiBackboneActualizarEstadoProyecto: 'api/Proyectos/ActualizarEstadoProyecto',
        apiAutorizacionServicioBaseUri: 'http://localhost:10221/',
        apiBackboneObtenerProyectosEntidadesPorUsuario: 'api/BackboneMock/ObtenerProyectosEntidadesPorUsuarioRoles?usuarioDnp=',
        apiFlujosCrearInstancia: 'api/Flujo/CrearInstancia',
        apiFlujosObtenerFlujoPorId: 'api/Flujo/ObtenerFlujoPorId?flujoId=',
        apiEjecutarFlujo: 'api/Flujos/EjecutarFlujo'
    });
})();