(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteModificacionLeyPoliticasServicio', tramiteModificacionLeyPoliticasServicio);

    tramiteModificacionLeyPoliticasServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteModificacionLeyPoliticasServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            generarPdfCartaTramite: generarPdfCartaTramite
        };

        function generarPdfCartaTramite(tramiteId, borrador = true, nombrearchivo) {
            let nombreFichaTramite = '';

            nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaAdicionDonacion;// Adicion por Donacion

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
    }

})();