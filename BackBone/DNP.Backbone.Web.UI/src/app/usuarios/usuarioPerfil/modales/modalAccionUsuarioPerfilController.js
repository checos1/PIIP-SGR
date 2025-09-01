(function () {
    'use strict';

    modalAccionUsuarioPerfilController.$inject = [
        'obj',
        '$uibModalInstance',
        'servicioUsuarios',
        'servicioEntidades',
        'autorizacionServicios',
        'utilidades'
    ];

    function modalAccionUsuarioPerfilController(
        obj,
        $uibModalInstance,
        servicioUsuarios,
        servicioEntidades,
        autorizacionServicios,
        utilidades
    ) {
        var vm = this;

        vm.idUsuario = obj.idUsuario;
        vm.idUsuarioDnp = obj.idUsuarioDnp;
        vm.obtenerDatosPerfilUsuario = obj.obtenerDatosPerfilUsuario;

        vm.idPerfil = null;
        vm.listaPerfiles = [];

        //vm.tipoEntidad = null;
        //vm.listadoTipoEntidades = [];

        vm.idEntidad = null;
        vm.listaEntidades = [];
        vm.listadoEntidades = [];
        

        vm.cerrar = $uibModalInstance.dismiss;
        //vm.onChangeTipoEntidad = onChangeTipoEntidad;
        vm.obtenerListadoEntidadesXUsuarioAutenticado = obtenerListadoEntidadesXUsuarioAutenticado;
        vm.onChangeEntidad = onChangeEntidad;
        vm.guardarUsuarioPerfil = guardarUsuarioPerfil;

        /// Comienzo
        vm.init = function () {
            //vm.listadoTipoEntidades = autorizacionServicios.obtenerTiposEntidad();
            vm.obtenerListadoEntidadesXUsuarioAutenticado();
        }

        //function onChangeTipoEntidad() {
        //    servicioUsuarios.obtenerEntidades(vm.tipoEntidad)
        //        .then(function (response) {
        //            let listaEntidades = [];

        //            response.data.forEach(item => {
        //                listaEntidades.push({ id: item.Id, nombre: item.Nombre});
        //            });

        //            listaEntidades.sort(function (a, b) {
        //                if (a.nombre > b.nombre) {
        //                    return 1;
        //                }
        //                if (a.nombre < b.nombre) {
        //                    return -1;
        //                }
        //                // a must be equal to b
        //                return 0;
        //            });

        //            vm.listaEntidades = listaEntidades.sort();

        //        }, function (error) {
        //            console.log("error", error);
        //        });
        //}


        function obtenerListadoEntidadesXUsuarioAutenticado() {
            servicioEntidades.obtenerListadoEntidadesXUsuarioAutenticado()
                .then(function (response) {
                    vm.listadoEntidades = response.data;
                }, function (error) {
                    console.log("error: obtenerListadoEntidadesXUsuarioAutenticado", error);
                });
        }

        function onChangeEntidad() {

            let entidad = vm.obtenerEntidad(vm.idEntidad);
            servicioEntidades.obtenerListadoPerfilesXEntidad(entidad.PadreId)
                .then(function (response) {
                    console.log(response.data)
                    let lista = [];
                    response.data.forEach(item => {
                        lista.push({ id: item.IdPerfil, nombre: item.NombrePerfil });
                    });

                    lista.sort(function (a, b) {
                        if (a.nombre > b.nombre) {
                            return 1;
                        }
                        if (a.nombre < b.nombre) {
                            return -1;
                        }
                        // a must be equal to b
                        return 0;
                    });

                    vm.listaPerfiles = lista;

            }, function (error) {
                console.log("error", error);
            });
        }

        vm.obtenerEntidad = function (id) {
            return vm.listadoEntidades.filter(x => x.Id == id)[0];
        }

        function guardarUsuarioPerfil() {
            //if (!vm.tipoEntidad) {
            //    utilidades.mensajeError('Seleccionar un tipo entidad.', false);
            //    return false;
            //}

            if (!vm.idEntidad) {
                utilidades.mensajeError('Seleccionar una entidad.', false);
                return false;
            }

            if (!vm.idPerfil) {
                utilidades.mensajeError('Seleccionar un perfil.', false);
                return false;
            }

            var dto = {
                idUsuario: vm.idUsuario,
                idPerfil: vm.idPerfil,
                idEntidad: vm.idEntidad,
                UsuarioDNP: vm.idUsuarioDnp
            };

            servicioUsuarios.crearUsuarioPerfil(dto)
                .then(function (response) {
                    if (response.data || response.data.Exitoso) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.obtenerDatosPerfilUsuario();
                        vm.cerrar();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje, false);
                    }
                });
        }

    }

    angular.module('backbone.usuarios').controller('modalAccionUsuarioPerfilController', modalAccionUsuarioPerfilController);
})();