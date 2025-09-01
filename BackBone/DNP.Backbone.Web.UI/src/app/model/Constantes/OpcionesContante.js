(function () {
    'use strict'

    angular.module('backbone.model')
        .constant('OpcionesContante', {
            //Usuarios
            UsuariosEditarUsuario: "Usuarios:EditarUsuario",
            UsuariosInvitarUsuario: "Usuarios:InvitarUsuario",
            UsuariosEliminarUsuario: "Usuarios:EliminarUsuario",

            //Monitoreo
            MonitoreoConfigCrearAlerta: "MonitoreoConfig:CrearAlerta",
            MonitoreoConfigEstado: "MonitoreoConfig:Estado",
            MonitoreoConfigActualizarAlerta: "MonitoreoConfig:ActualizarAlerta",
            MonitoreoConfigEliminarAlerta: "MonitoreoConfig:EliminarAlerta"

            //Perfiles

            //Roles

            //Consola Proyectos

            //Consola Tramites
        });
})();