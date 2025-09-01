(function () {
    'use strict';

    modalRolesXPerfilController.$inject = [
        'obj',
        'servicioEntidades',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades',
    ];

    function modalRolesXPerfilController(
        obj,
        servicioEntidades,
        $uibModalInstance,
        servicioUsuarios,
        utilidades,
    ) {
        var vm = this;

        /// models
        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardar = guardarUsuario;
        vm.IdUsuario = obj.Id;
        vm.nombre = '';
        vm.apelido = '';
        vm.idEntidad = obj.idEntidad;
        vm.nombreEntidad = obj.nombreEntidad;
        vm.idUsuarioDNP = obj.IdUsuarioDNP;
        vm.Nombres = obj.Nombre;
        vm.Correo = obj.Correo;
        vm.ListaPerfilesUsuario = [];
        vm.listaPerfilesUsuarioBackbone = [];
        vm.Activo = obj.Activo;
        vm.EstadoUsuario = '';
        vm.TipoUsuario = "";
        
        /// Comienzo
        vm.init = function () {
            if (obj) {
                var nombreSplit = obj.Nombre.split(' ');
                var len = nombreSplit.length;

                vm.nombre = nombreSplit.slice(0, -(len - 1)).join(' ');
                if (len > 1) {
                    vm.apelido = nombreSplit.slice(1).join(' ');
                } else {
                    vm.apelido = '';
                }
                if (vm.Activo == true) {
                    vm.EstadoUsuario = "Activo";
                }
                else {
                    vm.EstadoUsuario = "Inactivo";
                }
                if (vm.Correo != null) {
                    let esCorreoInstitucional = vm.Correo.toUpperCase().includes("DNP.GOV.CO");

                    if (esCorreoInstitucional) {
                        vm.TipoUsuario = "Usuario DNP"
                    }
                    else {
                        vm.TipoUsuario = "Usuario Externo"
                    }
                }


            }
            if (vm.idEntidad) {
                vm.obtenerListadoPerfilesXEntidad(vm.idEntidad);
                vm.obtenerListadoPerfilesXEntidadYUsuario(vm.idEntidad, vm.IdUsuario);
            }
        }
        vm.obtenerListadoPerfilesXEntidad = function (idEntidad) {
            var promise = servicioEntidades.obtenerListadoPerfilesXEntidad(idEntidad);
            promise.then(function (response) {
                vm.listaPerfilesBackbone = response.data;

            }, function (error) {
                console.log("error", error);
            });
        }
        function guardarUsuario() {
            var dto = {
                Nombre: vm.nombre + ' ' + vm.apelido,
                IdUsuario: vm.IdUsuario
            };

            servicioUsuarios.guardarUsuario(dto)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }

        vm.obtenerListadoPerfilesXEntidadYUsuario = function (idEntidad, idUsuario) {
            /*  var promise = actions.obtenerListadoPerfilesXUsuario(idEntidad, idUsuario);*/
            servicioUsuarios.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario)
                .then(function (response) {
                    var prueba = response.data;
                    vm.listaPerfilesUsuarioBackbone = prueba;
                    vm.ListaPerfilesUsuario = [];
                    var objetoperfil = { };
                    vm.listaPerfilesUsuarioBackbone.forEach(function (item, i) {
                       
                        var peticionObtenerRoles = {
                            usuario: usuarioDNP,
                            idPerfil: item.IdPerfil
                        };
                        ObtenerRolesporPerfil(peticionObtenerRoles, item.IdPerfil, item.NombrePerfil);
                        


                    });

                }, function (error) {
                    console.log("error", error);
                });
        }

        
       

        function ObtenerRolesporPerfil(peticionObtenerRoles, id, nombre) {
            var objetoperfil = {};
            servicioUsuarios.obtenerRolesDePerfil(peticionObtenerRoles)
                .then(function (response) {
                    if (response != undefined) {
                        objetoperfil.IdPerfil = id;
                        objetoperfil.NombrePerfil = nombre;
                        objetoperfil.RolesXperfil = {
                            data: response.data
                        }
                        vm.ListaPerfilesUsuario.push(objetoperfil);
                    }
                })
        }

    }

    angular.module('backbone.usuarios').controller('modalRolesXPerfilController', modalRolesXPerfilController);
})();