
(function () {
    'use strict';

    selecionarProyectosCreditosController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        '$uibModalInstance',
        'parametros',
        'servicioCreditos',
        '$timeout',
        'uiGridSelectionConstants',
        'uiGridPinningConstants',
        'utilidades'
    ];

    function selecionarProyectosCreditosController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        $uibModalInstance,
        parametros,
        servicioCreditos,
        $timeout,
        uiGridSelectionConstants,
        uiGridPinningConstants,
        utilidades
    ) {
        var vm = this;
        vm.listaProyectoContraCredito = [];
        vm.user = {};
        vm.parametros = parametros;
        vm.paso1visible = true;
        vm.paso2visible = false;
        //vm.paso3visible = false;
        vm.linhaContraCredito = [];
        vm.listaEntidadesFiltro = [];
        vm.listaProyectosOrigem = [];
        vm.listaProyectosCreditoOrigem = [];
        vm.entidadFiltroContra = 0;
        vm.bpinFiltroContra = null;
        vm.proyectosFiltroContra = null;
        vm.bpinFiltroCredito = null;
        vm.proyectosFiltroCredito = null;
        //#region Métodos

        vm.guardarProyectos = guardarProyectos;
        vm.cerrar = $uibModalInstance.dismiss;

        vm.paso1 = paso1;
        vm.paso2 = paso2;
        vm.filtrarContraCredito = filtrarContraCredito;
        vm.limparfiltroContraCredito = limparfiltroContraCredito;
        vm.filtrarCredito = filtrarCredito;
        vm.limparfiltroCredito = limparfiltroCredito;

        //#endregion

        function guardarProyectos() {

            let linhasCredito = $scope.gridApi.selection.getSelectedRows();
            if (!(linhasCredito.length > 0)) {
                toastr.warning("Seleccione un registro para continuar");
                return;
            }
            let proyectos = [];

            let contra = vm.linhaContraCredito[0];
            let c = {
                InstanciaId: vm.parametros.idInstancia,
                ProyectoId: contra.IdProyecto,
                ProyectoNombre: contra.NombreProyecto,
                FlujoId: vm.parametros.idFlujo,
                EntidadId: contra.IdEntidad,
                ProyectoTipo: 'Contracredito',
                ObjetoNegocioId: contra.BPIN,
                Usuario: usuarioDNP
            }
            proyectos.push(c);

            linhasCredito.forEach(x => {
                let p = {
                    InstanciaId: vm.parametros.idInstancia,
                    ProyectoId: x.IdProyecto,
                    ProyectoNombre: x.NombreProyecto,
                    FlujoId: vm.parametros.idFlujo,
                    EntidadId: x.IdEntidad,
                    ProyectoTipo: 'Credito',
                    ObjetoNegocioId: x.BPIN,
                    Usuario: usuarioDNP
                }
                proyectos.push(p);
            });


            var prm = {
                idFlujo: vm.parametros.idFlujo,
                tipoEntidad: vm.parametros.tipoEntidad,
                InstanciaId: vm.parametros.idInstancia,
                proyectos: proyectos
            };


            servicioCreditos.guardarProyectos(prm)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                    } else {
                        utilidades.mensajeError(response.data.Mensaje, false);
                    }
                });
        }


        function paso1() {
            angular.element('#step1li').removeClass('disabled').addClass('active');
            angular.element('#step2li').removeClass('active').addClass('disabled');
            vm.paso1visible = true;
            vm.paso2visible = false;
        }

        function paso2() {
            //console.log($scope.gridApi.selection.getSelectedRows());
            vm.linhaContraCredito = $scope.gridApi.selection.getSelectedRows();

            if (!(vm.linhaContraCredito.length > 0)) {
                toastr.warning("Seleccione un registro para continuar");
                return;
            }

            angular.element('#step2li').removeClass('disabled').addClass('active');
            angular.element('#step1li').removeClass('active').addClass('disabled');
            vm.paso1visible = false;
            vm.paso2visible = true;

            var prm = {
                idFlujo: vm.parametros.idFlujo,
                tipoEntidad: vm.parametros.tipoEntidad,
                idEntidad: vm.linhaContraCredito[0].IdEntidad
            }

            servicioCreditos.obtenerCreditos(prm)
                .then(function (response) {
                    //console.log(response.data);
                    if (response.data != null && response.data.length > 0) {
                        vm.gridOptionsCreditos.data = response.data;
                        vm.listaProyectosCreditoOrigem = response.data;
                    }
                });
        }

        function filtrarContraCredito() {

            vm.gridOptions.data = vm.listaProyectosOrigem;

            if (vm.entidadFiltroContra && vm.entidadFiltroContra != 0)
                vm.gridOptions.data = vm.gridOptions.data.filter(x => x.NombreEntidad.indexOf(vm.entidadFiltroContra) != -1);
            if (vm.bpinFiltroContra)
                vm.gridOptions.data = vm.gridOptions.data.filter(x => x.BPIN.includes(vm.bpinFiltroContra));
            if (vm.proyectosFiltroContra)
                vm.gridOptions.data = vm.gridOptions.data.filter(x => x.NombreProyecto.includes(vm.proyectosFiltroContra));
        }

        function filtrarCredito() {

            vm.gridOptionsCreditos.data = vm.listaProyectosCreditoOrigem;

            if (vm.bpinFiltroCredito)
                vm.gridOptionsCreditos.data = vm.gridOptionsCreditos.data.filter(x => x.BPIN.includes(vm.bpinFiltroCredito));
            if (vm.proyectosFiltroCredito)
                vm.gridOptionsCreditos.data = vm.gridOptionsCreditos.data.filter(x => x.NombreProyecto.includes(vm.proyectosFiltroCredito));
        }

        function limparfiltroContraCredito() {
            vm.entidadFiltroContra = 0;
            vm.bpinFiltroContra = null;
            vm.proyectosFiltroContra = null;
            vm.gridOptions.data = vm.listaProyectosOrigem;
        }

        function limparfiltroCredito() {
            vm.bpinFiltroContra = null;
            vm.proyectosFiltroContra = null;
            vm.gridOptionsCreditos.data = vm.listaProyectosCreditoOrigem;
        }

        const columnasDef = [{
            field: 'Sector',
            displayName: "SECTOR",
            width: "23%",
            headerCellClass: 'ui-grid-cell-header',
        },
        {
            field: 'NombreEntidad',
            displayName: "ENTIDAD",
            width: "28%",
            headerCellClass: 'ui-grid-cell-header',
        },
        {
            field: 'BPIN',
            displayName: "BPIN",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            width: "15%"
        },
        {
            field: 'NombreProyecto',
            displayName: "PROYECTO",
            width: "300%",
            headerCellClass: 'ui-grid-cell-header ',
        }
        ];

        vm.gridOptions = {
            enableRowSelection: false,
            enableSelectAll: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 31,
            multiSelect: false,
            //showGridFooter: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };

        vm.gridOptionsCreditos = {
            enableRowSelection: false,
            enableSelectAll: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 31,
            multiSelect: true,
            //showGridFooter: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };


        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
            ////$scope.gridApi.selection.setMultiSelect = false;
            //$scope.gridApi.grid.getColumn('selectionRowHeaderCol').colDef.allowCellFocus = false;
            // this.$scope.sessionsTableData = vm.gridOptions.data;

            //gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            //    this.toggleSelection(row);
            //}.bind(this));
            $timeout(function () {
                var col = gridApi.grid.getColumn(uiGridSelectionConstants.selectionRowHeaderColName);
                gridApi.pinning.pinColumn(col, uiGridPinningConstants.container.LEFT);
            }.bind(this));
        }


        //this.toggleSelection = function (row) {
        //    if (!row._helperData)
        //        row._helperData = {};
        //    row._helperData.checked = !!row.isSelected;
        //    if (row.isSelected) {
        //        this.addRowToCheckedList(row);
        //    } else {
        //        this.removeRowFromCheckedList(row);
        //    }
        //};


        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }


        /// Comienzo
        vm.$onInit = function () {
            var prm = {
                idFlujo: vm.parametros.idFlujo, tipoEntidad: vm.parametros.tipoEntidad
            }

            servicioCreditos.obtenerContraCreditos(prm)
                .then(function (response) {
                    //console.log(response.data);
                    if (response.data != null && response.data.length > 0) {
                        vm.gridOptions.data = response.data;
                        vm.listaProyectosOrigem = response.data;
                        vm.listaEntidadesFiltro = response.data.map(function (a) { return a.NombreEntidad; }).filter((v, i, a) => a.indexOf(v) === i).filter(onlyUnique);
                    }
                });
        }

    }

    angular.module('backbone').controller('selecionarProyectosCreditosController', selecionarProyectosCreditosController);
})();

