(function () {
    'use strict';

    archivoProcesoActualController.$inject = [
        '$scope',
        'tramiteVigenciaFuturaServicio',
        '$sessionStorage',
        'utilidades',
        'archivoServicios',
        'trasladosServicio',
    ];

    function archivoProcesoActualController(
        $scope,
        tramiteVigenciaFuturaServicio,
        $sessionStorage,
        utilidades,
        archivoServicios,
        trasladosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.descargarArchivoBlob = descargarArchivoBlob;

        vm.totalRegistrosconsulta = 0;
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;


        //variables locales
        vm.listaArchivosAsociados = [];
        vm.tipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.modelo = {
            coleccion: "proyectos", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: $sessionStorage.TipoTramiteId, allArchivos: $sessionStorage.allArchivosTramite,
            BPIN: $sessionStorage.BPIN
        }
        //region declarar metodos
        vm.initConsultaArchivos = initConsultaArchivos;
        vm.abrirpanel = abrirpanel;
        vm.abrirTooltip = abrirTooltip;


        function initConsultaArchivos() {
            initConsultaArchivosProyecto();

        }

        vm.rotated = false;
        function abrirpanel() {

            var acc = document.getElementById('divconsultaarchivo');
            var i;
            var rotated = false;


            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgconsultaarchivo'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;
        }

        function abrirTooltip() {
            utilidades.mensajeInformacion('Esta es la explicación de documentos del proceso actual  asociados al protecto....', false, "Archivos en proceso actual")
        }

        //#region Metodos

        function initConsultaArchivosProyecto() {

            trasladosServicio.obtenerDetallesTramite(vm.numeroTramite).then(function (result) {
                var x = result.data;
                if (x != null && $sessionStorage.tramiteId !== undefined && $sessionStorage.tramiteId !== null) {
                    vm.TramiteId = x.TramiteId;
                    $sessionStorage.tramiteId = x.TramiteId;
                }
                vm.listaArchivosAsociados = [];
                vm.totalRegistrosconsulta = 0;
                initConsultaArchivosAjuste();
               
            });


        }


        //#region archivos del proyecto
        function initConsultaArchivosAjuste() {
            tramiteVigenciaFuturaServicio.obtenerInstanciaProyectoTramite($sessionStorage.idInstancia, "0").
                then(function (response) {
                    if (response.data != null) {
                        vm.BPIN = response.data[0].ObjetoNegocioId
                        vm.InstanciaProyectoId = response.data[0].InstanciaProyecto;
                        cargarArchivosProyecto();
                    }
                });


           
        }

        function cargarArchivosProyecto() {

            let paramproyecto = {
                idinstancia: vm.InstanciaProyectoId,
            };

            archivoServicios.obtenerListadoArchivos(paramproyecto, "proyectos").then(function (response2) {
                if (response2 === undefined || typeof response2 === 'string') {

                } else {
                    response2.forEach(archivo => {
                        if (archivo.status != 'Eliminado') {
                            if (vm.listaArchivosAsociados.findIndex(x => x.idMongo = archivo.idMongo) < 0) {
                                vm.listaArchivosAsociados.push({
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
                                if (vm.listaArchivosAsociados.length > 0) {
                                    vm.totalRegistrosconsulta++;
                                }
                                else {
                                    vm.totalRegistrosconsulta = 0;
                                }
                            }
                        }


                    });

                }
            }, error => {
                console.log(error);
            });


        }

        //#end region archivos del proyecto

        /*region archivos     */


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



        


    }



    angular.module('backbone').component('archivoProcesoActual', {
        templateUrl: "src/app/formulario/ventanas/comun/archivoProcesoActual/archivoProcesoActual.html",
       
        controller: archivoProcesoActualController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            listaArchivos: '@',
        }
    });

})();