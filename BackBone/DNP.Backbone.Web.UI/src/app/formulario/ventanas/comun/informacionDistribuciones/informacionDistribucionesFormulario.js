(function () {
    'use strict';

    informacionDistribucionesFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'constantesBackbone',
        'archivoServicios',
        'tramiteTrasladoOrdinarioServicio',
        'comunesServicio'
    ];

    function informacionDistribucionesFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        constantesBackbone,
        archivoServicios,
        tramiteTrasladoOrdinarioServicio,
        comunesServicio
    ) {
        var vm = this;
        vm.nombreComponente = "informaciontramitedistribuciones";
        vm.lang = "es";
        vm.IdNivel = $sessionStorage.idNivel;
        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.camposActivos = 0;
        vm.dataTemporal = [];
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;


        //vm.listaProyectos = {};
        vm.error = {};


        function consultarFuentes() {
            vm.listaProyectos = [];
            debugger;
            comunesServicio.obtenerTramitesDistribucionAnteriores($sessionStorage.idInstancia)
                .then(function (rta) {
                    if (rta.data !== "null") {
                        vm.listaProyectos.push(jQuery.parseJSON(jQuery.parseJSON(rta.data)));
                        console.log(vm.listaProyectos);
                    }
                }).catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la información de resumen de trámites de distribucion");
                });
        }

        vm.init = function () {
            consultarFuentes();
            //console.log(vm.listaProyectos.length);
            //debugger;
        };

        vm.descargarDocumentoSoporte = function (InstanciaTramiteId) {
            debugger;
            let modelo = {
                coleccion: "tramites",
                section: "asociarproyecto"
            };
            let param = {
                idInstancia: InstanciaTramiteId.toLowerCase(),
                section: modelo.section,
                idAccion: $sessionStorage.idAccion,
                idNivel: $sessionStorage.idNivel
            };

            archivoServicios.obtenerListadoArchivos(param, modelo.coleccion).then(function (response) {
                let docTipoCertificaRecurso = response.filter((item) => {
                    return item.metadatos.tipodocumentoid === 65
                });
                if (docTipoCertificaRecurso) {
                    archivoServicios.obtenerArchivoBytes(docTipoCertificaRecurso[0].metadatos.idarchivoblob, modelo.coleccion).then(function (retorno) {
                        const blob = utilidades.base64toBlob(retorno, docTipoCertificaRecurso[0].metadatos.contenttype);
                        const downloadUrl = URL.createObjectURL(blob);
                        const a = document.createElement("a");
                        a.href = downloadUrl;
                        a.download = docTipoCertificaRecurso[0].nombre;
                        document.body.appendChild(a);
                        a.click();
                    }, function (error) {
                        utilidades.mensajeError("Error inesperado al descargar");
                    });
                }

            });
        };

        vm.descargarConcepto = function (tramiteId) {
            tramiteTrasladoOrdinarioServicio.generarPdfCartaTramite(tramiteId, true, "ConceptoTraslDistribucion");
        };
    }

    angular.module('backbone').component('informacionDistribucionesFormulario', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/comun/informacionDistribuciones/informacionDistribucionesFormulario.html",
        controller: informacionDistribucionesFormulario,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            objetonegocioid: '@',
            nombrecomponentepaso: '@',
            actualizacomponentes: '@'

        }
    });

})();

