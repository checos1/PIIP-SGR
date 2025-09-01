(function () {
    'use strict';

    FormularioPruebaController.$inject = [
        '$scope',
        'constantesAutorizacion',
        '$uibModal',
        'utilidades',
        'FileSaver',
        '$location',
        'sesionServicios',
        'servicioNotificacionesMantenimiento',
        'servicioUsuarios',
        'servicioNotificacionesMensajes',
        '$sessionStorage',
        '$http',
        'constantesBackbone',
        "servicioAcciones",
        '$q',
        "$filter"
    ];

    function FormularioPruebaController(
        $scope,
        constantesAutorizacion,
        $uibModal,
        utilidades,
        FileSaver,
        $location,
        sesionServicios,
        servicioNotificacionesMantenimiento,
        servicioUsuarios,
        servicioNotificacionesMensajes,
        $sessionStorage,
        $http,
        constantesBackbone,
        servicioAcciones,
        $q,
        $filter
    ) {
        var vm = this;

        /// Filtro
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrar = filtrar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.refrescarPagina = refrescarPagina;
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
        vm.guardarPrueba = guardarPrueba;
        vm.seleccionarAccionDevolver = seleccionarAccionDevolver;
        vm.BuscarAccionesDevolucion = buscarAccionesDevolucion;
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
            buscarAccionesDevolucion();

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

        function guardarPrueba() {
            var postDefinitivo = true;
            var parametrosEjecucionFlujo = new Object();
            parametrosEjecucionFlujo.IdInstanciaFlujo = $sessionStorage.idInstanciaFlujoPrincipal;//= vm.idInstancia;
            parametrosEjecucionFlujo.IdAccion = $sessionStorage.idAccion;
            parametrosEjecucionFlujo.PostDefinitivo = postDefinitivo;
            parametrosEjecucionFlujo.ObjetoContexto = new Object();
            //parametrosEjecucionFlujo.ObjetoContexto.IdRol = '1dd225f4-5c34-4c55-b11d-e5856a68839b';
            parametrosEjecucionFlujo.ObjetoContexto.IdUsuario = usuarioDNP;
            parametrosEjecucionFlujo.ObjetoDatos = new Object();

            $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiEjecutarFlujo, parametrosEjecucionFlujo).then(

                function (resultado) {
                    if (resultado.data !== null) {
                        if (postDefinitivo) {
                            if (resultado.status === 200) {
                                $sessionStorage.fichaPlantilla = undefined;
                                $sessionStorage.Ficha = undefined;
                                $sessionStorage.guardadoPrevio = true;

                                utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                                    false,
                                    vm.refrescarPagina,
                                    null);
                            }
                            else {
                                console.log('entro2');
                                utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                            }
                        }
                        else {
                            if (resultado.status === 200) {
                                sAlert.success($filter('language')('GuardadoTemporal'), 'mensaje').autoRemove();
                            }
                            else {
                                console.log('entro3');
                                utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                            }
                        }
                    }
                    else {
                        console.log('entro4');
                        utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                    }
                }
            ).catch(function (e) {

                var mensaje;
                var mensajeRecurso = $filter('language')('ErrorGuardadoTemporal');

                if (e.data === undefined)
                    mensaje = e.message;
                else {
                    if (e.data.ExceptionMessage && (e.data.ExceptionMessage.startsWith("{"))) {
                        try {
                            var excepcion = angular.fromJson(e.data.ExceptionMessage);
                            mensaje = utilidades.generarLogExcepcionHTML(excepcion);
                        } catch (e) {
                            mensaje = $filter('language')('ImposiblePresentarError');
                        }
                    } else
                        mensaje = e.data.ExceptionMessage;
                }
                console.log('entro5');
                utilidades.mensajeError(mensajeRecurso.replace("[0]", mensaje));

            });
        }

        function refrescarPagina() {
            location.reload();
        }

        function seleccionarAccionDevolver() {
            if (!vm.accionDevolverSeleccionada) {
                vm.accionDevolverSeleccionada = vm.accionSeleccionada
                    ? angular.copy(vm.accionSeleccionada.Flujo)
                    : null;
            }

            vm.seleccionarAccionModal = $uibModal.open({
                animation: true,
                templateUrl: '/src/app/panelEjecucionDeAccion/listarAccionesAnteriores.html',
                controller: 'listarAccionesAnterioresController',
                controllerAs: "vm",
                keyboard: false,
                backdrop: false,
                scope: $scope,
                size: "sm",

                resolve: {
                    listaacciones: $q.resolve(vm.AccionesDevolucion),
                    idInstancia: $q.resolve(vm.idInstancia),
                    idAccion: $q.resolve($sessionStorage.idAccion),
                    idAplicacion: $q.resolve($sessionStorage.IdAplicacion),
                    existeFichaGenerar: $q.resolve($sessionStorage.fichaPlantilla !== undefined)
                }
            });

        }

        function buscarAccionesDevolucion() {
            if (vm.idInstancia === undefined || vm.idInstancia === null)
                vm.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal;//= vm.idInstancia;
            return servicioAcciones.ObtenerAccionesDevolucion(vm.idInstancia, $sessionStorage.idAccion).then(
                function (resultado) {
                    if (resultado.data && resultado.data.Result.length > 0) {

                        vm.ExisteAccionesDevolver = true;
                        vm.AccionesDevolucion = resultado.data.Result;
                    }
                    else
                        vm.ExisteAccionesDevolver = false;
                });
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('FormularioPruebaController', FormularioPruebaController);
})();