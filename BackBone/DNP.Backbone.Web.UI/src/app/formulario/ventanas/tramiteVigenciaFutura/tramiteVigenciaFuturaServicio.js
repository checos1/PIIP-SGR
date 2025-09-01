(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteVigenciaFuturaServicio', tramiteVigenciaFuturaServicio);

    tramiteVigenciaFuturaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteVigenciaFuturaServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            actualizarTramitesRequisitos: actualizarTramitesRequisitos,
            obtenerProyectoRequisitosPorTramite: obtenerProyectoRequisitosPorTramite,
            obtenerInstanciasPermiso: obtenerInstanciasPermiso,
            obtenerErroresTramite: obtenerErroresTramite,

            obtenerDetalleTramitePorInstancia: obtenerDetalleTramitePorInstancia,
            validarVigenciaConpes: validarVigenciaConpes,
            eliminarAsociacion: eliminarAsociacion,
            ObtenerProyectoAsociacion: ObtenerProyectoAsociacion,
            asociarProyecto: asociarProyecto,
            obtenerProyectos: obtenerProyectos,

            obtenerDatosProyectoTramite: obtenerDatosProyectoTramite,
            obtenerDatosCronograma: obtenerDatosCronograma,
            ObtenerInformacionPresupuestalValores: ObtenerInformacionPresupuestalValores,
            obtenerEstadoActualAccion: obtenerEstadoActualAccion,
            generarPdfCartaTramiteVf: generarPdfCartaTramiteVf,
            GuardarInformacionPresupuestalValores: GuardarInformacionPresupuestalValores,
            obtenerInstanciaProyectoTramite: obtenerInstanciaProyectoTramite,
            ValidacionPeriodoPresidencial: ValidacionPeriodoPresidencial,
            obtenerDatosProyectosPorTramite: obtenerDatosProyectosPorTramite,
        };

        function actualizarTramitesRequisitos(TramiteRequisitosDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesRequisitos;
            return $http.post(url, TramiteRequisitosDto);
        }

        function obtenerProyectoRequisitosPorTramite(ProyectoId, TramiteId, IsCDP) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectoRequisitosPorTramite}` + '?pProyectoId=' + ProyectoId + '&pTramiteId=' + TramiteId + '&isCDP=' + IsCDP;
            return $http.get(url);
        }

        function obtenerInstanciasPermiso(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInstanciasPermiso;
            return $http.post(url, parametros);

        }

        function obtenerErroresTramite(guiMacroproceso, idInstancia, accionid, tieneCDP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresTramite;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idInstancia=" + idInstancia;
            url += "&accionid=" + accionid;
            if (tieneCDP === undefined) {
                url += "&tieneCDP=false"
            }
            else {
                url += "&tieneCDP=" + tieneCDP;
            }
            return $http.get(url);
        }
        function obtenerDetalleTramitePorInstancia(instanciaId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerDetalleTramitePorInstancia}/${instanciaId}`;
            return $http.get(url);
        }

        function validarVigenciaConpes(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneValidarConpesTramite}/${tramiteId}`;
            return $http.get(url);
        }

        function eliminarAsociacion(EliminarAsociacionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarAsociacionVFOPLN;
            return $http.post(url, EliminarAsociacionDto);
        }

        function ObtenerProyectoAsociacion(bpin, tramiteid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoAsociacionVFO;
            url += "?bpin=" + bpin;
            url += "&tramiteid=" + tramiteid;
            url += "&tipoTramite=VFO";
            return $http.get(url);
        }

        function asociarProyecto(asociarProyectoDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAsociarProyectoVFOPLN;
            return $http.post(url, asociarProyectoDto);
        }

        function obtenerProyectos(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBpin + "?bpin=" + bpin;
            return $http.post(url);
        }

        function obtenerDatosProyectoTramite(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProyectoTramite;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function obtenerDatosCronograma(instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosCronograma;
            url += "?instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function ObtenerInformacionPresupuestalValores(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerValores;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function obtenerEstadoActualAccion(tramiteId, bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerAccionActualyFinal;
            url += "?tramiteId=" + tramiteId + '&bpin=' + bpin;
            return $http.get(url);
        }

        function generarPdfCartaTramiteVf(tramiteId, borrador = true, tipoConcepto = "VF") {
            let nombreFichaTramite = '';

            switch (tipoConcepto) {
                case "VF":
                    nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaVf;
                    break;
                case "AL":
                    nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaAL;
                    break;
                case "VFE":
                    nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaVFExcep;
                    break;
                default:
                    nombreFichaTramite = constantesBackbone.apiBackBoneNombrePDFCartaVf;
                    break;
            }

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
                                const nombreArchivo = nombreFichaTramite.replace(/ /gi, "_") + '_' + tramiteId + '_' + moment().format("YYYYMMDDD_HHMMSS") + "pdf";
                                /*const blob = new Blob([respuesta], { type: 'application/pdf' });*/
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

        function GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarInformacionPresupuestalValores;
            return $http.post(url, informacionPresupuestalValoresDto);
        }

        function obtenerInstanciaProyectoTramite(InstanciaId, BPIN) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInstanciaProyectoTramite;
            url += "?InstanciaId=" + InstanciaId + "&BPIN=" + BPIN;
            return $http.get(url);


        }
        function ValidacionPeriodoPresidencial(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneValidacionPeriodoPresidencial;
            url += "?tramiteid=" + tramiteId ;
            return $http.get(url);
        }

        function obtenerDatosProyectosPorTramite(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProyectosPorTramite;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

    }

})();
