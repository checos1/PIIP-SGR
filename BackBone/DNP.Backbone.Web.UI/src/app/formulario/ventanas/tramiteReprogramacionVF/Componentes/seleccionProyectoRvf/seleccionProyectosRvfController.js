(function () {
    'use strict';

    seleccionProyectosRvfController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'flujoServicios',
        'trasladosServicio',
        'constantesBackbone',
        'justificacionCambiosServicio',
        'tramiteReprogramacionVfServicio',
        '$scope',
        'comunesServicio',
    ];

    function seleccionProyectosRvfController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio,
        flujoServicios,
        trasladosServicio,
        constantesBackbone,
        justificacionCambiosServicio,
        tramiteReprogramacionVfServicio,
        $scope,
        comunesServicio,
    ) {
        var vm = this;
        vm.lang = "es";

        vm.entidad = undefined;
        vm.IdEntidadSeleccionada = $sessionStorage.idEntidad;
        vm.sector = undefined;
        vm.BPIN = $sessionStorage.BPIN;
        vm.proyecto = $sessionStorage.nombreProyecto;
        vm.vigenciaInicial = undefined;
        vm.vigenciaFinal = undefined;
        vm.listaValores = [];
        vm.abrirpanelseleccion = abrirpanelseleccion;
        vm.estadoSeleccion = undefined;
        vm.eliminarAsociacion = eliminarAsociacion
        vm.crearAjuste = crearAjuste;
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: $sessionStorage.idInstancia,
            IdEntidad: vm.IdEntidadSeleccionada
        };


        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapituloAsociarId = null;
        vm.seccionCapituloCrearId = null;

        vm.estadoAjusteCreado = false;
        vm.estadoAjusteFinalizado = false;

        vm.habilitarautorizacion = false;
        vm.tieneautorizacionasociada = $sessionStorage.tieneautorizacionasociada;
        //Validaciones
        vm.nombreComponente = "proyectoasociarproyecto";
        vm.cargarDatos = cargarDatos;

        vm.inicaliazarAsociacion = inicaliazarAsociacion;

        function inicaliazarAsociacion() {
            //if (vm.listaValores.length == 0)
            //    vm.estadoSeleccion = "Sin asociación";
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.abrirpanelseleccion();
            tramiteVigenciaFuturaServicio.obtenerProyectos(vm.BPIN).then(function (response) {
                var obj = JSON.parse(response.data);
                $sessionStorage.proyectoId = obj[0].ProyectoId;
            });
            vm.cargarDatos();
        }


        $scope.$watch(() => $sessionStorage.tieneautorizacionasociada
            , (newVal, oldVal) => {
                if (newVal != oldVal) {
                    vm.tieneautorizacionasociada = newVal;
                }
            }, true);

        function cargarDatos() {
            var ptramiteId = $sessionStorage.InstanciaSeleccionada.tramiteId;
            if (ptramiteId !== undefined) {
                tramiteReprogramacionVfServicio.ObtenerAutorizacionAsociada(ptramiteId).then(
                    function (respuesta) {
                        if (respuesta.data.Id != 0 && respuesta.data.Id != undefined) {
                            $sessionStorage.tieneautorizacionasociada = true;
                        }
                        else {
                            $sessionStorage.tieneautorizacionasociada = false;
                        }
                    }
                )
            }
        }
        vm.rotated = false;
        function abrirpanelseleccion() {

            //var acc = document.getElementById('divasociaciarproyecto');
            //var i;
            //var rotated = false;


            //acc.classList.toggle("active");
            //var panel = acc.nextElementSibling;
            //if (panel.style.maxHeight) {
            //    panel.style.maxHeight = null;
            //} else {
            //    panel.style.maxHeight = panel.scrollHeight + "px";
            //}
            //var div = document.getElementById('u4_imgcargaarchivo'),
            //    deg = vm.rotated ? 180 : 0;
            //div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            //div.style.mozTransform = 'rotate(' + deg + 'deg)';
            //div.style.msTransform = 'rotate(' + deg + 'deg)';
            //div.style.oTransform = 'rotate(' + deg + 'deg)';
            //div.style.transform = 'rotate(' + deg + 'deg)';
            //vm.rotated = !vm.rotated;
        }

        function abrirTooltip() {
            utilidades.mensajeInformacion('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
        }

        /* ------------------------ Validaciones ---------------------------------*/

        //vm.notificacionValidacionPadre = function (errores) {
        //    //console.log("Validación  - CD Pvigencias futuras");
        //    vm.limpiarErrores(errores);
        //    if (errores != undefined) {
        //        var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
        //        var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
        //        var isValid = (erroresJson == null || erroresJson.length == 0);
        //        if (!isValid) {
        //            erroresJson[vm.nombreComponente].forEach(p => {

        //                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
        //            });
        //        }
        //        vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        //    }
        //}

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl!= undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }


        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-liberacion-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-estadoProyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
           


        }

        function eliminarAsociacion() {
            if ($sessionStorage.proyectoId === undefined || $sessionStorage.proyectoId === '' || $sessionStorage.proyectoId === 0) {
                utilidades.mensajeError('No hay proyectos asociados.');
            }
            else {
                utilidades.mensajeWarning(" ¿Esta seguro de continuar?",
                    function funcionContinuar() {
                        var eliminarAsociacionDto = {
                            TramiteId: $sessionStorage.tramiteId,
                            InstanciaId: vm.instanciaId,
                            ProyectoId: $sessionStorage.proyectoId,
                            IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                        };
                        trasladosServicio.eliminarProyectoTramite(eliminarAsociacionDto, eliminarAsociacionDto)
                            //tramiteVigenciaFuturaServicio.eliminarAsociacion(eliminarAsociacionDto)
                            .then(function (response) {
                                if (response.status == "200") {
                                    $sessionStorage.proyectoId = 'e';
                                    utilidades.mensajeSuccess("", false, false, false, "Se ha eliminado la asociación del proyecto con éxito.");

                                    //para guardar los capitulos modificados y que se llenen las lunas, este modificado en 0
                                    ObtenerSeccionCapitulo();
                                    eliminarCapitulosModificados(vm.seccionCapituloAsociarid);
                                    eliminarCapitulosModificados(vm.seccionCapituloCrear);

                                    $sessionStorage.EstadoAjusteCreado = false;
                                    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                                    $sessionStorage.EstadoAjusteFinalizado = false;
                                    vm.estadoAjusteFinalizado = $sessionStorage.EstadoAjusteFinalizado;
                                    $sessionStorage.tieneautorizacionasociada = false;
                                    ///Ajuste Estabilizacion
                                    // $sessionStorage.proyectoId = 0;
                                    // notificar usuarios
                                    var texto = "El proyecto con BPIN " + $sessionStorage.BPIN + " ha sido desasociado del tramite " + $sessionStorage.idObjetoNegocio;
                                    comunesServicio.notificarUsuariosPorInstanciaPadre($sessionStorage.idInstanciaIframe, "Desasociar proyecto", texto);

                                    $sessionStorage.BPIN = '';
                                    $sessionStorage.nombreProyecto = '';
                                    vm.BPIN = $sessionStorage.BPIN;
                                    vm.proyecto = $sessionStorage.nombreProyecto;
                                }
                                else {
                                    //utilidades.mensajeError("Error al realizar la operación", false);
                                }
                            })
                            .catch(error => {
                                if (error.status == "400") {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Se eliminará la asociación del proyecto y los posibles datos incluidos sobre su autorización "
                )
            }
        }

        function crearAjuste() {

            var listaproyectos = [];
            if ($sessionStorage.BPIN === undefined || $sessionStorage.BPIN === '') {
                utilidades.mensajeError('No hay proyectos asociados.');
            }
            else {
                utilidades.mensajeWarning(" ¿Esta seguro de continuar?",
                    function funcionContinuar() {
                        listaproyectos.push($sessionStorage.BPIN);
                        var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];
                        trasladosServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                            .then(function (resultado) {
                                var listaqueyatiene = resultado.data;
                                if (listaqueyatiene.length > 0)
                                    utilidades.mensajeError("El proyecto ya tiene generado el ajuste!");
                                else {
                                    const tramiteDto = {
                                        FlujoId: vm.parametros.idFlujo,
                                        ObjetoId: $sessionStorage.BPIN,
                                        UsuarioId: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                                        RolId: usuarioRolId,
                                        TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                                        ListaEntidades: [vm.IdEntidadSeleccionada],
                                        IdInstancia: vm.instanciaId,
                                        Proyectos: [{
                                            IdInstancia: vm.instanciaId,
                                            IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                                            IdObjetoNegocio: $sessionStorage.BPIN,
                                            IdEntidad: vm.IdEntidadSeleccionada,
                                            IdAccion: $sessionStorage.idAccion,
                                            ProyectoId: $sessionStorage.proyectoId,
                                            FlujoId: vm.parametros.idFlujo
                                        }]
                                    };

                                    flujoServicios.generarInstancia(tramiteDto)
                                        .then(function (resultado) {
                                            if (resultado.length <= 0 || (resultado[0].MensajeOperacion !== undefined && resultado[0].Exitoso === false)) {
                                                utilidades.mensajeError('No se creó el ajuste. ' + resultado[0].MensajeOperacion);
                                                return;
                                            }
                                            var instanciasFallidas = resultado.filter(function (instancia) {
                                                return !instancia.Exitoso;
                                            });
                                            var cantidadInstanciasFallidas = instanciasFallidas.lenght;

                                            if (cantidadInstanciasFallidas) {
                                                utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                                            } else {
                                                ActualizarInstanciaProyecto(resultado);


                                            }

                                        });

                                }


                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Al crear el ajuste no se podrán hacer modificaciones sobre el proyecto y la autorización seleccionados."
                )
            }
        }

        function ActualizarInstanciaProyecto(resultado) {

            vm.listaIstanciasCreadas = [];
            vm.listaIstanciasCreadas = resultado;

            vm.listaIstanciasCreadas.forEach(instancia => {
                const proyectoTramiteDto = {
                    TramiteId: vm.TramiteId,
                    InstanciaId: instancia.InstanciaId,
                    ProyectoId: $sessionStorage.proyectoId,
                    FlujoId: $sessionStorage.idFlujoIframe,
                    EntidadId: $sessionStorage.idEntidad
                };

                trasladosServicio.ActualizarInstanciaProyecto(proyectoTramiteDto).then(
                    function (resultado) {

                        if (resultado.data && (resultado.statusText === "OK" || resultado.status === 200)) {
                            utilidades.mensajeSuccess(
                                "Espere la notificación que será enviada desde el formulario de Ajustes del proyecto para continuar diligenciando este formulario.",
                                false,
                                function funcionContinuar() {

                                    //para guardar los capitulos modificados y que se llenen las lunas
                                    ObtenerSeccionCapitulo();
                                    guardarCapituloModificado(vm.seccionCapituloCrear);


                                    $sessionStorage.EstadoAsociacionVF = 'En Verificación';
                                    $sessionStorage.EstadoAjusteCreado = true;
                                    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;

                                },
                                false,
                                "El ajuste se ha creado con éxito.");


                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error);
                        }
                    }
                );
            });
        }


        vm.validarAsociarProyecto = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarLiberacion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-liberacion-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarEstadoControlPosterior = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-estadoProyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.errores = {
            'AS001': vm.validarAsociarProyecto,
            'AS002': vm.validarLiberacion,
            'AS006': vm.validarEstadoControlPosterior,
          


        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado(capituloID) {

            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: capituloID,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
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

        function eliminarCapitulosModificados(capituloID) {

            var data = {
                ProyectoId: 0,
                Justificacion: "",
                SeccionCapituloId: capituloID,
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
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            ObtenerSeccionCapituloAsociar();
            ObtenerSeccionCapituloCrea();
        }
        function ObtenerSeccionCapituloAsociar() {
            const span = document.getElementById('id-capitulo-proyectoasociarproyecto');
            vm.seccionCapituloAsociarid = span.textContent;


        }

        function ObtenerSeccionCapituloCrea() {
            const span = document.getElementById('id-capitulo-proyectocrearajuste');
            vm.seccionCapituloCrear = span.textContent;
        }

        vm.deshabilitarBotonDevolverSeccionProyecto = function () {
            vm.callback();

        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();

        }



    }

    angular.module('backbone').component('seleccionProyectosRvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/seleccionProyectosRvf.html",
        controller: seleccionProyectosRvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
            rolanalista: '@',
        }
    });

})();