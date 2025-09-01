(function () {
    'use strict';

    angular.module('backbone').factory('sesionServicios', sesionServicios);

    sesionServicios.$inject = ['$sessionStorage', '$localStorage'];

    function sesionServicios($sessionStorage, $localStorage) {
        return {
            setearPermisos: setearPermisos,
            setearUsuarioRoles: setearUsuarioRoles,
            obtenerPermisos: obtenerPermisos,
            obtenerUsuarioRoles: obtenerUsuarioRoles,
            obtenerUsuarioEntidades: obtenerUsuarioEntidades,
            obtenerUsuarioIdsRoles: obtenerUsuarioIdsRoles,
            obtenerUsuarioIdsEntidades: obtenerUsuarioIdsEntidades,
            tieneOpcionVinculada: tieneOpcionVinculada,
            tieneOpcionEnEntidad: tieneOpcionEnEntidad,
            tieneEntidad: tieneEntidad,
            obtenerPermisosMock: obtenerPermisosMock,
            eliminarTokens: eliminarTokens,
            limpiarUsuario: limpiarUsuario
        }

        ////////////      
        function limpiarUsuario() {
            var usuario = null;
            if ($sessionStorage)
                $sessionStorage.usuario = usuario;
        }

        function guardarUsuarioDatos(datos) {
            $sessionStorage.usuario = datos;
        }

        function setearUsuarioRoles(roles) {
            var usuario = null;
            if (!$sessionStorage.usuario)
                $sessionStorage.usuario = {};
            else
                usuario = $sessionStorage.usuario;

            $sessionStorage.usuario.roles = roles;
            guardarUsuarioDatos(usuario);
        }

        function setearPermisos(permisos) {
            var usuario = null;
            if (!$sessionStorage.usuario)
                $sessionStorage.usuario = {};
            else
                usuario = $sessionStorage.usuario;

            $sessionStorage.usuario.permisos = permisos;
            guardarUsuarioDatos(usuario);
        }

        function obtenerUsuarioRoles() {
            if (!$sessionStorage.usuario)
                $sessionStorage.usuario = {};
            if (!$sessionStorage.usuario.roles)
                $sessionStorage.usuario.roles = [];

            var usuario = $sessionStorage.usuario;
            return usuario.roles || [];
        }

        function obtenerPermisos() {
            if (!$sessionStorage.usuario)
                $sessionStorage.usuario = {};
            if (!$sessionStorage.usuario.permisos)
                $sessionStorage.usuario.permisos = [];

            var usuario = $sessionStorage.usuario;
            return usuario.permisos || [];
        }

        function obtenerPermisosMock() {
            return {
                IdUsuarioDNP: "Leticia01",
                OpcionesMenu: [
                    "Monitoreo",
                    "MonitoreoConfigAlertas",
                    "MonitoreoProyectos"
                ],
                Entidades: [
                    {
                        IdEntidad: "36b45420-9be2-46df-a77d-b58827f71c26",
                        EsSubEntidad: false,
                        NombreEntidad: "Boyacá",
                        Roles: [],
                        Opciones: [
                            {
                                Id: "dc53e45f-0dba-4b97-8449-454801e7bf93",
                                Nombre: "Crear nuevo Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:CrearAlerta",
                                Tipo: "Accion"
                            },
                            {
                                Id: "d71e9054-8651-4199-889a-49d10ea36dae",
                                Nombre: "Actualizar Estado del Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:Estado",
                                Tipo: "Accion"
                            },
                            {
                                Id: "d71e9054-8651-4199-889a-49d10ea323sdaw",
                                Nombre: "Actualizar Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:ActualizarAlerta",
                                Tipo: "Accion"
                            },
                            {
                                Id: "d71e9054-8651-4199-889a-49d10ea23dasda",
                                Nombre: "Eliminar Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:EliminarAlerta",
                                Tipo: "Accion"
                            }
                        ]
                    },
                    {
                        IdEntidad: "701109A8-5D83-4F13-B4A9-046B4D56B4AA",
                        EsSubEntidad: false,
                        NombreEntidad: "Chocó",
                        Roles: [],
                        Opciones: [
                            {
                                Id: "dc53e45f-0dba-4b97-8449-454801e7bf93",
                                Nombre: "Crear nuevo Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:CrearAlerta",
                                Tipo: "Accion"
                            },
                            {
                                Id: "d71e9054-8651-4199-889a-49d10ea36dae",
                                Nombre: "Actualizar Estado del Alerta",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "MonitoreoConfig:Estado",
                                Tipo: "Accion"
                            }
                        ]
                    },
                    {
                        IdEntidad: "6a81ac11-b826-4267-b4eb-2a2259bd2c3f",
                        EsSubEntidad: false,
                        NombreEntidad: "AGENCIA DE DESARROLLO RURAL-ADR",
                        Roles: [],
                        Opciones: [
                            //Usuarios
                            {
                                Id: "8760E9D5-740A-4E14-A5C6-754FD2622B84",
                                Nombre: "Usuarios: Invitar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:Invitar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "3C33CBE5-FE1B-4059-B7ED-85D48CE04BB3",
                                Nombre: "Usuarios: Cambiar Estado",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:CambiarEstado",
                                Tipo: "Accion"
                            },
                            {
                                Id: "C0D9BC9A-A1DE-4791-9179-9CA118C25FA2",
                                Nombre: "Usuarios: Editar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:Editar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "6A15C7C1-AE89-4211-B842-EF24C66FB906",
                                Nombre: "Usuarios: Eliminar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:Eliminar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "9A6CFC03-D2FF-4075-8204-A74B7269347D",
                                Nombre: "Usuarios: Listar Perfiles",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:ListarPerfiles",
                                Tipo: "Accion"
                            },
                            {
                                Id: "9954D17B-EAA5-44DA-B7A9-6FEDB23E5C92",
                                Nombre: "Usuarios: Asignar Perfiles",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:AsignarPerfiles",
                                Tipo: "Accion"
                            },
                            {
                                Id: "7A733395-3959-4AF7-9DAB-BB72AD7FB8FB",
                                Nombre: "Usuarios: Cambiar Estado Perfil",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:CambiarEstadoPerfil",
                                Tipo: "Accion"
                            },
                            {
                                Id: "3D41D159-EA94-4BAB-A4EF-D9EDB045C13D",
                                Nombre: "Usuarios: Add Proyectos Perfil",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Usuarios:AddProyectosPerfil",
                                Tipo: "Accion"
                            },

                            //Roles
                            {
                                Id: "1693F8F8-D0D2-4FD0-879E-0C6CEF6B1532",
                                Nombre: "Roles: Crear",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Roles:Crear",
                                Tipo: "Accion"
                            },
                            {
                                Id: "68EB669E-45AD-4506-9B44-B7DF743158AC",
                                Nombre: "Roles: Editar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Roles:Editar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "16C20075-A309-4ACE-9089-832E6B7B9822",
                                Nombre: "Roles: Eliminar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Roles:Eliminar",
                                Tipo: "Accion"
                            },

                            //Perfiles
                            {
                                Id: "A609E053-8985-4750-B2C8-4139CBD2E2E5",
                                Nombre: "Perfiles: Crear",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Perfiles:Crear",
                                Tipo: "Accion"
                            },
                            {
                                Id: "C2C6ACBC-5978-4D4E-B8D6-BDC454404AB3",
                                Nombre: "Perfiles: Editar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Perfiles:Editar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "416C9876-2AB3-41FE-9BFE-FB71FE4F7C18",
                                Nombre: "Perfiles: Eliminar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Perfiles:Eliminar",
                                Tipo: "Accion"
                            },

                            //ConsolaProyectos
                            {
                                Id: "FB76C235-6902-4F08-ACAB-1CC5C1BF99A2",
                                Nombre: "ConsolaProyectos: Ver Archivos",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaProyectos:VerArchivos",
                                Tipo: "Accion"
                            },
                            {
                                Id: "72332D98-05ED-4C35-AF8C-6A04EB2EEE4E",
                                Nombre: "ConsolaProyectos: Ver Fichas",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaProyectos:VerFichas",
                                Tipo: "Accion"
                            },
                            {
                                Id: "E55DC878-B265-40D7-A3A8-B998160D2EC0",
                                Nombre: "ConsolaProyectos: Cambiar Entidad",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaProyectos:CambiarEntidad",
                                Tipo: "Accion"
                            },

                            //ConsolaTramites
                            {
                                Id: "24FF8668-9840-408F-B543-D683D40E4BE1",
                                Nombre: "ConsolaProyectos: Ver Archivos",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaTramites:VerArchivos",
                                Tipo: "Accion"
                            },
                            {
                                Id: "6B96697B-C16B-4E54-89E3-75B053086446",
                                Nombre: "ConsolaProyectos: Ver Fichas",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaTramites:VerFichas",
                                Tipo: "Accion"
                            },
                            {
                                Id: "3AD8F69B-1772-4F0C-9F6E-A85A9C05A343",
                                Nombre: "ConsolaProyectos: Ver Proyectos",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "ConsolaTramites:VerProyectos",
                                Tipo: "Accion"
                            },

                            //Entidades
                            {
                                Id: "109E406A-4CAC-4E39-9712-4574E8F9FA18",
                                Nombre: "Entidades: Crear",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Crear",
                                Tipo: "Accion"
                            },
                            {
                                Id: "5C8AEFEA-72CF-4C48-8002-4CFF53EDBD8E",
                                Nombre: "Entidades: Crear Sub",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:CrearSub",
                                Tipo: "Accion"
                            },
                            {
                                Id: "CEF0537A-F318-43CA-8799-88CE01013B5A",
                                Nombre: "Entidades: Editar Matriz Flujos",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:EditarMatrizFlujos",
                                Tipo: "Accion"
                            },
                            {
                                Id: "EF0ADB88-7969-4630-AEDF-401486686367",
                                Nombre: "Entidades: Inflexibilidad",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Inflexibilidad",
                                Tipo: "Accion"
                            },
                            {
                                Id: "11A680D5-3ED2-4547-8ADD-5C298634F4DB",
                                Nombre: "Entidades: Adherencia",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Adherencia",
                                Tipo: "Accion"
                            },
                            {
                                Id: "383128B1-3606-4D97-A91C-38000CAA9CD3",
                                Nombre: "Entidades: Delegados",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Delegados",
                                Tipo: "Accion"
                            },
                            {
                                Id: "E645C128-98F1-4207-A7ED-E24D2CEB0DD8",
                                Nombre: "Entidades: Editar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Editar",
                                Tipo: "Accion"
                            },
                            {
                                Id: "6855840E-5AEE-447A-918B-62B7F9E4F46A",
                                Nombre: "Entidades: Eliminar",
                                IdOpcionPadre: null,
                                OpcionPadre: null,
                                IdOpcionDNP: "Entidades:Eliminar",
                                Tipo: "Accion"
                            }
                        ]
                    }
                ]
            }
        }

        function obtenerUsuarioIdsRoles() {
            return this.obtenerUsuarioRoles().map(function (rol) {
                return rol.IdRol;
            });
        }

        function tieneOpcionVinculada(idOpcionDNP) {
            var tienePermiso = false;
            var permisos = obtenerPermisos();

            permisos.Entidades.some(function (entidad) {
                entidad.Opciones.some(function (opcion) {
                    return tienePermiso = opcion.IdOpcionDNP == idOpcionDNP;
                });
                return tienePermiso;
            });

            return tienePermiso;
        }

        function tieneOpcionEnEntidad(idOpcionDNP, idEntidad) {
            var tienePermiso = false;
            var permisos = obtenerPermisos();

            var entidad = permisos.Entidades.find(e => e.IdEntidad == idEntidad);
            if (!entidad)
                throw "Entidad no encontrada.";

            entidad.Opciones.some(function (opcion) {
                return tienePermiso = opcion.IdOpcionDNP == idOpcionDNP;
            });

            return tienePermiso;
        }

        function tieneEntidad(idEntidad) {
            var permisos = obtenerPermisos();
            var entidad = permisos.Entidades.find(e => e.IdEntidad == idEntidad);
            return !!entidad;
        }

        function obtenerUsuarioEntidades() {
            const permisos = this.obtenerPermisos()
            return permisos && permisos.Entidades || [];
        }

        function obtenerUsuarioIdsEntidades() {
            const entidades = this.obtenerUsuarioEntidades()
            return entidades.map(entidad => entidad.IdEntidad);
        }

        function eliminarTokens(){
            $localStorage.authorizationData = null;
            _eliminarCookie("PIIPAuthCookie");
        }

        function _eliminarCookie (name) {
            document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
        };
    }

})();