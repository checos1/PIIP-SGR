(function (usuarioDNP, usuarioNombre, esUsuarioB2C) {
    informacionesUsuarioController.$inject = ["$scope", "$uibModal"];

    function informacionesUsuarioController($scope, $uibModal) {
        const vm = this;

        vm.nombreUsuario = usuarioNombre;
        vm.usuarioDNP = usuarioDNP;
        vm.esUsuarioB2C = esUsuarioB2C;
        vm.esUsuarioExterno = false;

        vm.init = function () {
            vm.nombreUsuario = vm.formatearNombreUsuario(usuarioNombre);
            vm.esUsuarioExterno = vm.validarTipoUsuarioExterno(usuarioDNP);
        }

        vm.formatearNombreUsuario = function (nombre) {
            let arrayNombre = nombre.split(':');
            let primerNombre = '';
            let primerApellido = '';

            if (arrayNombre.length > 0) {
                primerNombre = arrayNombre[0].split(' ')[0];
            }

            if (arrayNombre.length > 1) {
                primerApellido = arrayNombre[1].split(' ')[0];
            }

            return `${primerNombre} ${primerApellido}`;
        };

        vm.validarTipoUsuarioExterno = function (usuario) {
            let esUsuarioExterno = !usuario.includes('@dnp.gov.co')
            return esUsuarioExterno;
        };

        vm.abrirModalCambiarContrasena = function () {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl:
                    "src/app/usuarios/modales/cambiarContrasena/cambiarContrasenaTemplate.html",
                controller: "cambiarContrasenaController",
                controllerAs: "vm",
                size: "md",
                openedClass: "modal-cambiar-contrasena",
                resolve: {
                    params: {
                        usuarioDNP: vm.usuarioDNP,
                        esUsuarioExterno: vm.esUsuarioExterno,
                        nombreUsuario: vm.nombreUsuario
                    },
                },
            });
        };
    }

    angular.module("backbone").component("informacionesUsuario", {
        templateUrl:
            "src/app/usuarios/componentes/informacionesUsuario/informacionesUsuario.html",
        controller: informacionesUsuarioController,
        controllerAs: "vm",
    });
})(usuarioDNP, usuarioNombre, esUsuarioB2C);
