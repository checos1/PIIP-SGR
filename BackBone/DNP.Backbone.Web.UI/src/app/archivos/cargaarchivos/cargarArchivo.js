(function () {
    'use strict';
    angular.module('backbone').directive('cargarArchivo', cargarArchivo);
    cargarArchivoController.$inject = [
        '$scope',
        '$filter',
        'archivoServicios',
        'utilidades',
        'array.extensions',
        'trasladosServicio',
        '$timeout',
        '$window',
        '$sessionStorage'
        
    ];

    function cargarArchivoController(
        $scope,
        $filter,
        archivoServicios,
        utilidades,
        trasladosServicio,
        $timeout,
        $window,
        $sessionStorage) {
        const vm = this;
        //#region Variables
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.allArchivos = false;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.descargarArchivoBlob = descargarArchivoBlob;
        vm.eliminarArchivoBlob = eliminarArchivoBlob;
        vm.consultarArchivosTramite = consultarArchivosTramite;
        vm.gridOptions;
        vm.archivosLoad = [];
        vm.listTipoArchivo = [];

        vm.columnDef = [
            {
                field: 'codigoProceso',
                displayName: 'Código de Proceso',
                enableHiding: false,
                enableColumnMenu: false,
                width: '15%',
                pinnedRight: true
            },
            {
                field: 'descripcionTramite',
                displayName: 'Descripción del Trámite',
                enableHiding: false,
                enableColumnMenu: false,
                width: '20%',
                pinnedRight: true
            },
            {
                field: 'fecha',
                displayName: 'Fecha',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true
            },
            {
                field: 'nombreArchivo',
                displayName: 'Archivo',
                enableHiding: false,
                enableColumnMenu: false,
                width: '20%',
                pinnedRight: true
                
            },
            {
                field: 'tipoDocumentoSoporte',
                displayName: 'Tipo de Documento Soporte',
                enableHiding: false,
                enableColumnMenu: false,
                width: '20%',
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
                    '</button><button class="btnaccion" ng-click="grid.appScope.vm.eliminarArchivoBlob(row.entity)" tooltip-placement="Auto" uib-tooltip="Eliminar" ng-show="grid.appScope.vm.noJefePlaneacion">' +
                    '    <span style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionEliminar.png"></span>' +
                    '</button></div>',
                width: '10%',
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

        
        $scope.$watch('modelo', function () {
            setTimeout(function () {
                vm.listTipoArchivo = [];
                vm.noJefePlaneacion = vm.modelo.noJefePlaneacion;
                vm.disabledJefePlaneacion = vm.modelo.disabledJefePlaneacion;
                vm.allArchivos = vm.modelo.allArchivos == true ? true : vm.allArchivos;
                if (vm.modelo.soloLectura) {
                    vm.noJefePlaneacion = false;
                    vm.disabledJefePlaneacion = true;
                }
                consultarArchivosTramite();
                function obtenerTiposDocumentos() {
                    return archivoServicios.obtenerTipoDocumentoTramite(vm.modelo.idTipoTramite);
                }

                obtenerTiposDocumentos()
                    .then(function (response) {
                        if (response.data !== null && response.data.length > 0) {

                            response.data.forEach(archivo => {
                                vm.listTipoArchivo.push({
                                    Id: archivo.Id,
                                    Name: archivo.TipoDocumento,
                                    Obligatorio: archivo.Obligatorio
                                });
                            });

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
                        }
                        localStorage.setItem('contObligatoriosNoIngresados', $sessionStorage.contObligatoriosNoIngresados);
                    });

            }, 2000);

        });

     

        //#region Metodos
        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, vm.modelo.coleccion).then(function (retorno) {
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

        function eliminarArchivoBlob(entity) {
            archivoServicios.cambiarEstadoDataArchivo(entity.idMongo, vm.modelo.coleccion).then(function (retorno) {
                consultarArchivosTramite();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al eliminar");
            });
        }

       
        function consultarArchivosTramite() {
            let param = {
                idInstancia: vm.modelo.idInstancia,
                section: vm.modelo.section,
                idAccion: vm.modelo.idAccion
            };
            if (vm.allArchivos) {
                param = {
                    idInstancia: vm.modelo.idInstancia,
                };
            }

            

            archivoServicios.obtenerListadoArchivos(param, vm.modelo.coleccion).then(function (response) {
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
                                descripcionTramite: archivo.metadatos.descripciontramite,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumentoSoporte: archivo.metadatos.tipodocumentosoporte,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
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

        function adjuntarArchivo() {
            document.getElementById('file').value = "";
            document.getElementById('file').click();
        }

        $scope.fileNameChanged = function (input) {
            
            if (vm.tipoArchivo == "" || vm.tipoArchivo == null || vm.tipoArchivo == undefined) {
                utilidades.mensajeError("Debe seleccionar un tipo de archivo.");
                return;
            }
           
            if (input.files.length == 1)
                vm.filename = input.files[0].name;
            else
                vm.filename = input.files.length + " archivos"
            subirArchivo(input);
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'exe': case 'bin': case 'src': case 'vbs': return false;
                default: return true;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function subirArchivo(input) {
            for (var i = 0; i < input.files.length; i++) {
                vm.extension = obtenerExtension(input.files[i].name);
                if (!validarExtension(vm.extension)) {
                    utilidades.mensajeError('Extensión no permitida');
                    return;
                }
                var x = new Array();
                x = vm.modelo.ext.replaceAll('.', '').split(",");
                if (!x.includes(vm.extension)) {
                    utilidades.mensajeError('Extensión no permitida');
                    return;
                }
            }
            let tipoArchivo = vm.listTipoArchivo.find(x => x.Id == vm.tipoArchivo);
            for (var i = 0; i < input.files.length; i++) {
                vm.extension = obtenerExtension(input.files[i].name);
                let archivo = {
                    FormFile: input.files[i],
                    Nombre: input.files[i].name,
                    Metadatos: {
                        extension: vm.extension,
                        idInstancia: vm.modelo.idInstancia,
                        idAccion: vm.modelo.idAccion,
                        section: vm.modelo.section,
                        codigoProceso: vm.modelo.codigoProceso,
                        descripcionTramite: vm.modelo.descripcionTramite,
                        tipoDocumentoSoporte: tipoArchivo.Name
                    }
                };

                archivoServicios.cargarArchivo(archivo, vm.modelo.coleccion).then(function (response) {
                    if (response === undefined || typeof response === 'string') {
                        vm.mensajeError = response;
                        utilidades.mensajeError(response);
                    } else {
                        //Actualizar el idArchivoBlob en el pago de inflexibilidad
                        utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), false, false, false);
                        consultarArchivosTramite();
                    }
                }, error => {
                    console.log(error);
                });
            }
            
        };

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }
        
        

        //#endregion
    }
    
    function cargarArchivo() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelo: '='
            },
            templateUrl: 'src/app/archivos/cargaarchivos/cargarArchivo.html',
            controller: cargarArchivoController,
            controllerAs: 'vm',
            bindToController: true
        };
    }

})();