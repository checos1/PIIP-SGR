(function () {
    'use strict';

    angular.module('backbone.formulario').directive('adicionarWbs', adicionarWbs);

    adicionarController.$inject = ['utilidades', '$filter', '$scope'];

    function adicionarController(utilidades, $filter, $scope) {
        var vm = this;

        vm.cancelarAdicionar = cancelarAdicionar;
        vm.guardarAdicionar = guardarAdicionar;

        this.$onInit = function () {
            vm.modelo.mostrarAdicionar = false;
        }

        function guardarAdicionar() {
            utilidades.mensajeWarning($filter('language')('ConfirmarGuardar'),
                function () {
                    vm.datos[vm.modelo.propiedad].push(vm.nuevaJerarquia);
                    vm.datos[vm.modelo.propiedad].mostrarAdicionar = false;
                    vm.datos[vm.modelo.propiedad].mostrarAdicionarSinDatos = false;
                    vm.nuevaJerarquia = {};
                    $scope.$apply();
                    $scope.$emit("totalizar");
                });
        }

        function cancelarAdicionar() {
            utilidades.mensajeWarning($filter('language')('ConfirmaCancelar'),
                function () {
                    vm.datos[vm.modelo.propiedad].mostrarAdicionar = false;
                    if (!vm.datos[vm.modelo.propiedad].mostrarAdicionarSinDatos && !vm.datos[vm.modelo.propiedad].mostrarAdicionar && vm.datos[vm.modelo.propiedad].length === 0) {
                        vm.datos[vm.modelo.propiedad].mostrarAdicionarSinDatos = true;
                    }
                    if (!vm.datos[vm.modelo.propiedad].mostrarAdicionarSinDatos && !vm.datos[vm.modelo.propiedad].mostrarAdicionar && vm.datos[vm.modelo.propiedad].length >= 1) {
                        vm.datos[vm.modelo.propiedad].mostrarAdicionarSinDatos = false;

                    }
                    vm.nuevaJerarquia = {};
                    $scope.$apply();
                });
        }
    }

    function adicionarWbs() {
        return {
            restrict: 'E',
            scope: {
                modelo: '=',
                datos: '=',
                idPadre: '@'
            },
            templateUrl: '/src/app/formulario/componentes/WBS/directivas/adicionar/adicionar.template.html',
            controller: adicionarController,
            controllerAs: 'vm',
            bindToController: true
        };
    }
})();