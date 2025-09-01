(function () {
    'use strict';
    angular.module('backbone').controller('seleccionDeProyectosController', seleccionDeProyectosController);


    seleccionDeProyectosController.$inject = [
        '$scope',
      //  '$uibModalInstance',        
        'servicioCreditos',
        '$timeout',
        'uiGridSelectionConstants',
        'uiGridPinningConstants',
        'utilidades',
        '$sessionStorage'
    ];

    function seleccionDeProyectosController(
        $scope,
      //  $uibModalInstance,        
        servicioCreditos,
        $timeout,
        uiGridSelectionConstants,
        uiGridPinningConstants,
        utilidades,
        $sessionStorage
    ) {
        var vm = this;
        
        vm.listaProyectoContraCredito = [];
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: $sessionStorage.idInstanciaIframe,
            IdEntidad: $sessionStorage.idEntidad
        };
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
        vm.init = init;
        //#region Métodos

        vm.guardarProyectos = guardarProyectos;
        //vm.cerrar = $uibModalInstance.dismiss;

        vm.paso1 = paso1;
        vm.paso2 = paso2;
        vm.filtrarContraCredito = filtrarContraCredito;
        vm.limparfiltroContraCredito = limparfiltroContraCredito;
        vm.filtrarCredito = filtrarCredito;
        vm.limparfiltroCredito = limparfiltroCredito;

        //#endregion

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

        vm.gridOptions = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 31,
            multiSelect: false,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi
        };

        vm.gridOptionsCreditos = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 31,
            multiSelect: true,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function paso1() {
            angular.element('#step1li').removeClass('disabled').addClass('active');
            angular.element('#step2li').removeClass('active').addClass('disabled');
            vm.paso1visible = true;
            vm.paso2visible = false;
        }

        function paso2() {
          //  console.log($scope.gridApi.selection.getSelectedRows());
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
                idEntidad: vm.linhaContraCredito[0].IdEntidad,
                BPIN: vm.linhaContraCredito[0].BPIN,
                idInstancia: vm.parametros.idInstancia
            }

            servicioCreditos.obtenerCreditos(prm)
                .then(function (response) {
                 //   console.log(response.data);
                    if (response.data != null && response.data.length > 0) {
                        vm.gridOptionsCreditos.data = response.data;
                        vm.listaProyectosCreditoOrigem = response.data;
                    }
                });
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
                    if (response.data && response.data.Exito) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        //swal('', "Operación realizada con éxito!", 'success')
                        //$('#modal-componente').modal('toggle');
                        //utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        //vm.cerrar();
                    } else {
                        swal('', "Error al realizar la operación", 'error')
                    }
                });
        }

        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        /// Comienzo
        function init() {
            var prm = {
                idFlujo: vm.parametros.idFlujo, tipoEntidad: vm.parametros.tipoEntidad, idEntidad: vm.parametros.IdEntidad, idInstancia: vm.parametros.idInstancia
            }

            servicioCreditos.obtenerContraCreditos(prm)
                .then(function (response) {
                    //console.log(response.data);
                    if (response.data != null && response.data.length > 0) {
                        if (!(response.data[0].GruposPermitidos)) {
                            toastr.warning("El tramite no permite adicionar mas de un grupo");
                            return;
                        }
                        vm.gridOptions.data = response.data;
                        vm.listaProyectosOrigem = response.data;
                        vm.listaEntidadesFiltro = response.data.map(function (a) { return a.NombreEntidad; }).filter((v, i, a) => a.indexOf(v) === i).filter(onlyUnique);
                    }
                });
        }
    }

    angular.module('backbone')
        .component('seleccionDeProyectos', {
            templateUrl: "/src/app/formulario/modales/seleccionDeProyectos.html",
            controller: 'seleccionDeProyectosController',
            controllerAs: 'vm',
            //bindings: {
            //    accionCrearEditarEntidad: '&',
            //    accionEliminarEntidad: '&',
            //    accionCrearSubEntidad: '&',
            //    accionEditarSubEntidad: '&',
            //    accionFlujosViabilidad: '&',
            //}
        });
})();
