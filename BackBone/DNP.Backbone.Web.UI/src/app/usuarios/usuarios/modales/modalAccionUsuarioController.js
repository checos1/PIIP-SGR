(function () {
    'use strict';

    modalAccionUsuarioController.$inject = [
        'obj',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades',
    ];

    function modalAccionUsuarioController(
        obj,
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
        vm.Activo = obj.Activo;
        vm.EstadoUsuario = '';
        vm.TipoUsuario = "";

        /// Comienzo
        vm.init = function () {
            if (obj) {
                var nombreSplit = obj.Nombre.split(':');
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
        }

        function guardarUsuario() {
            var dto = {
                Nombre: vm.nombre + ':' + vm.apelido,
                IdUsuario: vm.IdUsuario,
                IdEntidad: vm.idEntidad,
                Correo: vm.Correo
            };
            
            servicioUsuarios.guardarUsuarioTerritorio(dto)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }

    }

    angular.module('backbone.usuarios').controller('modalAccionUsuarioController', modalAccionUsuarioController);
})();