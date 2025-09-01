(function () {
    'use strict';

    justificacionvfDetalleController.$inject = ['requerimientosTramitesServicio',
        '$sessionStorage', 'utilidades',
        '$scope', 'justificacionCambiosServicio', 'trasladosServicio', 'archivoServicios',
        'constantesBackbone', 'tramiteVigenciaFuturaServicio',];

    function justificacionvfDetalleController(

        requerimientosTramitesServicio,
        $sessionStorage,
        utilidades,
        $scope,
        justificacionCambiosServicio,
        trasladosServicio,
        archivoServicios,
        constantesBackbone,
        tramiteVigenciaFuturaServicio,
    ) {
        var vm = this;
        //vm.Justificaciones = [{
        //    TramiteId: 0,
        //    ProyectoId: 0,
        //    JustificacionId: 0,
        //    JustificacionPregunta: "",
        //    JustificacionRespuesta: null,
        //    TipoJustificacion: "",
        //    TipoRolId: 0
        //}];

        //vm.JustificacionPaso = [{
        //    paso: "",
        //    NombreUsuario: "",
        //    justificaciones: [{
        //        TramiteId: 0,
        //        ProyectoId: 0,
        //        JustificacionId: 0,
        //        JustificacionPreguntaId: 0,
        //        OrdenJustificacionPregunta: 0,
        //        JustificacionPregunta: "",
        //        JustificacionRespuesta: null,
        //        ObservacionPregunta: "",
        //        OpcionesRespuesta: "",
        //        ObservacionRespuesta: "",
        //        Tematica: "",
        //        OrdenTematica: 0,
        //        NombreRol: "",
        //        NombreNivel: "",
        //        CuestionarioId: 0,
        //        TipoJustificacion: "",
        //        TipoRolId: 0,
        //        Usuario: "",
        //        FechaEnvio: null,
        //        Paso: "",
        //        NombreUsuario: "",
        //        Cuenta: "",
        //    }]
        //}];

        vm.Preguntas = [];

        vm.IdNivel = $sessionStorage.idNivel;
        vm.mostrar = false;

        vm.disabled = true;
        vm.cantidadPreguntas = 0;
        vm.nombreComponente = "justificacionjustificacion";
        vm.descargarArchivoBlob = descargarArchivoBlob;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;

        vm.etapa = "ej";
        vm.seccionCapitulo = null;

        vm.listaArchivosAsociados = [];
        vm.modelo = {
            coleccion: "proyectos", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: $sessionStorage.TipoTramiteId, allArchivos: $sessionStorage.allArchivosTramite,
            BPIN: $sessionStorage.BPIN
        }

        vm.obtener = function () {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {

                    IniciarDiferentedePresupuesto();
                    obtenerPreguntas(vm.tramiteid, vm.tipotramiteid, 0);
                }
            });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

        }

        function IniciarDiferentedePresupuesto() {

            if ($sessionStorage.idNivel.toUpperCase() !== constantesBackbone.idNivelSeleccionProyectos && $sessionStorage.idNivel.toUpperCase() !== constantesBackbone.idNivelAprobacionEntidad) {
                vm.mostrar = true;
                BuscarProyectoID();
                initConsultaArchivosProyecto();
            }

        }

        vm.ActivarEditar = function () {
            if (vm.disabled == true) {
                $("#Editar").html("CANCELAR");
                vm.disabled = false;

            }
            else {
                $("#Editar").html("EDITAR");
                vm.Justificaciones = angular.copy(vm.Preguntas);
                vm.disabled = true;

            }
        }

        vm.registrorespuesta = function (response) {
            let respuestas = {};
            if (response !== undefined) {
                respuestas = response.JustificacionRespuesta;
            }


            for (const element of vm.Justificaciones) {

                for (const [key, value] of Object.entries(respuestas)) {
                    if (element.JustificacionPreguntaId == key) {
                        element.JustificacionRespuesta = value;

                    }

                }
                element.NivelId = vm.IdNivel;
                element.InstanciaId = $sessionStorage.idInstanciaIframe;
                element.ProyectoId = null;

            }

            return requerimientosTramitesServicio.guardarRespuestasJustificacion(vm.Justificaciones).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (vm.Justificaciones.filter(p => p.JustificacionRespuesta !== null && p.JustificacionRespuesta !== "").length > 0) {
                            guardarCapituloModificado();
                        }
                        else {
                            eliminarCapitulosModificados();
                        }



                        if (response.data.Exito) {
                            $("#Editar").html("EDITAR");
                            vm.disabled = true;
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }

            );


        }


        function obtenerPreguntas(idTramite, tipoTramiteId, tipoRolId) {
            vm.IdNivel = constantesBackbone.idNivelSeleccionProyectos;
            return requerimientosTramitesServicio.obtenerPreguntasJustificacion(idTramite, 0, tipoTramiteId, tipoRolId, vm.IdNivel).then(
                function (respuesta) {
                    vm.Preguntas = respuesta.data;
                    vm.Justificaciones = respuesta.data;
                    if (vm.Justificaciones[0].NombreUsuario !== null) {
                        let fecha = new Date(vm.Justificaciones[0].FechaEnvio).toLocaleDateString();
                        vm.Usuario = vm.Justificaciones[0].NombreUsuario + " " + vm.Justificaciones[0].Cuenta + ".  Fecha  de envío: " + fecha;
                    }
                });

        }

        function BuscarProyectoID() {
            tramiteVigenciaFuturaServicio.obtenerDatosProyectoTramite(vm.tramiteid).then(
                function (respuesta) {
                    if (respuesta.data.ProyectoId != 0) {
                        vm.proyectoId = respuesta.data.ProyectoId;
                        ObtenerPreguntasNopresuoyesto();
                    }
                });
        }

        function ObtenerPreguntasNopresuoyesto() {
            vm.IdNivel = constantesBackbone.idNivelAprobacionEntidad;
            var TipoProyecto = $sessionStorage.TipoProyecto;
            vm.tipoJustificacion = "Justificacion del " + TipoProyecto;
            return requerimientosTramitesServicio.ObtenerPreguntasProyectoActualizacionPaso(vm.tramiteid, vm.proyectoId, vm.tipotramiteid, vm.IdNivel, 0).then(
                function (respuesta) {
                    vm.JustificacionPaso = respuesta.data.filter(c => c.Paso == 'Viabilidad sectorial' || c.Paso == 'Viabilidad definitiva');
                });

        }

        function initConsultaArchivosProyecto() {

            //trasladosServicio.obtenerProyectosPorTramite(vm.modelo.idInstancia).
            //    then(function (response) {
            //        if (response.data != null) {
            //            vm.BPIN = response.data.ObjetoNegocioId;
            //            cargarArchivos();
            //        }
            //    });
            tramiteVigenciaFuturaServicio.obtenerInstanciaProyectoTramite($sessionStorage.idInstancia, "0").
                then(function (response) {
                    if (response.data != null) {
                        vm.BPIN = response.data[0].ObjetoNegocioId
                        vm.InstanciaProyectoId = response.data[0].InstanciaProyecto;
                        cargarArchivos();
                    }
                });
        }

        function cargarArchivos() {

            vm.listaArchivosAsociados = [];
            vm.totalRegistrosconsulta = 0;

            let param = {
                //bpin: $sessionStorage.BPIN,
                idinstancia: vm.InstanciaProyectoId,
                //idNivel: "4212ce81-4752-4bfd-8eeb-0110ea97348f"
                //extension: ".pdf",
                //bpin: vm.BPIN,
                // idInstancia: "d8a872d1-5b7f-48c6-957f-ca087f140756",
                //descripcionTramite: "VIGENCIAS FUTURAS",
                //tipoDocumentoSoporte: "Concepto Favorable Vigencia Futura Cabeza de Sector",
                //codigoProceso: $sessionStorage.idObjetoNegocio
                //obligatorio: "SI",
                //"nivel": "CONTROL POSTERIOR AJUSTE",
                "idNivel": constantesBackbone.idNivelViabilidadDefinitiva
            };

            archivoServicios.obtenerListadoArchivos(param, "proyectos").then(function (response2) {
                if (response2 === undefined || typeof response2 === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response2);
                } else {
                    response2.forEach(archivo => {
                        if (archivo.status != 'Eliminado') {
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


                    });

                }
            }, error => {
                console.log(error);
            });


        }

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

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-justificacionjustificacion');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacion(vm.modelo.descripcionAccion, null, vm.modelo.nombreAccion);

        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            if (vm.Justificaciones !== undefined)
                vm.Justificaciones.forEach(p => {
                    var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-" + p.JustificacionPreguntaId);
                    if (campoObligatorioProyectos != undefined) {
                        campoObligatorioProyectos.innerHTML = "";
                        campoObligatorioProyectos.classList.add('hidden');
                    }
                }
                );
        }

        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (erroresJson != undefined) {
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }



        vm.validarValoresVigenciaInformacionPresupuestalJustificacion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.validarValoresVigenciaInformacionPresupuestalPregunta = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO005': vm.validarValoresVigenciaInformacionPresupuestalJustificacion,
            'VFO005-': vm.validarValoresVigenciaInformacionPresupuestalPregunta,




        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }



    angular.module('backbone').component('justificacionvfDetalle', {

        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/justificacionvf/justificacionvfDetalle/justificacionvfDetalle.html",
        controller: justificacionvfDetalleController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            tipotramiteid: '@',
        },

    });

})();