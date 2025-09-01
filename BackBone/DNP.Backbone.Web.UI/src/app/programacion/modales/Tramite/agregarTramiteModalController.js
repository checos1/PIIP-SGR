(function () {
    'use strict';

    angular.module('backbone')
        .controller('agregarTramiteModalController', agregarTramiteModalController);

    agregarTramiteModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioAgregarTramiteModal',
        'TipoEntidad',
        'FlujoIds',
        'utilidades'
    ];

    function agregarTramiteModalController(
        $scope,
        $uibModalInstance,
        servicioAgregarTramiteModal,
        TipoEntidad,
        FlujoIds,
        utilidades
    ) {
        const vm = this;

        //#region Variables
        vm.columnas = servicioAgregarTramiteModal.columnasPorDefectoModalTramites;
        vm.buscando = false;
        vm.sinResultados = false;
        vm.lang = "es";

        vm.nombreTramiteFilaTemplate = 'src/app/programacion/modales/tramite/plantillas/plantillaNombreTramite.html';

        vm.model = {
            Tramites: [
            ]
        };
        vm.macroprocesos = [];
        vm.procesos = [];
        vm.subprocesos = [];

        vm.filtro = {
            macroproceso: "",
            proceso: "",
            subproceso: "",
            get seleccionado() {
                if (this.subproceso !== "") return this.subproceso;
                if (this.proceso !== "") return this.proceso;
                if (this.macroproceso !== "") return this.macroproceso;
                return "";
            },
            limpiar: function () {
                this.subproceso = "";
                this.proceso = "";
                this.macroproceso = "";
                vm.procesos = [];
                vm.subprocesos = [];
            }
        }

        vm.columnDefPrincial = [{
            field: 'nombre',
            displayName: 'NOMBRE DEL TRÁMITE',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.nombreTramiteFilaTemplate,
        }];

        vm.gridOptions;

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        //#endregion

        //#region Metodos

        function buscar() {
            servicioAgregarTramiteModal.ObtenerTramites(vm.filtro.seleccionado)
                .then(resultado => {
                    vm.model.Tramites = [];
                    vm.model.Tramites = resultado.data.map(p => ({
                        Id: p.Id,
                        nombre: p.Nombre,
                        Seleccionado: false
                    }));

                    //Quita de la lista los trámites que ya están configurados
                    for (let i = 0; i < vm.model.Tramites.length; ) {
                        let tramite = vm.model.Tramites[i];
                        if (FlujoIds.includes(tramite.Id)) {
                            vm.model.Tramites.splice(i, 1);
                        }
                        else {
                            i++;
                        }
                    }

                    vm.gridOptions.data = vm.model.Tramites;
                }, err => {
                    console.log(err);
                });
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function guardarTodo() {
            return new Promise(async (resolve, reject) => {
                for (let i = 0; i < vm.model.Tramites.length;) {
                    let tramite = vm.model.Tramites[i];
                    if (tramite.Seleccionado) {
                        try {
                            //Guardar el registro en la base de datos
                            let resultado = await servicioAgregarTramiteModal.Guardar({
                                IdProgramacion: 0,
                                FlujoId: tramite.Id,
                                TipoEntidad: TipoEntidad,
                                FechaDesde: null,
                                FechaHasta: null,
                                FlujoTramite: null,
                                creado: false,
                                cerrado: false,
                                iniciarProceso: 0,
                            });
                            if (resultado) {
                                console.log(resultado);
                            }
                            //Borrar el registro de la lista de trámites
                            vm.model.Tramites.splice(i, 1);
                        }
                        catch (err) {
                            i++
                            console.log(err);
                            _mostarToast("Error al guardar registro ");
                        }
                    }
                    else {
                        i++;
                    }
                }
                resolve(true);
            });
        }

        function limpiarGridTramites() {
            vm.model.Tramites = [];
            vm.gridOptions.data = vm.model.Tramites;
            $scope.gridApi.core.refresh();
        }

        function guardar() {
            guardarTodo().then(() => {
                vm.gridOptions.data = vm.model.Tramites;
                $scope.gridApi.core.refresh();
                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
            });
        }

        function mostrarResultados() {
            return vm.buscando === false && vm.sinResultados === false;
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        async function cargarMacroprocesos() {
            servicioAgregarTramiteModal.ObtenerNiveles(null, 'MACROPROCESO')
                .then(resultado => {
                    vm.macroprocesos = resultado.data;
                })
        }

        async function cargarProcesos(macroproceso) {
            servicioAgregarTramiteModal.ObtenerNiveles(macroproceso, 'PROCESO')
                .then(resultado => {
                    vm.procesos = resultado.data;
                    limpiarGridTramites();
                })
        }

        async function cargarSubprocesos(proceso) {
            servicioAgregarTramiteModal.ObtenerNiveles(proceso, 'SUBPROCESO')
                .then(resultado => {
                    vm.subprocesos = resultado.data;
                    limpiarGridTramites();
                })
        }

        async function init() {
            try {
                await cargarMacroprocesos();
            }
            catch (err) {
                _mostarToast('Ocurrió un error al intentar recupera la lista de subprocesos');
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

                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.model.Tramites;
            }
        };


        vm.init = init;
        vm.mostrarResultados = mostrarResultados;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.buscar = buscar;
        vm.cargarProcesos = cargarProcesos;
        vm.cargarSubprocesos = cargarSubprocesos;
        //#endregion
    }
})();