(function () {
    'use strict';

    listaNotificacionesMantenimientoController.$inject = [
        '$scope',
        'constantesAutorizacion',
        '$uibModal',
        'utilidades',
        'FileSaver',
        '$location',
        'sesionServicios',
        'servicioNotificacionesMantenimiento',
        'servicioUsuarios',
        'servicioNotificacionesMensajes'
    ];

    function listaNotificacionesMantenimientoController(
        $scope,
        constantesAutorizacion,
        $uibModal,
        utilidades,
        FileSaver,
        $location,
        sesionServicios,
        servicioNotificacionesMantenimiento,
        servicioUsuarios,
        servicioNotificacionesMensajes
    ) {
        var vm = this;

        /// Filtro
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrar = filtrar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;

        vm.NombreNotificacionFiltro = "";
        vm.FechaInicioFiltro = "";
        vm.FechaFinFiltro = "";
        vm.NombreNotificacionFiltro = "";
        vm.filtro = {
            NombreNotificacionFiltro: "",
            FechaInicioFiltro: null,
            FechaFinFiltro: null,
            TipoFiltro: null,
        }

        /// Modais
        vm.abrirModalEliminar = abrirModalEliminar;

        /// Actions
        //vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.editarNotificacion = editarNotificacion;
        vm.visualizarNotificacion = visualizarNotificacion;
        vm.crearNotificacion = crearNotificacion;

        /// Plantilla de acciones
        vm.plantillaAccionesTabla = 'src/app/notificacionesMantenimiento/plantillas/plantillaAccionesTablaListagemNotificaciones.html';
        vm.cellTemplateTipo = 'src/app/notificacionesMantenimiento/plantillas/plantillaTipo.html';

        /// Definiciones de componente
        vm.opciones = {
            nivelJerarquico: 0,
            gridOptions: {
                //showHeader: true,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 5,
                onRegisterApi: onRegisterApi,
                //expandableRowHeight: 200
            }
        };
        /// Definiciones de columna componente
        vm.columnDef = [{
            field: 'NombreNotificacion',
            displayName: 'Nombre notificacion',
            enableHiding: false,
            width: '25%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'FechaInicioFormateado',
            displayName: 'Fecha y hora inicio',
            cellFilter: 'date:\'dd/MM/yyyy - HH:mm\'',
            enableHiding: false,
            width: '20%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'FechaFinFormateado',
            displayName: 'Fecha y hora final',
            cellFilter: 'date:\'dd/MM/yyyy - HH:mm\'',
            enableHiding: false,
            width: '20%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'Tipo',
            displayName: 'Tipo',
            width: "20%",
            cellTemplate: vm.cellTemplateTipo,
        }, {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTabla,
            width: '14%'
        }];

        /// Comienzo
        vm.init = function () {
            listarNotificaciones();

        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        /// Getters
        function listarNotificaciones() {

            var filtro = {
                ConfigNotificacionIds: null,
                UsuarioNotificacionIds: null,
                Visible: null,
                NotificacionesLeida: null,
                Tipo: vm.filtro.TipoFiltro,
                ProcedimientoAlmacenadoId: null,
                NombreNotificacion: vm.filtro.NombreNotificacionFiltro,
                NombreArchivo: null,
                IdUsuarioDNP: null,
                EsManual: null,
                FechaInicio: vm.filtro.FechaInicioFiltro,
                FechaFin: vm.filtro.FechaFinFiltro,
            };


            servicioNotificacionesMantenimiento.obtenerListaNotificaciones(filtro)
                .then(function (response) {
                    response.data.forEach(item => {
                        var localInicio = moment.utc(item.FechaInicio).toDate();
                        item.FechaInicioFormateado = moment(localInicio).format('YYYY-MM-DD HH:mm:ss');
                        
                        var localFin = moment.utc(item.FechaFin).toDate();
                        item.FechaFinFormateado = moment(localFin).format('YYYY-MM-DD HH:mm:ss');
                    });
                    vm.datos = response.data;
                }, function (error) {
                    //TODO: error handler
                    console.log("ListaNotificaiconesMantoCtrl.obtenerListaMensajes => ", error);
                    toastr('Error en la carga d einformación. HTTP');
                });
        }

        /// Actions
        function downloadExcel() {
            servicioUsuarios.obtenerExcelNotificaciones(vm.datos).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "NotificacionesMantenimiento.xls");
            }, function (error) {
                console.log("error", error);
            });
        }

        /**
         * 
         * @desciption .Obtiene los datos actuales y los descarga creando un archivo PDF de la información
         * @param {Array} datos. Arreglo de la información a mostrar en el PDF de descarga.
         */
        vm.DescargarPDF = function (datos) {
            try {
                if (datos !== undefined && datos !== null) {


                    servicioNotificacionesMantenimiento.obtenerPDF(datos).then(response => {


                        let dataResponse = response.data;

                        // verificar si existe una exepción en la descarga
                        if (Boolean(dataResponse.EsExcepcion) === true) {

                            console.log('NotificacionesMantenimientoCtrl.DescargarPDF => ', String(dataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar PDF');
                        } else {

                            let file = dataResponse.Datos;

                            // obtener el archivo binario y convertirlo en un blob de JavaScript
                            let bytes = new Uint8Array(file.FileContents);
                            let blob = new Blob([bytes], {
                                type: file.ContentType
                            });

                            FileSaver.saveAs(blob, file.FileDownloadName);
                        }

                    });

                }
            } catch (exception) {
                console.log('NotificacionesMantenimientoCtrl.DescargarPDF => ', exception);
                toastr.error('Ocurrió un error al descargar PDF');
            }
        };

        /**
         * 
         * @description Evento provocado al presionar el obtón descargar PDF.
         * @param {Event} $event. Evento provocado 
         * @param {HTMLElement} sender. Componente HTML que provoca el evento 
         */
        vm.aDescargaPdf_onClick = function ($event, sender) {
            try {

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
                };

                vm.DescargarPDF(vm.datos);
                
            } catch (exception) {
                console.log('NotificacionMantenimientoCtrl.aDescargaPdf_onClick => ', exception);
                toastr.error('Error en la descarga del archivo PDF');
            }
        };

        function nombreDelArchivo(response) {
            var filename = "";
            var disposition = response.headers("content-disposition");
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            return filename;
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }


        function filtrar() {
            return listarNotificaciones()
        }

        function limpiarCamposFiltro() {
            vm.filtro = {
                NombreNotificacionFiltro: "",
                FechaInicioFiltro: null,
                FechaFinFiltro: null,
                TipoFiltro: null,
            }
            listarNotificaciones();
        }

        // Modal Eliminar
        function abrirModalEliminar(obj) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioNotificacionesMantenimiento.eliminarNotificacion(obj.Id)
                    .then(function (response) {
                        if (response.data) {
                            if (response.data.HttpStatusCode == 200) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!");
                                listarNotificaciones();
                            } else {
                                utilidades.mensajeError(response.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                    })
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            })
        }

        function visualizarNotificacion($row) {
            servicioNotificacionesMantenimiento.visualizarContenidoNotificacion($row);
        }

        function editarNotificacion($row) {
            $location.path(`/notificacionesMantenimiento/${$row.Id}`);
        }

        function crearNotificacion() {
            $location.path("/notificacionesMantenimiento");
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('listaNotificacionesMantenimientoController', listaNotificacionesMantenimientoController);
})();