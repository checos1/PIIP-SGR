(function () {
    'use strict';

    angular.module('backbone.formulario').directive('wbs', wbs);

    wbsController.$inject = ['$timeout', '$scope', '$uibModal', 'wbsServicio', 'templatesServicio'];

    function wbsController($timeout, $scope, $uibModal, wbsServicio, templatesServicio) {
        var vm = this;
        vm.templatesServicio = templatesServicio;
        vm.filtrosWBS = null;
        vm.mostrarFiltros = false;
        

        this.$onInit = function () {
            vm.modelo.mostrarAdicionar = false;
        }

        $scope.$watch('vm.datos', function () {
            if (vm.datos) {
                if (vm.filtrosWBS === null) {
                    wbsServicio.inicializarWBS(vm.datos);
                    vm.filtrosWBS = wbsServicio.construirFiltros(vm.modelo.propiedades, vm.datos);
                    vm.filtrosWBS.filter(f => f.valorSeleccionado !== null || f.valorSeleccionado !== undefined).forEach(f => vm.filtrar(null, f));
                    wbsServicio.collapsarDatos(vm.datos);
                }
            }
        });

        vm.buscar = function () {
            vm.filtrosWBS.filter(f => f.valorSeleccionado !== null || f.valorSeleccionado !== undefined).forEach(f => vm.filtrar(f.valorSeleccionado, f));
        }

        vm.limpiarCamposFiltro = function () {
            vm.filtrosWBS.filter(f => f.valorSeleccionado !== null || f.valorSeleccionado !== undefined).forEach(f => vm.filtrar(null, f));
        }

        vm.filtrar = function (valorSeleccionado, filtro) {
            wbsServicio.ocultarDatosCompleto(vm.datos);
            wbsServicio.filtrarWBS($scope.vm.modelo.propiedades, vm.datos, vm.filtrosWBS);

            if (valorSeleccionado === null)//Hack para que no se quede mostrando 'Todos' los registros al seleccionar otro filtro
                filtro.valorSeleccionado = undefined;
        }

        vm.mostrarONoMostrarFiltros = function () {
            vm.mostrarFiltros = !vm.mostrarFiltros;
        }

        vm.verLog = function () {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/panelPrincial/modales/logs/logsModal.html',
                controller: 'logsModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => null
                },
            });
        }

    }

    function wbs() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelo: '=',
                datos: '=',
                schema: '=',
                opciones: '<'
            },
            templateUrl: '/src/app/formulario/componentes/WBS/wbs.template.html',
            controller: wbsController,
            controllerAs: 'vm',
            bindToController: true
        };
    }
})();