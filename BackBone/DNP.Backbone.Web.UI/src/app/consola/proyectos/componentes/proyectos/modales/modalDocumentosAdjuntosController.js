(function () {
    'use strict';

    modalDocumentosAdjuntosController.$inject = [
        'objProyecto',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaProyectos',
        'FileSaver',
    ];

    function modalDocumentosAdjuntosController(
        objProyecto,
        $uibModalInstance,
        utilidades,
        servicioConsolaProyectos,
        FileSaver
    ) {
        var vm = this;

        vm.obtenerDocumentoAdjunto = obtenerDocumentoAdjunto;
        console.log('objProyecto', objProyecto);

        vm.listaDocumentos;
        vm.gridOptions;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.gridOptions = {
            columnDefs: [{
                field: 'DocumentName',
                displayName: 'Descripción del documento',
                enableHiding: false,
                width: '39%'
            },
            {
                field: 'Created',
                displayName: 'Fecha',
                cellFilter: 'date:\'dd/MM/yyyy hh:mm\'',
                enableHiding: false,
                width: '20%',
                sort: {
                    direction: 'desc',
                    priority: 0
                }
            },
            {
                field: 'Extension',
                displayName: 'Extensión',

                enableHiding: false,
                width: '15%'
            },
            {
                field: 'Category',
                displayName: 'Categoría',

                enableHiding: false,
                width: '20%'
            },
            {
                field: 'accion',
                displayName: 'Acción',
                headerCellClass: 'text-center',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.obtenerDocumentoAdjunto(row.entity)" tooltip-placement="Auto" uib-tooltip="Descargar">' +
                    '    <span style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionDownload.png"></span>' +
                    '</button></div>',
                width: '5%'
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
            //objProyecto.ProyectoId = 24;
            servicioConsolaProyectos.obtenerDocumentosAdjuntos(objProyecto.ProyectoId)
                .then(response => {
                    vm.gridOptions.data = response.data;
                })
        }

        function obtenerDocumentoAdjunto($row) {
            console.log('$row', $row)
            let proyecto = objProyecto.ProyectoId;
            let nombre = $row.DocumentName

            servicioConsolaProyectos.obtenerDocumentoAdjunto(proyecto.toString(), nombre).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, $row.DocumentName);
            });
        };
    }

    angular.module('backbone').controller('modalDocumentosAdjuntosController', modalDocumentosAdjuntosController);
})();