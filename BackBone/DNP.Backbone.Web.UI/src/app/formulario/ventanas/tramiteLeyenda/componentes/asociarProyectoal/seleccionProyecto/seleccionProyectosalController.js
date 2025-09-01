(function () {
    'use strict';

    seleccionProyectosalController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'flujoServicios',
        'trasladosServicio',
        'constantesBackbone',
        'justificacionCambiosServicio',
        'aclaracionLeyendaServicio',
        '$scope'
    ];

    function seleccionProyectosalController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio,
        flujoServicios,
        trasladosServicio,
        constantesBackbone,
        justificacionCambiosServicio,
        aclaracionLeyendaServicio,
        $scope
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
        vm.eliminarAsociacion = eliminarAsociacion;

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
        vm.seccionsolicitarModifi = null;

        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
        vm.estadoAjusteFinalizado = false;


        //Validaciones
        vm.nombreComponente = "asociarproyectoseleccionproyec";


        vm.inicaliazarAsociacion = inicaliazarAsociacion;

        function inicaliazarAsociacion() {
            //if (vm.listaValores.length == 0)
            //    vm.estadoSeleccion = "Sin asociación";
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.abrirpanelseleccion();
            aclaracionLeyendaServicio.obtenerProyectos(vm.BPIN).then(function (response) {
                var obj = JSON.parse(response.data);
                $sessionStorage.proyectoId = obj[0].ProyectoId;
            });

            $scope.$watch(function () {
                if (vm.estadoAjusteCreado != $sessionStorage.EstadoAjusteCreado)
                    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                return $sessionStorage;
            }, function (newVal, oldVal)
            {
            }, true);
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

       

        function eliminarAsociacion() {
            utilidades.mensajeWarning("¿Esta seguro de continuar?",
                function funcionContinuar() {
                    var eliminarAsociacionDto = {
                        TramiteId: $sessionStorage.tramiteId,
                        InstanciaId: vm.instanciaId,
                        ProyectoId: $sessionStorage.proyectoId
                    };

                    aclaracionLeyendaServicio.eliminarAsociacion(eliminarAsociacionDto)
                        .then(function (response) {
                            if (response.status == "200") {
                                $sessionStorage.proyectoId = 'e';
                                utilidades.mensajeSuccess("", false, false, false, "El proyecto ha sido desasociado con éxito.");

                                //para guardar los capitulos modificados y que se llenen las lunas, este modificado en 0
                                ObtenerSeccionCapitulo();
                                eliminarCapitulosModificados(vm.seccionCapituloAsociarid);
                                eliminarCapitulosModificados(vm.seccionsolicitarModifi);

                                $sessionStorage.EstadoAjusteCreado = false;
                                vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                                $sessionStorage.EstadoAjusteFinalizado = false;
                                vm.estadoAjusteFinalizado = $sessionStorage.EstadoAjusteFinalizado;
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
                "El proyecto será desasociado."
            )
        }

        

        

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
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


        }

        vm.validarValoresVigenciaAsociarproyectoAsociarproyecto = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.errores = {
            'AL001': vm.validarValoresVigenciaAsociarproyectoAsociarproyecto,



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
        }
        function ObtenerSeccionCapituloAsociar() {
            const span = document.getElementById('id-capitulo-asociarproyectoseleccionproyec');
            vm.seccionCapituloAsociarid = span.textContent;

            const spanModifi = document.getElementById('id-capitulo-informacionpresupuestalsolicitudmodifi');
            vm.seccionsolicitarModifi = spanModifi.textContent;
        }
     
        vm.deshabilitarBotonDevolverSeccionProyecto = function () {
            vm.callback();

        }



    }

    angular.module('backbone').component('seleccionProyectosal', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/asociarProyectoal/seleccionProyecto/seleccionProyectosal.html",
        controller: seleccionProyectosalController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
        }
    });

})();