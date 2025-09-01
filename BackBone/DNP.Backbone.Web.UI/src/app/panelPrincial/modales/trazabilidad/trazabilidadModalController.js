(function () {
    'use strict';

    angular.module('backbone')
        .controller('trazabilidadModalController', trazabilidadModalController);

    trazabilidadModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$filter',
        'trazabilidadModalServicio',
        'IdInstancia',
        'IdNivel'
    ];

    function trazabilidadModalController(
        $scope,
        $uibModalInstance,
        $filter,
        trazabilidadModalServicio,
        IdInstancia,
        IdNivel
    ) {
        const vm = this;

        //#region Variables
        vm.columnas = ["Usuario", "Paso", "Accion", "Fecha y Hora"];
        vm.buscando = false;
        vm.sinResultados = false;
        vm.lang = "es";
        vm.mostrarMensajeProyectos = false;
        vm.Mensaje = "";

        vm.model = {
            logs: [{
                id: 1,
                flujoId: "",
                instanciaId: "",
                usuario: "Usuario general",
                fechaHora: $filter('date')(new Date(), 'dd/MM/yyyy HH:mm'),
                proceso: "Proceso",
                de: "Paso origen",
                operacion: "Operación",
                a: "Acción destino",
                rol: "Rol",
                entidad: "Nombre de entidad"
            }]
        };

        vm.columnDefPrincial = [{
            field: 'usuario',
            displayName: 'Usuario',
            enableHiding: false,
            width: '16%',
        }, {
            field: 'proceso',
            displayName: 'Proceso',
            enableHiding: false,
            width: '14%',
        }, {
            field: 'de',
            displayName: 'De',
            enableHiding: false,
            width: '14%',
        }, {
            field: 'a',
            displayName: 'A',
            enableHiding: false,
            width: '14%',
        }, {
            field: 'rol',
            displayName: 'Rol',
            enableHiding: false,
            width: '14%',
        }, {
            field: 'entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '14%',
        }, {
            field: 'fechaHora',
            displayName: 'Fecha y Hora',
            enableHiding: false,
            width: '14%',
        }];

        vm.gridOptions;

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        //#endregion

        //#region Metodos

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function mostrarResultados() {
            return vm.buscando === false && vm.sinResultados === false;
        }

        function mostrarMensajeRespuesta() {
            if (vm.Mensaje) {
                vm.mostrarMensajeProyectos = true;
            } else {
                vm.mostrarMensajeProyectos = false;
            }
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        async function init() {
            try {
                cargarLogs();
            }
            catch (err) {
                _mostarToast('Ocurrió un error al intentar recupera la lista de subprocesos');
            }
        }

        function cargarLogs() {
            vm.buscando = true;
            if (IdInstancia) {
                servicioLogsModal.obtener(IdInstancia, IdNivel).then((result) => {
                    if (result.data.length > 0) {
                        vm.model.logs = result.data.map(a => ({
                            id: a.Id,
                            flujoId: a.FlujoId,
                            instanciaId: a.InstanciaId,
                            usuario: a.NombreUsuario,
                            fechaHora: $filter('date')(a.FechaCreacion, 'dd/MM/yyyy HH:mm'),
                            proceso: a.Proceso,
                            de: a.De,
                            operacion: a.Operacion,
                            a: a.A,
                            rol: a.Rol,
                            entidad: a.Entidad
                        }));
                        vm.sinResultados = false;
                        vm.buscando = false;
                    }
                    else {
                        vm.model.logs = [];
                        vm.sinResultados = true;
                        vm.buscando = false;
                    }
                    vm.gridOptions.data = vm.model.logs;
                });
            }
            else {
                vm.buscando = false;
                vm.gridOptions.data = vm.model.logs;
            }
        }

        this.$onInit = function () {

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    enablePaginationControls: true,
                    useExternalPagination: false,
                    useExternalSorting: false,
                    paginationCurrentPage: 1,
                    enableVerticalScrollbar: 1,
                    enableFiltering: false,
                    showHeader: true,
                    useExternalFiltering: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: onRegisterApi
                };

                console.log(vm.gridOptions)
                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.model.logs;

                vm.buscando = false;
                vm.sinResultados = false;
                mostrarMensajeRespuesta();
            }
        };


        vm.init = init;
        vm.mostrarResultados = mostrarResultados;
        vm.cerrar = cerrar;

        //#endregion
    }
})();