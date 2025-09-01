(function () {
    "use strict";

    angular.module("backbone").controller("seleccionarEntidadesController", seleccionarEntidadesController);

    seleccionarEntidadesController.$inject = ["$scope", "$uibModalInstance", "$filter", "appSettings", "entidades"];

    function seleccionarEntidadesController(
        $scope,
        $uibModalInstance,
        $filter,
        appSettings,
        entidades) {
        
        var vm = this;                

        /* Métodos */
        vm.continuar = continuar;
        vm.cancelar = cancelar;
        
        /* Variables */
        vm.instanciaModal = $uibModalInstance;
        vm.lang = 'es';
        vm.listaDeEntidades = entidades || [];
        vm.totalRegistros = vm.listaDeEntidades.length;
        vm.numeroPorPagina = appSettings.topePaginacionConsultaAplicaciones;
        vm.maximoPaginas = 10;
        var accionesTemplate = '<div class="text-center"> <button type="button" class="btn btn-default"  ng-click="grid.appScope.vm.continuar(row.entity)" tooltip-placement="bottom" uib-tooltip="Crear Instancia"><span class="icon-instance"></span></button></div>';

        vm.gridEntidades= {
            rowHeight: 36,
            enableRowSelection: false,
            enableRowHeaderSelection: false,
            rowSelection: false,
            noUnselect: false,
            multiSelect: false,
            enableVerticalScrollbar: 0,
            showGridFooter: true,
            gridFooterTemplate:
            '<div class="ui-grid-cell-contents" ng-show="grid.appScope.vm.filasFiltradas"><strong>Total registros: {{grid.appScope.vm.filasFiltradas}} / {{grid.appScope.vm.totalRegistros}}.<strong></div>',
            enableFiltering: true,
            totalItems: vm.listaDeEntidades.length,
            paginationPageSize: vm.numeroPorPagina,
            minRowsToShow: vm.listaDeEntidades.length < vm.numeroPorPagina ? vm.listaDeEntidades.length : vm.numeroPorPagina,
            enablePaginationControls: false,
            paginationCurrentPage: 1,
            columnDefs: [
                { name: $filter('language')('TextoEntidad'), field: 'NombreEntidad', enableHiding: false, width: "90%" },
                { name: $filter('language')('TextoAcciones'), displayName: 'Acciones', enableSorting: false, enableFiltering: false, width: "10%", cellTemplate: accionesTemplate }
            ],
            data: vm.listaDeEntidades,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                gridApi.core.on.rowsRendered($scope, function() {
                    var activas = $scope.gridApi.grid.getVisibleRows();
                    vm.filasFiltradas = activas.length;
                });                
            }
        };                

        function continuar(entidadSeleccionada) {
            vm.instanciaModal.close(entidadSeleccionada);
        }

        function cancelar() {            
            $uibModalInstance.dismiss(null);
        };
      
    }
})();