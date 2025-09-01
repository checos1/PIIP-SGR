/// <reference path="../../../../model/AlertaMonitoreoGeneradaModel.js" />

(function () {
    'use strict';

    angular.module('backbone')
        .controller('alertasGeneradasController', alertasGeneradasController);

    alertasGeneradasController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioConsolaAlertas',
        'AlertaMonitoreoGeneradaModel',
        'ClassificacionAlertaConstante',
        'peticion',
        'proyectoId'
    ];

    function alertasGeneradasController(
        $scope,
        $uibModalInstance,
        servicioConsolaAlertas,
        AlertaMonitoreoGeneradaModel,
        ClassificacionAlertaConstante,
        peticion,
        proyectoId
    ) {
        const vm = this;

        //#region Variables

        const listaAlertas = [];
        const classificaciones = ClassificacionAlertaConstante;
        vm.listaAlertas = listaAlertas;
        vm.cargandoAlertas = false;

        //#endregion

        //#region Metodos

        async function _listarAlertas() {
            vm.cargandoAlertas = true;

            return await servicioConsolaAlertas.listarAlertasPorProyectoId(peticion, proyectoId)
                .then(response => {
                    vm.listaAlertas = (response.data || []).map(x => new AlertaMonitoreoGeneradaModel(x));
                    vm.cargandoAlertas = false
                })
                .catch(() => {
                    vm.cargandoAlertas = false;
                    toastr.error("Ocurrio un error al cargar alertas.")
                });
        }

        function cerrar() {
            $uibModalInstance.dismiss('cerrar');
        }

        function obtnerClassePorClassificacion(classificacionId) {
            const tipo = (classificaciones.find(x => x.valor == classificacionId) || {} ).tipo;
            if (tipo)
                return `alert-${tipo}`;
        }

        async function init() {
            _listarAlertas();
        }

        vm.init = init;
        vm.cerrar = cerrar;
        vm.obtnerClassePorClassificacion = obtnerClassePorClassificacion;

        //#endregion
    }
})();