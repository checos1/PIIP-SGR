(function () {
    'use strict';
    angular.module('backbone.formulario').directive('botonesFormulario', botonesFormulario);
    botonesFormularioController.$inject = [
        '$scope',
        'constantesAutorizacion',
        '$uibModal',
        'utilidades',
        'FileSaver',
        'servicioFichasProyectos',
        '$location',
        'sesionServicios',
        'servicioNotificacionesMantenimiento',
        'servicioUsuarios',
        'servicioNotificacionesMensajes',
        '$sessionStorage',
        '$http',
        'constantesBackbone',
        "servicioAcciones",
        '$q',
        "$filter",
        "$timeout",
        'trasladosServicio',
        'viabilidadServicio',
        'constantesTipoBancoProyecto',
        'transversalSgrServicio',
        'priorizacionSgrServicio',
        'procesosSgrServicio'
    ];

    function botonesFormularioController(
        $scope,
        constantesAutorizacion,
        $uibModal,
        utilidades,
        FileSaver,
        servicioFichasProyectos,
        $location,
        sesionServicios,
        servicioNotificacionesMantenimiento,
        servicioUsuarios,
        servicioNotificacionesMensajes,
        $sessionStorage,
        $http,
        constantesBackbone,
        servicioAcciones,
        $q,
        $filter,
        $timeout,
        trasladosServicio,
        viabilidadServicio,
        constantesTipoBancoProyecto,
        transversalSgrServicio,
        priorizacionSgrServicio,
        procesosSgrServicio
    ) {
        var vm = this;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.revisiones;
        vm.resourceGroupId = $sessionStorage.resourceGroupId;
        vm.accionPendiente = null;

        vm.esFlujoPriorizacion = false;
        vm.esFlujoAprobacion = false;

        if ($sessionStorage.idInstanciaIframe != undefined && $sessionStorage.idAccion != undefined) {
            trasladosServicio.obtenerObservacionesPasoPadre($sessionStorage.idInstanciaIframe, $sessionStorage.idAccion).then(
                function (respuesta) {
                    if (respuesta.status === 200) {
                        $sessionStorage.idAccionAnterior = respuesta.data.AccionId;
                        vm.modelo.textoObservacionEnvio = respuesta.data.Observacion;
                    }

                });
        }
        $scope.$watch('modelo', function () {
            vm.refrescarPagina = refrescarPagina;
            vm.redirectIndexPage = redirectIndexPage;
            vm.redirectConsolaProcesos = redirectConsolaProcesos;
            cargarLogs();

            var ficha = {
                Nombre: constantesBackbone.nombreFichaPreguntas,
            };

            $sessionStorage.Ficha = ficha;

            $sessionStorage.fichaPlantilla = {
                NombreReporte: ficha.Nombre,
                PARAM_BORRADOR: true,
                PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                InstanciaId: $sessionStorage.idInstancia,
                NivelId: $sessionStorage.idNivel
            };

            if (vm.modelo.disabled == undefined)
                vm.modelo.disabled = true;

            if (vm.modelo.observacionEnvio == undefined)
                vm.modelo.observacionEnvio = false;

            if (vm.modelo.visible == undefined)
                vm.modelo.visible = false;

            if (vm.modelo.nombreAccion == undefined)
                vm.modelo.nombreAccion = '';

            if (vm.modelo.descripcionAccion == undefined)
                vm.modelo.descripcionAccion = '';

            if (vm.modelo.textoObservacionEnvio == undefined)
                vm.modelo.textoObservacionEnvio = '';



        });

        $timeout(function () {
            if ($('.menu').offset() != undefined) {
                var altura = $('.menu').offset().top;

                $(window).on('scroll', function () {
                    if ($(window).scrollTop() > altura) {
                        $('.menu').addClass('menu-fixed');
                    } else {
                        $('.menu').removeClass('menu-fixed');
                    }
                });
            }
        }, 2000);

        vm.guardarAccion = guardarAccion;
        vm.seleccionarAccionDevolver = seleccionarAccionDevolver;

        function guardarAccion() {
            if (!vm.modelo.disabled) {
                if (vm.modelo.observacionEnvio && (vm.modelo.textoObservacionEnvio == '' || vm.modelo.textoObservacionEnvio == undefined)) {
                    utilidades.mensajeError('Debe diligenciar las observaciones de envío.');
                    return;
                }
                vm.flujoCTEI52 = utilidades.obtenerParametroTransversal('FlujoCTEI52');
                vm.isCTEI52 = $sessionStorage.flujoSeleccionado == vm.flujoCTEI52;
                vm.accionPendiente = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null);
                vm.isSolicitarCtus = $sessionStorage.isSolicitarCtus && $sessionStorage.listadoAccionesTramite.some(x => (x.Ventana === 'solicitarCtusOcadPazSgr' || (x.Ventana === 'previosViabilidadSgr' && x.Entidad == 'Entidad FCTeI')) && x.Estado === 'PasoEnProgreso');
                vm.isCtusAutomatico = vm.isCTEI52 && $sessionStorage.listadoAccionesTramite.some(x => (x.Ventana === 'previosViabilidadSgr' && x.Entidad == 'Entidad FCTeI') && x.Estado === 'PasoEnProgreso');

                vm.flujoPriorizacion = utilidades.obtenerParametroTransversal('FlujosPriorizacionSGR');
                vm.esFlujoPriorizacion = vm.flujoPriorizacion.includes($sessionStorage.flujoSeleccionado.toUpperCase());
                //vm.esFlujoPriorizacion = $sessionStorage.flujoSeleccionado == vm.flujoPriorizacion;

                vm.flujoAprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');
                vm.esFlujoAprobacion = $sessionStorage.flujoSeleccionado.toUpperCase() == vm.flujoAprobacion.toUpperCase();

                if (vm.esFlujoPriorizacion) {
                    procesosSgrServicio.obtenerPriorizacionesPendientes($sessionStorage.idInstanciaIframe).then(
                        function (respuesta) {
                            let informacionPriorizacion = JSON.parse(respuesta.data);
                            let priorizacionesHechas = informacionPriorizacion.reduce(function (suma, registro) { return registro.Priorizado !== null ? suma + 1 : suma }, 0);
                            let priorizacionesFaltanes = informacionPriorizacion.length - priorizacionesHechas;
                            //let ultimaPriorizacionHecha = informacionPriorizacion.reduce(function (ultima, registro) { return registro.Priorizado !== null ? registro.Priorizado : ultima }, null);
                            //let entidadUltimaPriorizacion = informacionPriorizacion.reduce(function (ultima, registro) { return registro.Priorizado !== null ? registro.NombreEntidad : ultima }, null);

                            let instanciaId = $sessionStorage.idInstanciaIframe.toUpperCase();

                            let priorizacionInstanciaActual = informacionPriorizacion.find(registro =>
                                (registro.InstanciaId || '').toUpperCase() === instanciaId
                            );

                            let valorPriorizadoInstanciaActual = priorizacionInstanciaActual ? priorizacionInstanciaActual.Priorizado : null;
                            let entidadInstanciaActual = priorizacionInstanciaActual ? priorizacionInstanciaActual.NombreEntidad : null;

                            if (priorizacionInstanciaActual !== null) {
                                if (valorPriorizadoInstanciaActual == 1 || valorPriorizadoInstanciaActual == true) {

                                    utilidades.mensajeWarning(
                                        "La priorización no podrá ser modificada ni eliminada. ¿Está seguro de continuar?",
                                        function funcionContinuar() {
                                            guardarDefinitivo();
                                        },
                                        function funcionCancelar(reason) {
                                            console.log("reason", reason);
                                        },
                                        "Aceptar",
                                        "Cancelar",
                                        `El proyecto de inversión cambiará de estado a \"Viable Priorizado\".`);

                                } else if (valorPriorizadoInstanciaActual == 0 || valorPriorizadoInstanciaActual == false) {

                                    utilidades.mensajeWarning(
                                        "¿Está seguro de continuar?",
                                        function funcionContinuar() {
                                            guardarDefinitivo();
                                        },
                                        function funcionCancelar(reason) {
                                            console.log("reason", reason);
                                        },
                                        "Aceptar",
                                        "Cancelar",
                                        `El proyecto quedará NO priorizado por la entidad o instancia ${entidadInstanciaActual}, en consecuencia, se archivará y NO podrá reutilizarse para consulta ni para ninguna otra operación`);

                                }
                            }
                        });
                } else if (vm.esFlujoAprobacion) {
                    procesosSgrServicio.obtenerAprobacionesPendientes($sessionStorage.idInstanciaIframe).then(
                        function (respuesta) {

                            let informacionAprobacion = JSON.parse(respuesta.data);
                            let entidadSesion = $sessionStorage.listadoAccionesTramite.find(x => x.Id == $sessionStorage.idAccion).IdEntidad;
                            let aprobacionEntidad = informacionAprobacion.find(x => x.EntidadId == entidadSesion).Aprobado;
                            let entidad = informacionAprobacion.find(x => x.EntidadId == entidadSesion).NombreEntidad;

                            if (typeof aprobacionEntidad !== 'undefined') { 
                                if (aprobacionEntidad == 1) {

                                    utilidades.mensajeWarning(
                                        "¿Está seguro de continuar?",
                                        function funcionContinuar() {
                                            guardarDefinitivo();
                                        },
                                        function funcionCancelar(reason) {
                                            console.log("reason", reason);
                                        },
                                        "Aceptar",
                                        "Cancelar",
                                        `El proyecto quedará aprobado por la entidad o instancia ${entidad}`);

                                } else if (aprobacionEntidad == 0) {
                                    utilidades.mensajeWarning(
                                        "¿Está seguro de continuar?",
                                        function funcionContinuar() {
                                            guardarDefinitivo();
                                        },
                                        function funcionCancelar(reason) {
                                            console.log("reason", reason);
                                        },
                                        "Aceptar",
                                        "Cancelar",
                                        `El proyecto quedará NO aprobado por la entidad o instancia ${entidad}, en consecuencia, se archivará y NO podrá reutilizarse para consulta ni para ninguna otra operación`);
                                }
                            }
                    });
                } else
                if ((vm.resourceGroupId == constantesTipoBancoProyecto.territorio || vm.resourceGroupId == constantesTipoBancoProyecto.sgr) && !vm.accionPendiente) {
                    utilidades.mensajeWarning(
                        "¿Está seguro de continuar?",
                        function funcionContinuar() {
                            guardarDefinitivo();
                        },
                        function funcionCancelar(reason) {
                            console.log("reason", reason);
                        },
                        "Aceptar",
                        "Cancelar",
                        "Una vez finalizado, este formulario no podrá modificarse.");
                } else if (vm.resourceGroupId == constantesTipoBancoProyecto.sgr && vm.isCtusAutomatico) {
                    utilidades.mensajeWarning(
                        "¿Está seguro de continuar?",
                        function funcionContinuar() {
                            guardarDefinitivo();
                        },
                        function funcionCancelar(reason) {
                            console.log("reason", reason);
                        },
                        "Aceptar",
                        "Cancelar",
                        "Se creará una solicitud para emitir un CTUS a la entidad Departamento Nacional de Planeación");
                } else {
                    guardarDefinitivo();
                }
            }
        }

        function guardarDefinitivo() {
            var postDefinitivo = true;
            var parametrosEjecucionFlujo = new Object();
            parametrosEjecucionFlujo.IdInstanciaFlujo = $sessionStorage.idInstanciaFlujoPrincipal;//= vm.idInstancia;
            parametrosEjecucionFlujo.IdAccion = $sessionStorage.idAccion;
            parametrosEjecucionFlujo.PostDefinitivo = postDefinitivo;
            parametrosEjecucionFlujo.ResourceGroupId = $sessionStorage.resourceGroupId;
            parametrosEjecucionFlujo.ObjetoContexto = new Object();
            parametrosEjecucionFlujo.ObjetoContexto.IdUsuario = usuarioDNP;
            parametrosEjecucionFlujo.ObjetoDatos = new Object();

            $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiEjecutarFlujo, parametrosEjecucionFlujo).then(
            function (resultado) {
                if (resultado.data !== null) {
                    if (postDefinitivo) {
                        if (resultado.status === 200) {
                            $sessionStorage.fichaPlantilla = undefined;
                            $sessionStorage.Ficha = undefined;
                            $sessionStorage.guardadoPrevio = true;
                            if (parametrosEjecucionFlujo.ResourceGroupId == constantesTipoBancoProyecto.territorio && !vm.accionPendiente) {
                                utilidades.mensajeSuccess("Usted puede acceder a él desde la consola de procesos",
                                    true,
                                    vm.redirectIndexPage,
                                    vm.redirectConsolaProcesos,
                                    resultado.data.MensajeEjecucion,
                                    "IR A MIS PROCESOS",
                                    "IR CONSOLA DE PROCESOS"
                                );
                                return;
                            }
                            if (parametrosEjecucionFlujo.ResourceGroupId == constantesTipoBancoProyecto.sgr && !vm.accionPendiente) {

                                if (vm.esFlujoPriorizacion) {

                                    let mensajes = resultado.data.MensajeEjecucion.split("|");

                                    if (mensajes.length == 1) {

                                        utilidades.mensajeSuccess(
                                            mensajes[0],
                                            false,
                                            vm.refrescarPagina,
                                            null,
                                            null
                                        );

                                    } else if (mensajes.length == 2) {

                                        utilidades.mensajeSuccess(
                                            mensajes[0],
                                            false,
                                            function funcionContinuar() {
                                                setTimeout(function () {
                                                    utilidades.mensajeSuccess(
                                                        mensajes[1],
                                                        false,
                                                        vm.refrescarPagina,
                                                        null,
                                                        null
                                                    );
                                                }, 500);
                                            },
                                            null,
                                            null
                                        );
                                    }
                                } else if (vm.esFlujoAprobacion) {

                                    let mensaje = resultado.data.MensajeEjecucion;

                                    utilidades.mensajeSuccess(
                                        mensaje,
                                        false,
                                        vm.refrescarPagina,
                                        null,
                                        null
                                    );

                                } else { 

                                    utilidades.mensajeSuccess("Usted puede acceder a él desde la consola de procesos",
                                        false,
                                        vm.refrescarPagina,
                                        null,
                                        resultado.data.MensajeEjecucion
                                    );
                                }
                                return;
                            }
                            if (parametrosEjecucionFlujo.ResourceGroupId == constantesTipoBancoProyecto.sgr && (vm.isSolicitarCtus || vm.isCtusAutomatico)) {
                                transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeCreacionInstanciaCTUS")
                                    .then(function (respuestaParametro) {
                                        if (respuestaParametro.data) {
                                            vm.entidadMensaje = vm.isCtusAutomatico ? 'Departamento Nacional de Planeación' : $sessionStorage.entidadDestinoCtus;
                                            vm.mensajeEjecucion = `${respuestaParametro.data.Valor} ${vm.entidadMensaje}.`;
                                            utilidades.mensajeSuccess("Usted puede acceder a él desde la consola de procesos",
                                                false,
                                                vm.refrescarPagina,
                                                null,
                                                vm.mensajeEjecucion);
                                            return;
                                        }
                                    }, function (error) {
                                        utilidades.mensajeError(error);
                                    });
                                return;
                            }
                            if (parametrosEjecucionFlujo.ResourceGroupId == constantesTipoBancoProyecto.territorio) {
                                utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                                    false,
                                    vm.redirectIndexPage,
                                    null);
                                return;
                            }
                            if (resultado.data.MensajeEjecucion.includes('BPIN')) {
                                utilidades.mensajeSuccess(resultado.data.MensajeEjecucion,
                                    false,
                                    vm.refrescarPagina,
                                    null);
                                return;
                            }
                            utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                                false,
                                vm.refrescarPagina,
                                null);
                        }
                        else {
                            utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                        }
                    }
                    else {
                        if (resultado.status === 200) {

                            sAlert.success($filter('language')('GuardadoTemporal'), 'mensaje').autoRemove();
                        }
                        else {
                            utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                        }
                    }
                }
                else {
                    utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                }
            }
            ).catch(function (e) {

                var mensaje;
                var mensajeRecurso = $filter('language')('ErrorGuardadoTemporal');

                if (e.data === undefined)
                    mensaje = e.message;
                else {
                    if (e.data.ExceptionMessage && (e.data.ExceptionMessage.startsWith("{"))) {
                        try {
                            var excepcion = angular.fromJson(e.data.ExceptionMessage);
                            mensaje = utilidades.generarLogExcepcionHTML(excepcion);
                        } catch (e) {
                            mensaje = $filter('language')('ImposiblePresentarError');
                        }
                    } else
                        mensaje = e.data.ExceptionMessage;
                }
                utilidades.mensajeError(mensajeRecurso.replace("[0]", mensaje));

            });
        }

        function refrescarPagina() {
            location.reload();
        }
        function redirectIndexPage() {
            $location.url("/proyectos/pl");
        }
        function redirectConsolaProcesos() {
            $location.url("/consolaprocesos/index");
        }
        function seleccionarAccionDevolver() {
            if (!vm.accionDevolverSeleccionada) {
                vm.accionDevolverSeleccionada = vm.accionSeleccionada
                    ? angular.copy(vm.accionSeleccionada.Flujo)
                    : null;
            }

            if (vm.modelo.textoObservacionEnvio !== null && vm.modelo.textoObservacionEnvio !== undefined ? vm.modelo.textoObservacionEnvio.length == 0 : vm.modelo.textoObservacionEnvio == null || vm.modelo.textoObservacionEnvio == undefined) {
                utilidades.mensajeError('Debe diligenciar las observaciones de envío.');
                return;
            }

            if (vm.resourceGroupId == constantesTipoBancoProyecto.sgr) {
                utilidades.mensajeWarning(
                    "¿Está seguro de continuar?",
                    function funcionContinuar() {
                        devolverDefinitivo();
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Una vez devuelto, este formulario no podrá modificarse.");
            } else {
                devolverDefinitivo();
            }
        }

        function devolverDefinitivo() {
            vm.seleccionarAccionModal = $uibModal.open({
                animation: true,
                templateUrl: '/src/app/panelEjecucionDeAccion/listarAccionesAnteriores.html',
                controller: 'listarAccionesAnterioresController',
                controllerAs: "vm",
                keyboard: false,
                backdrop: false,
                scope: $scope,
                size: "sm",

                resolve: {
                    listaacciones: $q.resolve($sessionStorage.AccionesDevolucion),
                    idInstancia: $q.resolve($sessionStorage.idInstancia),
                    idAccion: $q.resolve($sessionStorage.idAccion),
                    idAplicacion: $q.resolve($sessionStorage.IdAplicacion),
                    existeFichaGenerar: $q.resolve($sessionStorage.fichaPlantilla !== undefined)
                }
            });
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacion(vm.modelo.descripcionAccion, null, vm.modelo.nombreAccion);
            /*swal({
                title: `<span ><p style='text-align: left; font-weight:600'>¿Que es esto?</p></span><span><p style='text-align: left;font-weight:600'>${vm.modelo.nombreAccion}</p></span>`,
                text: `<p style='text-align: left; overflow:scroll; height:100px;' id='pMensaje'></p>`,
                confirmButtonText: "Aceptar",
                closeOnConfirm: true,
                html: true
            });*/

        }

        function cargarLogs() {
            viabilidadServicio.obtener(vm.idInstancia, "00000000-0000-0000-0000-000000000000").then((result) => {

                var nivel = "";
                var contador = 0;
                result.data.forEach(revision => {
                    if (nivel != revision.NivelId) {
                        contador++;
                    }
                    nivel = revision.NivelId;
                });

                vm.revisiones = contador;
            });
        }

        vm.verLog = function () {
            console.log($sessionStorage);
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/panelPrincial/modales/logs/historicoObservacionesModal.html',
                controller: 'historicoObservacionesModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => vm.idInstancia,
                    IdNivel: () => vm.IdNivel,
                    CodigoProceso: () => $sessionStorage.InstanciaSeleccionada.CodigoProceso,
                    NombreProceso: () => $sessionStorage.InstanciaSeleccionada.NombreFlujo,
                    IdObjetoNegocio: () => $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio,
                },
            });
        }

        vm.descargarFicha = function () {
            crearDocumento($sessionStorage.fichaPlantilla).then(function (fichaTemporal) {
                FileSaver.saveAs(fichaTemporal, fichaTemporal.name);
            }, function (error) {
                utilidades.mensajeError(error);
            });
        }

        //#region Metodos para la generación y consulta de fichas de proyectos
        /**
         * Metodo que genera el documento usando la api de fichas de proyectos.
         * @param fichaPlantilla
         * @returns {*}
         */
        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = $sessionStorage.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha($sessionStorage.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        //var blob = new Blob([respuesta], { type: 'application/pdf' });
                        const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                        var fileOfBlob = new File([blob], nombreArchivo, { type: 'application/pdf' });
                        var archivo = {};

                        var metadatos = {
                            NombreAccion: $sessionStorage.nombreAccion,
                            IdAplicacion: $sessionStorage.IdAplicacion,
                            IdNivel: $sessionStorage.idNivel,
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAccion: $sessionStorage.idAccion,
                            IdInstanciaFlujoPrincipal: $sessionStorage.idInstanciaFlujoPrincipal,
                            IdObjetoNegocio: $sessionStorage.idObjetoNegocio,
                            Size: blob.size,
                            ContenType: 'application/pdf',
                            Extension: extension,
                            FechaCreacion: new Date(),
                            Tipo: 'Ficha',
                            NombreFicha: respuestaFicha.Nombre,
                            TipoFicha: respuestaFicha.Descripcion
                        }

                        archivo = {
                            FormFile: fileOfBlob,
                            Nombre: nombreArchivo,
                            Metadatos: metadatos
                        };

                        if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            archivoServicios.cargarArchivo(archivo, $sessionStorage.IdAplicacion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    reject(response);
                                } else {
                                    resolve(fileOfBlob);
                                }
                            }, function (error) {
                                reject(error);
                            });
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }
    }
    function botonesFormulario() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelo: '=',
                estado: '='
            },
            templateUrl: 'src/app/formulario/ventanas/comun/botonesFormulario.html',
            controller: botonesFormularioController,
            controllerAs: 'vm',
            bindToController: true
        };
    }


})();
