
(function () {
    'use strict';
    angular.module('backbone').controller('reporteGanttController', reporteGanttController);
    reporteGanttController.$inject = [
        '$scope',
        'reporteGanttServicio',
        '$routeParams'
    ];
    function reporteGanttController(
        $scope,
        reporteGanttServicio,
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
                ReportId: "444c4b50-09af-44e7-b3bf-1dfa9d3ab17f"
            };
            reporteGanttServicio
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
        .component('reporteGantt', {
            templateUrl: "/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaGantt/reporteGantt.html",
            controller: 'reporteGanttController',
            controllerAs: 'vm'
        });
})();