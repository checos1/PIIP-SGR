(function () {
    'use strict';

    modalDocumentosController.$inject = [
        'objTramite',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaTramites',
        'FileSaver',
        'archivoServicios',
        '$sessionStorage'
    ];

    function modalDocumentosController(
        objTramite,
        $uibModalInstance,
        utilidades,
        servicioConsolaTramites,
        FileSaver,
        archivoServicios,
        $sessionStorage
    ) {
        var vm = this;

        vm.obtenerDocumento = obtenerDocumento;
        console.log('objTramite', objTramite);

        vm.idAplicacion = 'f76bca19-8116-4157-aa89-d8a41759e79d';
        vm.gridOptions;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.gridOptions = {
            columnDefs: [{
                field: 'Nombre',
                displayName: 'Descripción del documento',
                enableHiding: false,
                width: '60%'
            },
            {
                field: 'Fecha',
                displayName: 'Fecha',
                cellFilter: 'date:\'dd/MM/yyyy hh:mm\'',
                enableHiding: false,
                width: '25%',
                sort: {
                    direction: 'desc',
                    priority: 0
                }
            },
            {
                field: 'accion',
                displayName: 'Acción',
                headerCellClass: 'text-center',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.obtenerDocumento(row.entity)" tooltip-placement="Auto" uib-tooltip="Descargar">' +
                    '    <span style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionDownload.png"></span>' +
                    '</button></div>',
                width: '14%'
            }],
            enableVerticalScrollbar: true,
            enableSorting: true,
            showHeader: true,
            showGridFooter: false
        };

        /// Comienzo
        vm.init = function () {
            vm.gridOptions.data = [];

            //TEST
            objTramite.IdInstancia = "4bad4259-26eb-41c0-9302-f4498ced3836";
            servicioConsolaTramites.obtenerDocumentos(objTramite.IdInstancia)
                .then(response => {
                    console.log(response.data);
                    if (response != null)
                        vm.gridOptions.data = response.data;
                })
        }

        function obtenerDocumento(entity) {

            archivoServicios.obtenerArchivoBytes(entity.Metadatos.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.Metadatos.ContentType);
                //const blob = new Blob([retorno], { type: entity.Metadatos.ContentType });
                FileSaver.saveAs(blob, entity.Nombre);
            }, function (error) {
                toastr.error("Error inesperado al descargar");
            });
        };
    }

    angular.module('backbone').controller('modalDocumentosController', modalDocumentosController);
})();