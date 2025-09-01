(function () {
    'use strict';

    documentoSoporteController.$inject = ['$sessionStorage', 'archivoServicios', '$scope', 'servicioAcciones'];

    function documentoSoporteController(
        $sessionStorage, archivoServicios, $scope, servicioAcciones
    ) {
        const vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.descargarArchivoBlob = descargarArchivoBlob;
        vm.consultarArchivosTramite = consultarArchivosTramite;
        vm.gridOptions;
        vm.archivosLoad = [];
        vm.listTipoArchivo = [];

        vm.columnDef = [
            {
                field: 'codigoProceso',
                displayName: 'Código del Proceso',
                enableHiding: false,
                enableColumnMenu: false,
                width: 200,
                pinnedRight: true
            },
            {
                field: 'fecha',
                displayName: 'Fecha',
                enableHiding: false,
                enableColumnMenu: false,
                width: 100,
                pinnedRight: true
            },
            {
                field: 'nombreArchivo',
                displayName: 'Archivo',
                enableHiding: false,
                enableColumnMenu: false,
                width: 200,
                pinnedRight: true

            },
            {
                field: 'tipoDocumentoSoporte',
                displayName: 'Tipo de Documento Soporte',
                enableHiding: false,
                enableColumnMenu: false,
                width: 200,
                pinnedRight: true
            },
            {
                field: 'nivel',
                displayName: 'Nivel que Agrega',
                enableHiding: false,
                enableColumnMenu: false,
                width: 150,
                pinnedRight: true

            },
            {
                field: 'obligatorio',
                displayName: 'Obligatorio',
                enableHiding: false,
                enableColumnMenu: false,
                width: 50,
                pinnedRight: true

            },
            {
                field: 'accion',
                displayName: 'Ver',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                pinnedRight: true,
                cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.descargarArchivoBlob(row.entity)" tooltip-placement="Auto" uib-tooltip="Descargar">' +
                    '    <span style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionDownload.png"></span>' +
                    '</button></div>',
                width: 50,
                cellClass: 'text-center'
            }
        ];
        //#endregion
        if (!vm.gridOptions) {
            vm.gridOptions = {
                columnDefs: vm.columnDef,
                enableColumnResizing: false,
                showGridFooter: false,
                enablePaginationControls: true,
                useExternalPagination: false,
                useExternalSorting: false,
                paginationCurrentPage: 1,
                enableVerticalScrollbar: 1,
                enableFiltering: false,
                useExternalFiltering: false,
                paginationPageSizes: [10, 15, 25, 50, 100],
                paginationPageSize: 10,
                onRegisterApi: onRegisterApi
            };

        }

        //#region Metodos
        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, "proyectos").then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                //var blob = new Blob([retorno], {
                //    type: entity.ContenType
                //});
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = entity.nombreArchivo;
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }


        function consultarArchivosTramite() {
            servicioAcciones.obtenerInstanciaProyecto($sessionStorage.idInstanciaIframe, $sessionStorage.BPIN).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                }
                else {
                    let param = {
                        bpin: $sessionStorage.BPIN,
                        idinstancia: response.data.IdInstanciaProyecto
                    };

                    archivoServicios.obtenerListadoArchivos(param, "proyectos").then(function (response) {
                        if (response === undefined || typeof response === 'string') {
                            vm.mensajeError = response;
                            utilidades.mensajeError(response);
                        } else {
                            vm.gridOptions.columnDefs = [];
                            vm.archivosLoad = [];
                            response.forEach(archivo => {
                                if (archivo.status != 'Eliminado') {
                                    vm.archivosLoad.push({
                                        codigoProceso: archivo.metadatos.codigoproceso,
                                        fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                        nombreArchivo: archivo.nombre,
                                        tipoDocumentoSoporte: archivo.metadatos.tipodocumentosoporte,
                                        idArchivoBlob: archivo.metadatos.idarchivoblob,
                                        obligatorio: archivo.metadatos.obligatorio,
                                        nivel: archivo.metadatos.nivel,
                                        idNivel: archivo.metadatos.idnivel,
                                        ContenType: archivo.metadatos.contenttype,
                                        idMongo: archivo.id
                                    });
                                }


                            });

                            vm.gridOptions.showHeader = true;
                            vm.gridOptions.columnDefs = vm.columnDef;
                            vm.gridOptions.data = vm.archivosLoad;
                            vm.filasFiltradas = vm.gridOptions.data.length > 0;

                            $sessionStorage.contObligatoriosNoIngresados = 0;
                            vm.listTipoArchivo.forEach(tipoArchivo => {
                                if (tipoArchivo.Obligatorio === true) {
                                    vm.tieneArchivosAdjuntos = false;
                                    vm.archivosLoad.forEach(archivo => {
                                        if (archivo.tipoDocumentoSoporte === tipoArchivo.Name) {
                                            vm.tieneArchivosAdjuntos = true;
                                        }
                                    });

                                    if (vm.tieneArchivosAdjuntos === false) {
                                        $sessionStorage.contObligatoriosNoIngresados = $sessionStorage.contObligatoriosNoIngresados + 1;
                                    }
                                }
                            });

                            localStorage.setItem('contObligatoriosNoIngresados', $sessionStorage.contObligatoriosNoIngresados);

                        }
                    }, error => {
                        console.log(error);
                    });
                }
            }, error => {
                console.log(error);
            });
            
        }

        vm.obtener = function () {
            var idTramite = $sessionStorage.TramiteId;
            var proyectoId = $sessionStorage.ProyectoId;
            var tipoTramiteId = $sessionStorage.TipoTramiteId;
            var tipoRolId = $sessionStorage.TipoRolId;
            var TipoProyecto = $sessionStorage.TipoProyecto;

            consultarArchivosTramite();

        };

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }
        

    }

    angular.module('backbone').component('documentoSoporte', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/documentoSoporte/documentoSoporte.html",
        controller: documentoSoporteController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();