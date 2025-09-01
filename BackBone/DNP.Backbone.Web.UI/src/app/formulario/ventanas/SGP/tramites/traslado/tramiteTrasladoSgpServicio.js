(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteTrasladoSgpServicio', tramiteTrasladoSgpServicio);

    tramiteTrasladoSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteTrasladoSgpServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            generarPdfCartaTramite: generarPdfCartaTramite,
            validacionProyectosTramiteNegocio: validacionProyectosTramiteNegocio
        };

        function generarPdfCartaTramite(tramiteId, borrador = true, nombrearchivo) {
            let nombreFichaTramite = '';



            nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCarta;// Traslado Sgp



            //Obtiene el identificador único del reporte
            servicioFichasProyectos.ObtenerIdFicha(nombreFichaTramite)
                .then(function (respuestaFicha) {
                    var fichaPlantilla = {
                        NombreReporte: nombreFichaTramite,
                        TramiteId: tramiteId,
                        IdReporte: respuestaFicha.ID
                    };



                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla))
                        .then(function (respuesta) {
                            if (borrador) {
                                const nombreArchivo = nombrearchivo.replace(/ /gi, "_") + '_' + tramiteId + '_' + moment().format("YYYYMMDDD_HHMMSS") + "pdf";
                                //const blob = new Blob([respuesta], { type: 'application/pdf' });
                                const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                                const file = new File([blob], nombreArchivo, { type: 'application/pdf' });
                                FileSaver.saveAs(file, nombreArchivo);
                            }
                        }, function (error) {
                            utilidades.mensajeError(error);
                        });
                }, function (error) {
                    utilidades.mensajeError(error);
                });
        }

        function validacionProyectosTramiteNegocio(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneValidacionProyectosTramiteNegocio + "?tramiteId=" + tramiteId;
            return $http.get(url);
        }
    }
})();