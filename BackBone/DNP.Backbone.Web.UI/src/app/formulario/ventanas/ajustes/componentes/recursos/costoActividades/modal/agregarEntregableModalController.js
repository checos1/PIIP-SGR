(function () {
    'use strict';

    angular.module('backbone')
        .controller('agregarEntregableModalController', agregarEntregableModalController);

    agregarEntregableModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'utilidades',
        '$filter',
        'Producto'
    ];

    function agregarEntregableModalController(
        $scope,
        $uibModalInstance,
        utilidades,
        $filter,
        Producto
    ) {
        const vm = this;

        //#region Variables
        vm.options = [];
        vm.Producto = Producto;
        vm.agregarOtro = false;
        vm.textoEntregable = "";
        vm.mostrarError = false;
        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;
        console.log(Producto);
        //#endregion

        //#region Metodos

        function guardar() {
            vm.mostrarError = false;
            vm.mostrarErrorLongitud = false;

            if (vm.agregarOtro && !vm.textoEntregable) {
                vm.mostrarError = true;

                return;
            }

            if (vm.agregarOtro && (vm.textoEntregable.length < 20 || vm.textoEntregable.length > 200)) {
                vm.mostrarErrorLongitud = true;

                return;
            }

            if (vm.agregarOtro) {
                let entregableAgreagar = new Object();
                entregableAgreagar.nombre = vm.textoEntregable;
                entregableAgreagar.etapa = vm.Producto.Etapa;
                entregableAgreagar.productoId = vm.Producto.ProductoId;
                entregableAgreagar.deliverable = vm.Producto.AplicaEDT;
                entregableAgreagar.deliverableCatalogId = null;

                vm.options.push(entregableAgreagar);
            }

            $uibModalInstance.close(vm.options);
        }

        function cerrar() {
            vm.options = [];
            $uibModalInstance.close();
        }

        function toggleGuardar() {
            if ((vm.options && vm.options.length > 0) || vm.agregarOtro) {
                vm.btnGuardarDisable = false;
            }
            else {
                vm.btnGuardarDisable = true;
            }
        };

        function toggleCurrency(entregable) {
            if (entregable.isChecked) {

                let entregableAgreagar = new Object();
                entregableAgreagar.nombre = entregable.EntregableNombre;
                entregableAgreagar.etapa = vm.Producto.Etapa;
                entregableAgreagar.productoId = vm.Producto.ProductoId;
                entregableAgreagar.deliverable = vm.Producto.AplicaEDT;
                entregableAgreagar.deliverableCatalogId = entregable.EntregableId;

                vm.options.push(entregableAgreagar);

            } else {
                var toDel = vm.options.map(function (e) { return e.deliverableCatalogId; }).indexOf(entregable.EntregableId);
                vm.options.splice(toDel, 1);
            }

            toggleGuardar();
        };

        function seleccionarNuevo(nuevo) {
            vm.agregarOtro = nuevo.isChecked;

            toggleGuardar();

        };

        async function init() {
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.toggleCurrency = toggleCurrency;
        vm.seleccionarNuevo = seleccionarNuevo;
        //#endregion
    }
})();