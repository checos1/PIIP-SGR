(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteProgramacionRecursosServicio', tramiteProgramacionRecursosServicio);

    tramiteProgramacionRecursosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteProgramacionRecursosServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            generarPdfCartaTramite: generarPdfCartaTramite,
            obtenerResumenReprogramacionPorProductoVigencia: obtenerResumenReprogramacionPorProductoVigencia,
            ObtenerAutorizacionesParaReprogramacion: ObtenerAutorizacionesParaReprogramacion,
            AsociarAutorizacionRVF: AsociarAutorizacionRVF,
            ObtenerAutorizacionAsociada: ObtenerAutorizacionAsociada,
            EliminaReprogramacionVF: EliminaReprogramacionVF,
            obtenerErroresProgramacion: obtenerErroresProgramacion,
            ObtenerProgramacionBuscarProyecto: ObtenerProgramacionBuscarProyecto,
            BorrarTramiteProyecto: BorrarTramiteProyecto
        };

        function generarPdfCartaTramite(tramiteId, borrador = true, nombrearchivo) {
            let nombreFichaTramite = '';

            nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaReprogramacionVF;// Reprogramación VF

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

        function obtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarResumenReprogramacionPorProductoVigencia + "?InstanciaId=" + instanciaId + "&TramiteId=" + tramiteId + "&ProyectoId=" + proyectoId;
            return $http.get(url);
        }

        function ObtenerAutorizacionesParaReprogramacion(bpin, tramiteid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAutorizacionesParaReprogramacion;
            url += "?bpin=" + bpin;
            url += "&tramiteid=" + tramiteid;
            url += "&tipoTramite=VFO";
            return $http.get(url);
        }

        function AsociarAutorizacionRVF(asociarAutorizacionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAsociarAutorizacionRVF;
            return $http.post(url, asociarAutorizacionDto);
        }

        function ObtenerAutorizacionAsociada(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAutorizacionAsociada;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function EliminaReprogramacionVF(reprogramacionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminaReprogramacionVF;
            return $http.post(url, reprogramacionDto);
        }

        function obtenerErroresProgramacion(idInstancia, accionid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProgramacion;
            url += "?IdInstancia=" + idInstancia;
            url += "&accionid=" + accionid;
            return $http.get(url);
        }

        function ObtenerProgramacionBuscarProyecto(EntidadDestinoId, tramiteid, bpin, NombreProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerProgramacionBuscarProyecto;
            url += "?EntidadDestinoId=" + EntidadDestinoId;
            url += "&tramiteid=" + tramiteid;
            url += "&bpin=" + bpin
            url += "&NombreProyecto=" + NombreProyecto;
            return $http.get(url);
        }

        function BorrarTramiteProyecto(programacionDistribucionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneBorrarTramiteProyecto;
            return $http.post(url, programacionDistribucionDto);
        }
    }

})();