(function () {
    'use strict';

    consultaConpesFormulario.$inject = [
        '$scope',
        'conpesServicio',
        'utilidades',
    ];

    function consultaConpesFormulario(
        $scope,
        conpesServicio,
        utilidades
    ) {
        var vm = this;

        vm.lang = 'es';
        vm.queryConpes = '';
        vm.nombreComponente = 'consultaGeneralConpes';
        vm.conpesList = [];
        vm.conpesSelected = [];
        vm.compesListFiltered = [];
        vm.conpespreseleccionados = [];

        vm.paginationModel = {
            callCompleted: false,
            hasError: false,
            totalItems: 0,
            paginaActual: 1,
            itemsPorPagina: 5
        };

        vm.init = function () {
            vm.setEvents();
        }

        vm.setEvents = function () {
            $scope.$watch('vm.conpespreseleccionados', function () {
                vm.conpesSelected = [];
                vm.resetPagination();

                vm.conpesList = vm.filterConpesList(vm.conpesList);
                vm.paginationModel.totalItems = vm.conpesList.length;
                vm.updateConpesPaginated();
            });

            $scope.$watch('vm.clearconpesindex', function () {
                if (vm.clearconpesindex && vm.clearconpesindex > 0) {
                    vm.clearConpes();
                }
            });
        }

        vm.getConpes = function () {
            if (vm.queryConpes === '') {
                return;
            }
            vm.conpesList = [];
            vm.resetPagination();

            conpesServicio.BuscarConpes(vm.queryConpes)
                .then(function (response) {
                    vm.paginationModel.callCompleted = true;

                    if (response.data.estado && response.data.mensaje == 'OK') {
                        vm.conpesList = vm.filterConpesList(response.data.documentosCONPES);
                        vm.paginationModel.totalItems = vm.conpesList.length;

                        vm.updateConpesPaginated();
                        return;
                    }

                    vm.paginationModel.hasError = true;
                }, function (error) {
                    vm.pagina.callCompleted = true;
                    vm.paginationModel.hasError = true;
                    utilidades.mensajeError('No fue posible consultar el listado de conpes');
                });
        }

        vm.filterConpesList = function (conpesList) {
            if (!vm.conpespreseleccionados) {
                return [...conpesList];
            }

            return conpesList.filter(
                item => !vm.conpespreseleccionados.some(w => w.numeroCONPES === item.numeroCONPES)
            );
        }

        vm.updateConpesPaginated = function () {
            vm.compesListFiltered = vm.conpesList.slice(
                (vm.paginationModel.paginaActual - 1) * vm.paginationModel.itemsPorPagina,
                (vm.paginationModel.paginaActual) * vm.paginationModel.itemsPorPagina
            ).map(item => {
                item['selected'] = vm.conpesSelected.some(w => w.numeroCONPES == item.numeroCONPES);
                return item;
            });
        }

        vm.selectConpes = function (idCheckbox) {
            const selectorConpes = document.getElementById('compes-check-' + idCheckbox);
            if (selectorConpes === undefined || selectorConpes === null) {
                return;
            }

            if (selectorConpes.checked) {
                const itemSelected = vm.compesListFiltered.find(w => w.numeroCONPES == idCheckbox);
                if (itemSelected === undefined) {
                    return;
                }
                vm.conpesSelected.push(itemSelected);
            } else {
                vm.conpesSelected = vm.conpesSelected.filter(w => w.numeroCONPES != idCheckbox);
            }
        }

        //Mapea el listado de compes preseleccionados
        vm.setConpesExistentes = function (compesPreseleccionados) {
            vm.conpespreseleccionados = compesPreseleccionados;
        }

        vm.onCompesPageChange = function () {
            vm.updateConpesPaginated();
        }

        vm.saveConpes = function () {
            vm.guardarcompesevent({ compesList: vm.conpesSelected });
        }

        vm.clearConpes = function () {
            vm.conpesSelected = [];
            vm.queryConpes = [];
            vm.conpesList = [];
            vm.compesListFiltered = [];
            vm.resetPagination();
        }

        vm.removeConpes = function (conpesSeleccionado) {
            vm.removerconpesevent({ conpes: conpesSeleccionado });
        }

        vm.resetPagination = function () {
            vm.paginationModel.totalItems = 0;
            vm.paginationModel.paginaActual = 1;
            vm.paginationModel.callCompleted = false;
            vm.paginationModel.hasError = false;
        }
    }

    angular.module('backbone').component('consultaConpesFormulario', {        
        templateUrl: "src/app/formulario/ventanas/comun/consultaConpes/consultaConpesFormulario.html",
        controller: consultaConpesFormulario,
        controllerAs: "vm",
        bindings: {
            conpespreseleccionados: '<',
            guardarcompesevent: '&',
            removerconpesevent: '&',
            clearconpesindex: '<'
        }
    });
})();