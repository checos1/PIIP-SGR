(function () {
    'use strict';

    angular.module('backbone').controller('configurarMensajeModalController', configurarMensajeModalController);

    configurarMensajeModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioProgramacion',
        'Programacion',
        'utilidades',
        '$filter',
        'servicioNotificacionesMantenimiento',
        'servicioActualizarFechasModal'
    ];

    function configurarMensajeModalController(
        $scope,
        $uibModalInstance,
        servicioProgramacion,
        Programacion,
        utilidades,
        $filter,
        servicioNotificacionesMantenimiento,
        servicioActualizarFechasModal
    ) {
        var vm = this;
        console.log(Programacion);

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;

        vm.cellTemplateTipo = 'src/app/notificacionesMantenimiento/plantillas/plantillaTipo.html';


        //#region Variables
        vm.capitulo = Programacion.capitulo;
        vm.tipoEntidad = Programacion.TipoEntidad;

        vm.columnDef = [{
            field: 'NombreNotificacion',
            displayName: 'Nombre notificacion',
            enableHiding: false,
            width: '38%',
        }, {
            field: 'FechaInicioFormateado',
            displayName: 'Fecha y hora inicio',
            cellFilter: 'date:\'dd/MM/yyyy - HH:mm\'',
            enableHiding: false,
            width: '20%',
        }, {
            field: 'FechaFinFormateado',
            displayName: 'Fecha y hora final',
            cellFilter: 'date:\'dd/MM/yyyy - HH:mm\'',
            enableHiding: false,
            width: '20%',
        }, {
            field: 'Tipo',
            displayName: 'Tipo',
            width: "18%",
            cellTemplate: vm.cellTemplateTipo,
        }];

        vm.gridOptions = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 31,
            multiSelect: false,
            columnDefs: vm.columnDef,
            onRegisterApi: onRegisterApi
        };

        function listarNotificacionesPantalla() {

            var filtro = {
                ConfigNotificacionIds: null,
                UsuarioNotificacionIds: null,
                Visible: null,
                NotificacionesLeida: null,
                Tipo: null,
                ProcedimientoAlmacenadoId: null,
                NombreNotificacion: null,
                NombreArchivo: null,
                IdUsuarioDNP: null,
                EsManual: null,
                FechaInicio: null,
                FechaFin: null,
                PantallaProgramacion: true
            };


            servicioNotificacionesMantenimiento.obtenerListaNotificaciones(filtro)
                .then(function (response) {
                    console.log(response.data);
                    response.data.forEach(item => {
                        var localInicio = moment.utc(item.FechaInicio).toDate();
                        item.FechaInicioFormateado = moment(localInicio).format('YYYY-MM-DD HH:mm:ss');

                        var localFin = moment.utc(item.FechaFin).toDate();
                        item.FechaFinFormateado = moment(localFin).format('YYYY-MM-DD HH:mm:ss');
                    });
                    vm.gridOptions.data = response.data;
                }, function (error) {
                    console.log("servicioNotificacionesMantenimiento.obtenerListaNotificaciones => ", error);
                    toastr('Error en la carga d einformación. HTTP');
                });
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function guardar() {
            let item = $scope.gridApi.selection.getSelectedRows();
            if (item.length == 0) {
                toastr.warning('Seleccione una mensaje');
                return;
            }

            console.log(item);

            Programacion.IdNotificacion = item[0].Id;

            servicioActualizarFechasModal.Guardar(Programacion).then(result => {
                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                vm.cerrar();
            });
        }

        function cerrar() {
            $uibModalInstance.close(vm.data);
        }

        async function init() {
            await listarNotificacionesPantalla();
            esperarPorRender();
        }

        function esperarPorRender() {
            setTimeout(() => {
                if (vm.gridOptions.data && $scope.gridApi.grid.columns && $scope.gridApi.grid.columns.length > 0) {
                    seleccionarRenglon();
                }
                else
                    esperarPorRender();
            }, 1000);
        }

        function seleccionarRenglon() {
            // - Seleccionar la entidad
            // - Verificar si ya tiene una notificación seleccionada
            if (Programacion.IdNotificacion) {
                let indexNotificacion = vm.gridOptions.data.findIndex(f => f.Id === Programacion.IdNotificacion);
                if (indexNotificacion >= 0) {
                    $scope.gridApi.grid.modifyRows(vm.gridOptions.data);
                    $scope.gridApi.selection.selectRow(vm.gridOptions.data[indexNotificacion]);
                }
            }
        }
    }
})();