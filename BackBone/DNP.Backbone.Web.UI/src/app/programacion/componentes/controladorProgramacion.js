var programacionCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('controladorProgramacion', controladorProgramacion);
    controladorProgramacion.$inject = [
        '$scope',
        'servicioProgramacion',
        '$uibModal',
        '$log',
        '$q',
        '$sessionStorage',
        '$localStorage',
        '$timeout',
        '$location',
        '$filter',
        'utilidades'
    ];

    function controladorProgramacion(
        $scope,
        servicioProgramacion,
        $uibModal,
        $log,
        $q,
        $sessionStorage,
        $localStorage,
        $timeout,
        $location,
        $filter,        
        utilidades) {
        var vm = this;
        programacionCtrl = vm;       
        //variables
        vm.lang = "es";        
        vm.VerCuota = 0;       
        vm.VerCalendario = 0;       
        vm.VerGenerarPresupuestal = 0;       
        vm.VerCredito = 0;       
        vm.TabActivo = 1;
        this.$onInit = function () {
          
        };         
        vm.CargarCuota = function () {
            vm.VerCuota = 1;
            vm.VerCalendario = 0;
            vm.VerGenerarPresupuestal = 0;
            vm.VerCredito = 0;  
        }
        vm.CargarCalendario = function () {
            vm.VerCuota = 0;
            vm.VerCalendario = 1;
            vm.VerGenerarPresupuestal = 0;
            vm.VerCredito = 0;  
        }
        vm.CargarGenerarPresupuestal = function () {
            vm.VerCuota = 0;
            vm.VerCalendario = 0;
            vm.VerGenerarPresupuestal = 1;
            vm.VerCredito = 0;
        }
        vm.CargarCredito = function () {
            vm.VerCuota = 0;
            vm.VerCalendario = 0;
            vm.VerGenerarPresupuestal = 0;
            vm.VerCredito = 1;  
        }

        vm.ActivarTab = function (tab) {
            vm.TabActivo = tab;
        }
    }
    angular.module('backbone')
        .component('programacion', {
            templateUrl: 'src/app/programacion/componentes/programacion.template.html',
            controller: 'controladorProgramacion',
            controllerAs: 'vm'
        });
})();