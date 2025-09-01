(function () {
    'use strict';

    angular.module('backbone').controller('FichaTemplateController', FichaTemplateController)
        .filter('bytes', filterBytes);

    function filterBytes() {
        return function (bytes, precision) {
            if (bytes === 0) { return '0 bytes' }
            if (isNaN(parseFloat(bytes)) || !isFinite(bytes)) return '-';
            if (typeof precision === 'undefined') precision = 1;

            var units = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB'],
                number = Math.floor(Math.log(bytes) / Math.log(1024)),
                val = (bytes / Math.pow(1024, Math.floor(number))).toFixed(precision);

            return (val.match(/\.0*$/) ? val.substr(0, val.indexOf('.')) : val) + ' ' + units[number];
        }
    }
    
    FichaTemplateController.$inject = [
        '$scope', '$filter', '$q', '$sessionStorage', 'archivoServicios', 'utilidades', '$uibModalInstance', 'params'
    ];

    function FichaTemplateController($scope, $filter, $q, $sessionStorage, archivoServicios, utilidades, $uibModalInstance, params) {
        var vm = this;
    
        //Variables
        vm.gridApi;
        vm.gridFichas;
        vm.botonesTemplate =
            '<div class="text-center contenedor-acciones">' +
            '<button type="button" class="btnaccion" ' +
            'ng-hide="row.entity.IdArchivoBlob === null" ' +
            'ng-click="grid.appScope.vm.descargarArchivo(row.entity)" ' +
            'tooltip-placement="bottom" uib-tooltip="{{ \'Descargar\' | language }}" >' +
            '<span aria-hidden="true"><img src="Img/iconsgrid/iconAccionDownload.png" alt="Descargar"/></span>' +
            '</button>' +
            '</div>';
        
        //Metodos
        vm.salir = salir;
        vm.descargarArchivo = descargarArchivo;
        
        function salir() {
            $uibModalInstance.close(null);
        }
        
        function obtenerDatos(){
            var params = {
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                Tipo: 'Ficha'
            };

            return $q(function (resolve, reject) {
                archivoServicios.obtenerListadoArchivos(params, $sessionStorage.IdAplicacion).then(function (response) {
                    var data = _.filter(response, function (r) { return r.status !== 'Descartado'; });

                    if (_.size(data) > 0) {
                        var dataTemp = [];
                        
                        _.each(data, function (o) {
                            var archivoTemp = {
                                Id: o.id.toString(),
                                PasoAccion: o.metadatos.nombreaccion,
                                Nombre: o.nombre.slice(0, ((utilidades.obtenerExtensionArchivo(o.nombre).length + 1) * -1)),
                                NombreCompleto: o.nombre,
                                Status: o.status.trim(),
                                ContenType: o.metadatos.contenttype,
                                Tamano: $filter('bytes')(o.metadatos.size),
                                TamanioReal: o.metadatos.size,
                                Extension: o.metadatos.extension,
                                IdArchivoBlob: o.metadatos.idarchivoblob,
                                FechaCreacionArchivo: o.metadatos.fechacreacionarchivo,
                                FechaCreacion: o.metadatos.fechacreacion
                            };

                            dataTemp.push(archivoTemp);

                            vm.gridFichas.data = _.sortBy(dataTemp, 'FechaCreacion');
                        });
                    }
                    
                    resolve(data);
                }, function(error){
                    reject(error);
                });
            });
        }

        /**
         * Permite descargar el archivo seleccionado
         * @param {object} entity
         */
        function descargarArchivo(entity) {
            if (entity.IdArchivoBlob != undefined) {
                archivoServicios.obtenerArchivoBytes(entity.IdArchivoBlob, $sessionStorage.IdAplicacion).then(function (retorno) {
                    const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                    //var blob = new Blob([retorno], {
                    //    type: entity.ContenType
                    //});
                    FileSaver.saveAs(blob, entity.NombreCompleto);
                }, function (error) {
                    toastr.error("Error inesperado al descargar");
                });
            }
        }

        function constructor() {
            return $q(function(resolve, reject){
                var columns = [
                    {
                        name: 'Nombre',
                        displayName: 'Nombre',
                        enableCellEdit: false,
                        enableColumnMenu: false,
                        enableSorting: true
                    }, //Columna de Nombre de archivo
                    {
                        name: 'Extension',
                        displayName: 'Extensión',
                        enableCellEdit: false,
                        enableColumnMenu: false,
                        enableSorting: true
                    }, //Columna de Extensión de archivo
                    {
                        name: 'Tamano',
                        displayName: 'Tamaño',
                        enableCellEdit: false,
                        enableColumnMenu: false,
                        enableSorting: true
                    }, //Columna de Tamaño de archivo
                    {
                        name: 'Acciones',
                        cellTemplate: vm.botonesTemplate,
                        enableCellEdit: false,
                        enableSorting: false,
                        enableColumnMenu: false,
                        enableFiltering: false
                    } //Columna de boton de acciones
                ]; //Columnas del grid

                vm.gridFichas = {
                    paginationPageSizes: [],
                    paginationPageSize: 3,
                    enablePaginationControls: true,
                    columnDefs: columns,
                    rowHeight: 180,
                    enableCellSelection: true,
                    enableRowSelection: false,
                    enableColumnMenu: false,
                    enableCellEditOnFocus: true,
                    rowEditWaitInterval: -1,
                    onRegisterApi: function (gridApi) {
                        vm.gridApi = gridApi;
                    }
                }; //Grid de archivos
                
                resolve();
            });
        }
        
        constructor().then(function(){
            obtenerDatos();
        });
    }
})();