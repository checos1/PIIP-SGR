(function () {
    'use strict';

    controladorAgregarPreguntas.$inject = ['$scope', '$uibModalInstance', '$http', 'items', '$localStorage', 'constantesTipoFiltro', 'CacheServicios'];

    function controladorAgregarPreguntas($scope, $uibModalInstance, $http, items, $localStorage, constantesTipoFiltro, CacheServicios) {
        var vm = this;
        vm.selecionados = [];
        vm.selecionadosOriginal = [];
        vm.agregarItem = items.agregarItem;
        vm.items = items;
        vm.datoAGuardar = [];
        vm.cancelar = cancelar;
        vm.adicionarDato = adicionarDato;
        vm.gravaDato = gravaDato;
        vm.modelo = { items: vm.items.campos }
        vm.listaRCache = null;
        vm.esEdicion = false;
        vm.getItems = getItems;
        vm.getClasificacion = getClasificacion;
        vm.eliminarPregunta = eliminarPregunta;

        vm.datos = []
        $scope.data = { Acuerdo: 0, Sector: 0, Clasificacion: 0 }



        this.$onInit = function () {
            if (vm.items) {
                vm.datos = vm.items.data
                vm.selecionados = vm.items.selecionados
                vm.selecionadosOriginal = vm.items.selecionados
            }
        }

        vm.log = function (item) {
            console.log(item)
        }

        function getItems() {
            try {
                return (vm.datos || []).find(p => p.Id == JSON.parse($scope.data.Acuerdo).Id).Items;
            } catch (ex) {
                return null;
            }
        }

        function getClasificacion() {

            try {
                return (getItems() || []).find(p => p.Id == JSON.parse($scope.data.Sector).Id).Items;
            } catch (ex) {
                return null;
            }
        }

        function getRemoverPreguntas() {
            var result = []
            for (var i = 0; i < vm.selecionadosOriginal.length; i++) {
                var item = vm.selecionadosOriginal[i];
                if (!vm.selecionados.find(p => p.id === item.id)) {
                    for (var j = 0; j < item.Clasificacion.Items.length; j++) {
                        result.push(item.Clasificacion.Items[j])
                    }
                }
            }
            return result;
        }
        function getPreguntas() {

            try {
                var preguntas = []
                for (var i = 0; i < vm.selecionados.length; i++) {
                    var item = vm.selecionados[i];
                    for (var j = 0; j < item.Clasificacion.Items.length; j++) {
                        preguntas.push(item.Clasificacion.Items[j])
                    }
                }
                return preguntas;
            } catch (ex) {
                console.log(ex)
                return null;
            }
        }

        function eliminarPregunta(id) {
            vm.selecionados = vm.selecionados.filter(p=> p.id !== id)
        }

        function adicionarDato() {
            var acuerdo = JSON.parse($scope.data.Acuerdo)
            var sector = JSON.parse($scope.data.Sector)
            var clasificacion = JSON.parse($scope.data.Clasificacion)
            var id = acuerdo.Id + '-' + sector.Id + '-' + clasificacion.Id;
            var item = vm.selecionados.find(p => p.id === id);
            if (!item) {
                vm.selecionados.push({
                    id: id,
                    Acuerdo: acuerdo,
                    Sector: sector,
                    Clasificacion: clasificacion,
                })
                $scope.data.Acuerdo= null
                $scope.data.Sector = null
                $scope.data.Clasificacion = null
            }
        }

        function gravaDato() {
            $uibModalInstance.close({
                nuevosItem: vm.datoAGuardar, datos: vm.items.datos
                , preguntas: getPreguntas()
                , removerPreguntas: getRemoverPreguntas()
                ,selecionados: vm.selecionados
            });
        };

        function cancelar() {
            $uibModalInstance.dismiss('cancel');
        };

    }

    angular.module('backbone').controller('controladorAgregarPreguntas', controladorAgregarPreguntas);
})();