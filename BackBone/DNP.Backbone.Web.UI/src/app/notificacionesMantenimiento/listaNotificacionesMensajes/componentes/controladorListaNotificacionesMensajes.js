(function () {
    'use strict';
    angular.module('backbone').controller('controladorListaNotificacionesMensajes', controladorListaNotificacionesMensajes);

    controladorListaNotificacionesMensajes.$inject = [
        '$scope',        
        'servicioNotificacionesMantenimiento',
        'servicioNotificacionesMensajes',
        'FileSaver'
    ];

    function controladorListaNotificacionesMensajes(
        $scope,        
        servicioNotificacionesMantenimiento,
        servicioNotificacionesMensajes,
        FileSaver
    ) {
        var vm = this;

        const LEIDA = "Leída";
        const NO_LEIDA = "No Leída";
        //Métodos
        vm.mostrarOcultarFiltro = mostrarOcultarFiltro;
        vm.exportarPdf = exportarPdf;
        vm.btnExcel_OnClick = btnExcel_OnClick;
        vm.exportarXml = exportarXml;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.buscar = buscar;
        vm.mostrarModalDetalle = mostrarModalDetalle;

        //variables
        vm.lang = "es";
        vm.mostrarFiltro = false;
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.BusquedaRealizada = false;
        //Plantillas para personalizar el contenido de las celdas
        vm.columnaNotificacion = 'src/app/notificacionesMantenimiento/listaNotificacionesMensajes/componentes/plantillas/notificacionColumna.html';

        vm.columnDef = [{
            field: 'estado',
            displayName: 'Estado',
            enableHiding: false,
            width: '10%',
        }, {
            field: 'notificacion',
            displayName: 'Notificaciones',
            enableHiding: false,
            cellTemplate: vm.columnaNotificacion,
            width: '70%',
        },
        {
            field: 'fecha',
            displayName: 'Fecha',
            enableHiding: false ,
            type: 'date', 
            cellFilter: 'date:\'dd-MM-yyyy\'',
            
            width: '20%'
        }];

        vm.listaNotificaciones = [];
        
        vm.filtro = {
            estado: null,
            notificacion: null,
            fecha: null
        };

        // grid main
        vm.gridOptions;

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
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

                cargarNotificaciones();
            }
        };

        function cargarNotificaciones(restablecer) {
            let options = { year: 'numeric', month: 'short' };
            vm.buscando = true;
            servicioNotificacionesMensajes.obtenerListaMensajesNotificaciones({
                Fecha: vm.filtro.fecha,
                Notificacion: vm.filtro.notificacion,
                UsuarioYaLeyo: vm.filtro.estado == LEIDA ? true : vm.filtro.estado == NO_LEIDA ? false : null
            })
                .then(result => {
                    vm.listaNotificaciones = result.data.map(t => ({
                        id: t.Id,
                        estado: t.UsuarioYaLeyo ? LEIDA : NO_LEIDA,
                        nombreNotificacion: t.UsuarioConfigNotificacion.NombreNotificacion,
                        notificacion: t.UsuarioConfigNotificacion.ContenidoNotificacion,
                        notificacionCortada: cortarNotificacion(t.UsuarioConfigNotificacion.ContenidoNotificacion, 75),
                        fecha: t.UsuarioConfigNotificacion.FechaInicio,
                        fechaString: new Date(t.UsuarioConfigNotificacion.FechaInicio).toLocaleDateString('es-ES', options),
                        nombreTipo: t.UsuarioConfigNotificacion.NombreTipo,
                        notifiacionTipo: t.UsuarioConfigNotificacion.Tipo,
                        visible: t.Visible,
                        idUsuarioDNP: t.IdUsuarioDNP,
                        idArchivo: t.UsuarioConfigNotificacion.IdArchivo
                    }))
                    vm.gridOptions.columnDefs = vm.columnDef;
                    vm.gridOptions.data = vm.listaNotificaciones;
                    vm.hayRegistros = true;
                    vm.buscando = false;
                    if (restablecer)
                        vm.BusquedaRealizada = false;
                    else
                        vm.BusquedaRealizada = true;
                }, error => {
                    toastr.error("Error al cargar las notificaciones del usuario");
                    vm.gridOptions.columnDefs = vm.columnDef;
                    vm.gridOptions.data = [];
                    vm.hayRegistros = false;
                    vm.buscando = false;
                });
        }

        /**
         * 
         * @description . Realiza la petición HTTP para descargar el archivo binario del excel con los datos proporcionados 
         * @param {Array} datos. Arreglo de datos con la estructura de la clase UsuarioNotificacionDto 
         */
        function descargarExcel(datos){
            try {

                if(typeof datos !== 'object')
                    throw { message: 'El origen de datos no contiene la estructura correcta' };
                
                if(datos.length === 0)
                    throw { message: 'El arreglo no puede estar vacío' };

                    servicioNotificacionesMensajes.ObtenerExcel(datos).then(excelResponse => {

                        let excelDataResponse = excelResponse.data;

                        if( Boolean(excelDataResponse.EsExcepcion) === true ){

                            console.log('ListaNotificaciones.descargarExcel => ', String(excelDataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar Excel');
                        }
                        else{

                            let file = excelDataResponse.Datos;

                            var bytes = Uint8Array.from( atob( String(file.FileContent)).split('').map(char => char.charCodeAt(0)) );
                            var blob  = new Blob([bytes], { type: String(file.ContentType) });

                            FileSaver.saveAs(blob, String(file.FileName));
                        }
                    });
            }
            catch(exception){
                throw { message:  `descargarExcel : ${exception.message}` };
            }
        }

        /**
         * 
         * @description. Provocado al presionar el obtón de descarga de Excel
         * @param {Event} event . Evento provocado 
         * @param {HTMLElement} sender. Componente HTML que provoca el evento 
         */
        function btnExcel_OnClick(event, sender) {
            try {

                servicioNotificacionesMensajes.obtenerListaMensajesNotificaciones({
                    
                    Fecha: vm.filtro.fecha,
                    Notificacion: vm.filtro.notificacion,
                    UsuarioYaLeyo: vm.filtro.estado == LEIDA ? true : vm.filtro.estado == NO_LEIDA ? false : null
                })
                .then(response => {

                    let dataResponse = response.data;

                    if(dataResponse.length > 0)
                        descargarExcel(dataResponse);
                });
            }
            catch(exception){
                console.log('ListaNotificacionesCtrl.btnExcel_OnClick => ', exception);
                toastr.error('Error en la descarga de Excel.');
            }
        }
        

        function cortarNotificacion(texto, longitud) {
            if (texto) {
                if (longitud < texto.length)
                    return `${texto.substring(0, longitud)}...`;
                return texto;
            }
            return null;
        }

        function mostrarOcultarFiltro() {
            vm.mostrarFiltro = !vm.mostrarFiltro;
            var idSpanArrow = 'arrow-IdPanelBuscador';
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
            }
        }

        function exportarPdf() {
            servicioNotificacionesMensajes.obtenerPdfNotificacionesMensajes(vm.gridOptions.data).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
          
                FileSaver.saveAs(blob, nombreDelArchivo(retorno));
            });
            
        }

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
        function exportarXls() {
            alert('No disponible');
        }

        function exportarXml() {
            alert('No disponible');
        }

        function limpiarCamposFiltro() {
            vm.filtro = {
                estado: null,
                notificacion: null,
                fecha: null
            }
            cargarNotificaciones(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }

        function buscar() {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            cargarNotificaciones(false);
        }

        function mostrarModalDetalle(model) {
            servicioNotificacionesMantenimiento.visualizarContenidoNotificacion({
                NombreNotificacion: model.nombreNotificacion,
                NombreTipo: model.nombreTipo,
                Tipo: model.notifiacionTipo,
                ContenidoNotificacion: model.notificacion,
                IdArchivo: model.idArchivo
            });
            if (model.estado == NO_LEIDA)
                marcarComoLeida(model);
        }

        function marcarComoLeida(model) {
            servicioNotificacionesMensajes.marcarNotificacionComoLeida({
                UsuarioNotificacionIds: [model.id],
                IdUsuarioDNP: model.idUsuarioDNP,
                Visible: model.visible,
                NotificacionesLeida: model.estado == LEIDA ? true : model.estado == NO_LEIDA ? false : null
            }).then((result) => {
                model.estado = LEIDA
                vm.gridOptions.data = vm.listaNotificaciones;
            });
        }
    }

    angular.module('backbone')
        .component('listaNotificacionesMensajes', {
            templateUrl: 'src/app/notificacionesMantenimiento/listaNotificacionesMensajes/componentes/listaNotificacionesMensajes.template.html',
            controller: 'controladorListaNotificacionesMensajes',
            controllerAs: 'vm'
        });
})();