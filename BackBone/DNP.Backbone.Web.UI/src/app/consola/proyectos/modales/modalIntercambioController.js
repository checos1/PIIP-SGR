var modalIntercambioCtrl = null;

try {
    (function() {

        let module = angular.module('backbone');

        /**
         * 
         * @description Controlador de la ventana modal de intermcambio de proyectos entre entidades
         * @param {object} $scope . $scope actual del componente HTML
         * @param {object} $uibModalInstance . Servicio provider de las modales de ui-bootstrap
         * @param {object} data . Información enviada desde el controlador principal. Datos del proyecto actual y el historial de cambios entre entidades
         */
        let modalIntercambioController = function($scope, $uibModalInstance, data){
            
            modalIntercambioCtrl = this;

            modalIntercambioCtrl.$parentScope = ($scope.$parent) ? $scope.$parent.vm : {};

            //#endregion ng-model 
            modalIntercambioCtrl.modelo = {

                /*inicializar con el parámetro*/
                proyectoEntidadHistorialLista : angular.copy(data.proyectoEntidadHistorial),
                proyecto: angular.copy(data.proyecto),

                proyectoNombre: '', 

                // datos seleccionados
                entidadSeleccionada: null,

                // objeto de registro a insertar. AuditoriaEntidad
                auditoriaEntidad: {
                    EntidadOrigenId : Number(0),
                    EntidadOrigen: String(''),
                    EntidadDestinoId : Number(0),
                    EntidadDestino: String(''),
                    ProyectoId: Number(0),
                    SectorId : Number(0)
                }

            };
            //#endregion ng-model

            modalIntercambioCtrl.catalogos = {
                listaEntidades : []
            };

            //#region GRID
            modalIntercambioCtrl.grid = {

                intercambioGridApi : {},

                intercambioOptions : 

                     {
                        enableSorting: true,

                        // columnas
                        columnDefs : [
                            {
                                field: 'EntidadOrigen',
                                displayName: 'Entidad Origen',
                                enableHiding: false,
                                width       : '30%',
                            },
                            {
                                field: 'EntidadDestino',
                                displayName: 'Entidad Destino',
                                enableHiding: false,
                                width       : '30%',
                            },

                            {
                                field: 'Usuario',
                                displayName: 'Usuario',
                                enableHiding: false,
                                width       : '20%',
                            },
                            {
                                field: 'FechaMovimiento',
                                displayName: 'Fecha',
                                type: 'date',
                                cellFilter: 'date:\'dd/MM/yyyy HH:mm\'',
                                enableHiding: false,
                                width       : '20%',
                            },
                        ],

                        enableOnDblClickExpand: false,
                        showGridFooter: false,
                        enablePaginationControls: true,
                        useExternalPagination: false,
                        useExternalSorting: false,
                        paginationCurrentPage: 1,
                        enableVerticalScrollbar: 1,
                        enableFiltering: false,
                        useExternalFiltering: false,
                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                        paginationPageSize: 10,
                        
                        // Register API
                        onRegisterApi : (gridApi) => { modalIntercambioCtrl.grid.intercambioGridApi = gridApi; },

                        data: modalIntercambioCtrl.modelo.proyectoEntidadHistorialLista
                }
            };
            //#endregion GRID

            //#region  EVENTOS
            modalIntercambioCtrl.eventos = {

                mondaIntercambio_$onInit : function(){
                    
                    modalIntercambioCtrl.modelo.proyectoNombre    = `${modalIntercambioCtrl.modelo.proyecto.NombreObjetoNegocio} - ${modalIntercambioCtrl.modelo.proyecto.IdObjetoNegocio}`;
                    modalIntercambioCtrl.$parentScope.obtenerEntidadesNegocio(modalIntercambioCtrl.$parentScope.tipoEntidad).then(response => {

                        modalIntercambioCtrl.catalogos.listaEntidades = response.data;
                    });
                },

                 /**
                 * 
                 * @description Provocado al presionar el obtón de guardado de intercambio de entidades
                 * @param {Event} $event . Evento provocado por componente HTML origen
                 * @param {Object} sender . Componente HTML origen
                 */
                btnGuardarIntercambio_onClick: function($event, sender){
                    try {
                        if(modalIntercambioCtrl.modelo.entidadSeleccionada === null)
                        {
                            toastr.warning('Debe seleccionar al menos un elemento');
                            return;
                        }  

                        
                        modalIntercambioCtrl.modelo.auditoriaEntidad.EntidadOrigenId  = Number(modalIntercambioCtrl.modelo.proyecto.IdEntidad);
                        modalIntercambioCtrl.modelo.auditoriaEntidad.EntidadOrigen    = String(modalIntercambioCtrl.modelo.proyecto.NombreEntidad);

                        modalIntercambioCtrl.$parentScope.obtenerDatosGeneralesEntidad(modalIntercambioCtrl.modelo.entidadSeleccionada.IdEntidad).then(response => {
                            var entidad = response.data;

                            if (entidad !== null && entidad !== undefined) {

                                if (entidad.EntityTypeCatalogOptionId == null) {
                                    toastr.warning('Id entidad (opción) es nulo. Favor de seleccionar otra entidad.');
                                    return;
                                }

                                modalIntercambioCtrl.modelo.auditoriaEntidad.EntidadDestinoId = Number(entidad.EntityTypeCatalogOptionId);
                                modalIntercambioCtrl.modelo.auditoriaEntidad.EntidadDestino   = String(entidad.Entidad);
                                modalIntercambioCtrl.modelo.auditoriaEntidad.SectorId         = Number(entidad.SectorNegocioId);

                                modalIntercambioCtrl.modelo.auditoriaEntidad.ProyectoId = Number(modalIntercambioCtrl.modelo.proyecto.ProyectoId);

                                $uibModalInstance.close(/*Información*/modalIntercambioCtrl.modelo.auditoriaEntidad);
                            }
                        });
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl.btnGuardarIntercambio_onClick: ', exception);
                        toastr.error('Ocurrió un error al cerrar la ventana modal');
                    }
                },

                 /**
                 * 
                 * @description Evento provocado al presionar el botón cancelar de la ventana modal de intercambio de entidades
                 * @param {Event} $event . Evento provocado por componente HTML origen
                 * @param {Object} sender . Componente HTML origen
                 */
                btnCancelarIntercambio_onClick: function($event, sender){
                    try {
                        $uibModalInstance.dismiss('cancel');
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl.btnCancelarIntercambio_onClick: ', exception);
                        toastr.error('Ocurrió un error al cerrar la ventana modal');
                    }
                }
            };
            // //#endregion EVENTOS
        };

        modalIntercambioController.$inject = [
            '$scope', '$uibModalInstance', 'data'
        ];

        module.controller('modalIntercambioController', modalIntercambioController);
    })();
}
catch(exception){
    console.log('intercambioProyectoEntidadCtrl');
}