
(function () {
    'use strict';
    angular.module('backbone').controller('reporteEstructuraActividadesController', reporteEstructuraActividadesController);
    reporteEstructuraActividadesController.$inject = [
        '$scope',
        'reporteEstructuraActividadesServicio',
        '$routeParams'
    ];
    function reporteEstructuraActividadesController(
        $scope,
        reporteEstructuraActividadesServicio,
        $routeParams
    ) {
        var vm = this;
        vm.embedFiltro = {
            $schema: "http://powerbi.com/product/schema#basic",
            target: {
                table: "Proyectos",
                column: "BPIN",
            },
            operator: "In",
            values: [],
            displaySettings: {
                isLockedInViewMode: true,
                isHiddenInViewMode: false,
                displayName: "Filtro por BPIN",
            },
        };
        //Inicio
        vm.init = function () {
            
            var filtro = {
                ReportId: "f82ce80a-ea65-43da-8ca4-86a9f03bab1d"
            };
            reporteEstructuraActividadesServicio
                .obtenerReportesPowerBI(filtro)
                .then(
                    function (retorno) {
                        var Bpin = $routeParams['BPIN'];
                        vm.embedFiltro.values = [Bpin];
                        vm.embedConfig = retorno.data;
                    }
                );
        };
    }

    angular.module('backbone')
        .component('reporteEstructuraActividades', {
            templateUrl: "/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaEstructuraActividades/reporteEstructuraActividades.html",
            controller: 'reporteEstructuraActividadesController',
            controllerAs: 'vm'
        });
})();