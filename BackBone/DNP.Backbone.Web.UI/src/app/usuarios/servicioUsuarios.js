(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.usuarios').factory('servicioUsuarios', servicioUsuarios);

    servicioUsuarios.$inject = ['$http', 'constantesBackbone', '$q', 'utilidades'];

    function servicioUsuarios($http, constantesBackbone, $q, utilidades) {

        return {
            obtenerUsuarios: obtenerUsuarios,
            obtenerUsuariosPorEntidad: obtenerUsuariosPorEntidad,
            obtenerUsuariosXEntidad: obtenerUsuariosXEntidad,
            obtenerUsuariosXEntidadTerritorio: obtenerUsuariosXEntidadTerritorio,
            invitarUsuario: invitarUsuario,
            crearUsuarioPerfil: crearUsuarioPerfil,
            obtenerTodosPerfiles: obtenerTodosPerfiles,
            ObtenerTodasEntidades: ObtenerTodasEntidades,
            ObtenerUsuarioDominio: obtenerUsuarioDominio,
            autorizacionObtenerUsuarioPorIdUsuarioDnp: autorizacionObtenerUsuarioPorIdUsuarioDnp,
            autorizacionObtenerUsuarioPorCorreoDNP: autorizacionObtenerUsuarioPorCorreoDNP,
            autorizacionObtenerDatosUsuarioPIIP: autorizacionObtenerDatosUsuarioPIIP,
            identidadObtenerUsuarioPorId: identidadObtenerUsuarioPorId,
            apiIdentidadVerificarExistenciaUsuarioSTSAplicacion: apiIdentidadVerificarExistenciaUsuarioSTSAplicacion,
            apiIdentidadObtenerAplicacionesExistenciaUsuarioSTS: apiIdentidadObtenerAplicacionesExistenciaUsuarioSTS,
            apiIdentidadObtenerAplicacionesConfiablesExistenciaUsuarioSTS: apiIdentidadObtenerAplicacionesConfiablesExistenciaUsuarioSTS,
            validarContrasenaActualSTS: validarContrasenaActualSTS,
            cambiarContrasenaSTS: cambiarContrasenaSTS,
            registrarUsuarioAPPSTS: registrarUsuarioAPPSTS,
            registrarUsuarioSTS: registrarUsuarioSTS,
            registrarUsuarioPIIP: registrarUsuarioPIIP,
            registrarUsuarioTerritorioPIIP: registrarUsuarioTerritorioPIIP,
            enviarCorreoInvitacionSTS: enviarCorreoInvitacionSTS,
            obtenerDatosUsuario: obtenerDatosUsuario,
            obtenerPerfilesUsuario: obtenerPerfilesUsuario,
            obtenerRolesDePerfil: obtenerRolesDePerfil,
            obtenerProyectosDePerfil: obtenerProyectosDePerfil,
            obtenerProyectosPorEntidad: obtenerProyectosPorEntidad,
            asociarProyectosAUsuarioPerfil: asociarProyectosAUsuarioPerfil,
            obtenerPerfiles: obtenerPerfiles,
            obtenerPerfilesPorUsuario: obtenerPerfilesPorUsuario,
            obtenerPerfilesPorUsuarioXUsuarioAutenticado: obtenerPerfilesPorUsuarioXUsuarioAutenticado,
            obtenerListadoPerfilesXEntidadYUsuario: obtenerListadoPerfilesXEntidadYUsuario,
            obtenerListadoPerfilesXUsuarioTerritorio: obtenerListadoPerfilesXUsuarioTerritorio,
            obtenerExcelPerfiles: obtenerExcelPerfiles,
            obtenerExcelRoles: obtenerExcelRoles,
            obtenerRoles: obtenerRoles,
            guardarPerfil: guardarPerfil,
            guardarUsuario: guardarUsuario,
            guardarUsuarioTerritorio: guardarUsuarioTerritorio,
            setActivoUsuarioPerfilPorEntidad: setActivoUsuarioPerfilPorEntidad,
            setActivoUsuarioPerfil: setActivoUsuarioPerfil,
            setActivoUsuarioEntidad: setActivoUsuarioEntidad,
            eliminarPerfil: eliminarPerfil,
            eliminarUsuarioPerfil: eliminarUsuarioPerfil,
            imprimirPdfPerfiles: imprimirPdfPerfiles,
            imprimirPdfRoles: imprimirPdfRoles,
            guardarRol: guardarRol,
            eliminarRol: eliminarRol,
            obtenerOpciones: obtenerOpciones,
            obtenerOpcionesDeRol: obtenerOpcionesDeRol,
            obtenerRolesPorAplicacion: obtenerRolesPorAplicacion,
            obtenerEntidades: obtenerEntidades,
            obtenerPerfilesPorAplicacion: obtenerPerfilesPorAplicacion,
            obtenerPerfilesAutorizadosPorAplicacion: ObtenerPerfilesAutorizadosPorAplicacion,
            obtenerJsonLocal: obtenerJsonLocal,
            obtenerProyectos: obtenerProyectos,
            guardarProyectos: guardarProyectos,
            cambiarContrasena: cambiarContrasena,
            obtenerUsuarioPorNombre: obtenerUsuarioPorNombre,

            obtenerUsuariosPorEntidadPdf: obtenerUsuariosPorEntidadPdf,
            imprimirPdf: imprimirPdf,
            obtenerExcel: obtenerExcel,

            obtenerExcelNotificaciones: obtenerExcelNotificaciones,
            obtenerUsuarioPorUsuarioDNP: obtenerUsuarioPorUsuarioDNP,
            validarPermisoInactivarUsuario: validarPermisoInactivarUsuario
        }

        function obtenerUsuarios() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuarios;
            return $http.get(url);
        }

        /**
         * 
         * @description . Obtiene la lista de usuarios registrados por tipo de entidad.
         * @param {String} tipoEntidad . Tipo de entidad del usuario actual
         * @param {object} filtroExtra. Filtros para la consulta { NomberUsuario: String, CuentaUsuario: String, Estado: bool } 
         */
        function obtenerUsuariosPorEntidad(tipoEntidad, filtro, filtroExtra) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosPorEntidad + tipoEntidad + '&filtro=' + filtro;
            var params = { filtroExtra: JSON.stringify(filtroExtra) };
            return $http.get(url, { /*data*/ params: params });
        }

        function obtenerUsuariosXEntidad(tipoEntidad, filtro, filtroExtra) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosXEntidad + tipoEntidad + '&filtro=' + filtro;
            var params = { filtroExtra: JSON.stringify(filtroExtra) };
            return $http.get(url, { params: params });
        }        

        function obtenerUsuariosXEntidadTerritorio(tipoEntidad, filtro, filtroExtra) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosXEntidadTerritorio + tipoEntidad + '&filtro=' + filtro;
            var params = { filtroExtra: JSON.stringify(filtroExtra) };
            return $http.get(url, { params: params });
        }

        function obtenerDatosUsuario(usuarioGuid, usuarioDnp) {
            const parametros = usuarioDnp && `${usuarioGuid}&usuarioDnp=${usuarioDnp}` || usuarioGuid;
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerDatosUsuario}${parametros}`;
            return $http.get(url);
        }

        function obtenerPerfilesUsuario() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfilesUsuario + usuarioGuid;
            return $http.get(url);
        }

        function obtenerRolesDePerfil(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerRolesDePerfil;
            return $http.get(url,
                {
                    params:
                    {
                        usuarioDnp: parametros.usuario,
                        idPerfil: parametros.idPerfil
                    }
                });
        }

        function obtenerProyectosDePerfil(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosDePerfil;
            return $http.get(url,
                {
                    params:
                    {
                        usuarioDnp: parametros.usuario,
                        idUsuarioPerfil: parametros.idUsuarioPerfil
                    }
                });
        }

        function obtenerProyectosPorEntidad(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPorEntidad + '&idEntidad=' + parametros.idEntidad;
            return $http.get(url);
        }

        function asociarProyectosAUsuarioPerfil(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAsociarProyectosAUsuarioPerfil;
            return $http.post(url, parametros);
        }

        function obtenerPerfiles(perfilFiltro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfiles + perfilFiltro;
            console.log(url);
            return $http.get(url);
        }

        function obtenerExcelPerfiles(perfilFiltro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelPerfiles + '?perfilFiltro=' + perfilFiltro;
            return $http.post(url, { perfilFiltro: perfilFiltro }, { responseType: 'arraybuffer' });
        }

        function obtenerExcelRoles(rolFiltro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelRoles + '?rolFiltro=' + rolFiltro;
            return $http.post(url, { rolFiltro: rolFiltro }, { responseType: 'arraybuffer' });
        }

        function obtenerPerfilesPorUsuario(tipoEntidad, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfilesUsuario + tipoEntidad;
            if (idUsuario) {
                url += '&idUsuario=' + idUsuario;
            }

            return $http.get(url);
        }

        function obtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfilesUsuarioXUsuarioAutenticado;
            url = url.replace('{{tipoEntidad}}', tipoEntidad)
            if (idUsuario) {
                url += '&idUsuario=' + idUsuario;
            }

            return $http.get(url);
        }

        function obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListadoPerfilesXEntidadYUsuario;
            url = url.replace('{{idEntidad}}', idEntidad)
            url = url.replace('{{idUsuario}}', idUsuario)

            return $http.get(url);
        }

        function obtenerListadoPerfilesXUsuarioTerritorio(idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListadoPerfilesXUsuarioTerritorio;
            url = url.replace('{{idUsuario}}', idUsuario)

            return $http.get(url);
        }

        function obtenerRoles(roleFiltro) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerRoles + '?roleFiltro=' + roleFiltro;
            return $http.get(url);
        }

        function obtenerRolesPorAplicacion(rolFiltro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerRolesPorAplicacion + rolFiltro;
            return $http.get(url);
        }

        function guardarRol(rolDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarRol;
            return $http.post(url, rolDto);
        }

        function eliminarRol(perfilDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarRol;
            return $http.post(url, perfilDto);
        }

        function obtenerOpciones() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerOpciones;
            return $http.get(url);
        }

        function obtenerOpcionesDeRol(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerOpcionesDeRol;
            return $http.get(url,
                {
                    params:
                    {
                        usuarioDnp: parametros.usuario,
                        idRol: parametros.idRol
                    }
                });
        }

        function guardarPerfil(perfilDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPerfil;
            return $http.post(url, perfilDto);
        }

        function guardarUsuario(usuarioDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEditarUsuario;
            return $http.post(url, usuarioDto);
        }

        function guardarUsuarioTerritorio(usuarioDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEditarUsuarioTerritorio;
            return $http.post(url, usuarioDto);
        }

        function setActivoUsuarioPerfilPorEntidad(idUsuario, idEntidad, activo, idUsuarioDNP, tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSetActivoUsuarioPerfilPorEntidad;
            return $http.post(url, { idUsuario, idEntidad, activo, usuarioDNP: usuarioDNP, IdUsuarioDNP: idUsuarioDNP, TipoEntidad: tipoEntidad });
        }

        function setActivoUsuarioPerfil(idUsuarioPerfil, activo, idUsuarioDnp, idEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSetActivoUsuarioPerfil;
            return $http.post(url, { idUsuarioPerfil, activo, usuarioDNP: idUsuarioDnp, idEntidad: idEntidad });
        }

        function setActivoUsuarioEntidad(idUsuario, idEntidad, activo, idUsuarioDNP, tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSetActivoUsuarioEntidad;
            return $http.post(url, { idUsuario, idEntidad, activo, usuarioDNP: usuarioDNP, IdUsuarioDNP: idUsuarioDNP, TipoEntidad: tipoEntidad });
        }

        function eliminarPerfil(perfilDto) {
            perfilDto.UsuarioDNP = usuarioDNP;
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarPerfil;
            return $http.post(url, perfilDto);
        }

        function eliminarUsuarioPerfil(idUsuarioPerfil, idEntidad, idUsuarioDNP) {
            var usuarioDto = {
                IdUsuarioPerfil: idUsuarioPerfil,
                IdEntidad: idEntidad,
                IdUsuarioDnp: idUsuarioDNP
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarPerfilUsuario;
            return $http.post(url, usuarioDto);
        }

        function imprimirPdfPerfiles(listPerfiles) {
            var url = urlPDFBackbone + constantesBackbone.apiBackbonePerfilesImprimirPDF;
            return $http.post(url, listPerfiles, { responseType: 'blob' });
        }

        function obtenerEntidades(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesInviteUsuarios;
            return $http.get(url, {
                params:
                {
                    tipoEntidad: tipoEntidad
                }
            });
        }

        function obtenerPerfilesPorAplicacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfilesPorAplicacion;
            return $http.get(url,
                {
                    params:
                    {
                        usuarioDnp: parametros.usuario,
                        aplicacion: parametros.idAplicacion
                    }
                });
        }

        function ObtenerPerfilesAutorizadosPorAplicacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPerfilesAutorizadosPorAplicacion;
            return $http.get(url, {
                params:
                {
                    usuarioDnp: parametros.usuario,
                    aplicacion: parametros.idAplicacion
                }
            });
        }

        function registrarUsuarioPIIP(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneRegistrarUsuarioPIIP;
            return $http.post(url, parametros);
        }

        function registrarUsuarioTerritorioPIIP(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneRegistrarUsuarioTerritorioPIIP;
            return $http.post(url, parametros);
        }

        function invitarUsuario(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInvitarUsuario;
            return $http.post(url, parametros);
        }

        function crearUsuarioPerfil(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearUsuarioPerfil;
            return $http.post(url, parametros);
        }

        function obtenerTodosPerfiles() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTodosPerfiles;
            return $http.get(url, { UsuarioDNP: usuarioDNP });
        }

        function ObtenerTodasEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTodasEntidades;
            return $http.get(url, { UsuarioDNP: usuarioDNP });
        }

        function imprimirPdfRoles(listRoles) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneRolesImprimirPDF;
            return $http.post(url, listRoles, { responseType: 'blob' });
        }

        //--------p/ mock------------//
        function obtenerJsonLocal(nombreJson) {
            var url = 'http://localhost:3024/src/assets/' + nombreJson + '.json';

            return $http({
                method: 'GET',
                'Content-Type': 'application/json;charset=utf-8',
                url: url
            });
        }

        //TODO remover ao integrar com o backend
        function obtenerProyectos() {

            return $q(function (resolve, reject) {
                var result = [{
                    CodigoBpin: '00000000001',
                    ProyectoId: 1,
                    ProyectoNombre: "Proyecto teste 1",
                    ProyectoBpin: "Proyecto teste 1 - 00000000001"
                },
                {
                    CodigoBpin: '00000000002',
                    ProyectoId: 2,
                    ProyectoNombre: "Proyecto teste 2",
                    ProyectoBpin: "Proyecto teste 2 - 00000000002"
                },
                {
                    CodigoBpin: '00000000003',
                    ProyectoId: 3,
                    ProyectoNombre: "Proyecto teste 3",
                    ProyectoBpin: "Proyecto teste 3 - 00000000003"
                }];

                resolve(result);
            });

        }

        //TODO remover ao integrar com o backend
        function guardarProyectos() {

            return $q(function (resolve, reject) {
                var result = {
                    data: {
                        Exito: true
                    }
                };

                resolve(result);
            });

        }

        function cambiarContrasena(credencialUsuarioDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCambiarClaveUsuario;
            return $http.post(url, credencialUsuarioDto);
        }

        function obtenerUsuariosPorEntidadPdf(tipoEntidad, filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosPorEntidadPdf + tipoEntidad + '&filtro=' + JSON.stringify(filtro);
            return $http.get(url);
        }

        function imprimirPdf(dto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneUsuariosImprimirPDF;
            return $http.post(url, dto, { responseType: 'blob' });
        }

        function obtenerExcel(tipoEntidad, filtro) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelUsuarios + tipoEntidad + '&filtro=' + JSON.stringify(filtro);
            return $http.post(url, JSON.stringify(filtro), { responseType: 'arraybuffer' });
        }

        function obtenerUsuarioPorNombre(nombre) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerUsuariosPorNombre}${nombre}`;
            return $http.get(url);
        }

        function obtenerExcelNotificaciones(notificaciones) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelNotificaciones;
            return $http.post(url, notificaciones, {
                responseType: 'arraybuffer'
            });
        }

        function autorizacionObtenerUsuarioPorIdUsuarioDnp(usuario) {

            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiAutorizacionObtenerUsuarioPorIdUsuarioDnp}${usuario}`;
            return $http.get(url);
        }

        function autorizacionObtenerUsuarioPorCorreoDNP(correo) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiAutorizacionObtenerUsuarioPorCorreoDNP}${correo}`;
            return $http.get(url);
        }

        function autorizacionObtenerDatosUsuarioPIIP(usuario) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiAutorizacionObtenerDatosUsuarioPorNombre}${usuario}`;
            return $http.get(url);
        }

        function identidadObtenerUsuarioPorId(usuario) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadObtenerUsuarioPorId}${usuario}`;
            return $http.get(url);
        }

        function apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(aplicacion, tipoDocumento, numeroDocumento) {
            let ruta = constantesBackbone.apiIdentidadVerificarExistenciaUsuarioSTSAplicacion
            ruta = utilidades.replaceAll(ruta, '{{aplicacion}}', aplicacion == 'PIIP' ? constantesBackbone.keySTSApplication : constantesBackbone.keySTSApplicationMGA)
            ruta = utilidades.replaceAll(ruta, '{{tipoDocumento}}', tipoDocumento)
            ruta = utilidades.replaceAll(ruta, '{{numeroDocumento}}', numeroDocumento)
            let url = `${apiBackboneServicioBaseUri}${ruta}`;
            console.log(url)
            return $http.get(url);
        } 

        function apiIdentidadObtenerAplicacionesExistenciaUsuarioSTS(tipoDocumento, numeroDocumento) {
            let ruta = constantesBackbone.apiIdentidadObtenerAplicacionesExistenciaUsuarioSTS
            ruta = utilidades.replaceAll(ruta, '{{aplicacion}}', constantesBackbone.keySTSApplication)
            ruta = utilidades.replaceAll(ruta, '{{tipoDocumento}}', tipoDocumento)
            ruta = utilidades.replaceAll(ruta, '{{numeroDocumento}}', numeroDocumento)
            let url = `${apiBackboneServicioBaseUri}${ruta}`;
            console.log(url)
            return $http.get(url);
        }

        function apiIdentidadObtenerAplicacionesConfiablesExistenciaUsuarioSTS(tipoDocumento, numeroDocumento) {
            let ruta = constantesBackbone.apiIdentidadObtenerAplicacionesConfiablesExistenciaUsuarioSTS
            ruta = utilidades.replaceAll(ruta, '{{aplicacion}}', constantesBackbone.keySTSApplication)
            ruta = utilidades.replaceAll(ruta, '{{tipoDocumento}}', tipoDocumento)
            ruta = utilidades.replaceAll(ruta, '{{numeroDocumento}}', numeroDocumento)
            let url = `${apiBackboneServicioBaseUri}${ruta}`;
            console.log(url)
            return $http.get(url);
        }

        function registrarUsuarioAPPSTS(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadRegistrarUsuarioAPPSTS}`;
            return $http.post(url, parametros);
        }

        function validarContrasenaActualSTS(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadValidarContrasenaActualSTS}`;
            return $http.post(url, parametros);
        }

        function cambiarContrasenaSTS(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadCambiarContrasenaSTS}`;
            return $http.post(url, parametros);
        }

        function registrarUsuarioSTS(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadRegistrarUsuarioSTS}`;
            return $http.post(url, parametros);
        }

        function enviarCorreoInvitacionSTS(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiIdentidadEnviarCorreoInvitacionSTS}`;
            return $http.post(url, parametros);
        }

        function obtenerUsuarioDominio(nombre) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerUsuarioDominio}${nombre}`;
            return $http.get(url);
        }

        function obtenerUsuarioPorUsuarioDNP() {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerUsuarioPorIdUsuarioDnp}`;
            return $http.get(url);
        }

        function validarPermisoInactivarUsuario(usuarioDnp, usuarioDnpEliminar) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonevalidarPermisoInactivarUsuario + '?usuarioDnp=' + usuarioDnp + '&usuarioDnpEliminar=' + usuarioDnpEliminar;
            return $http.get(url);
        }

        
    }
})();