(function () {
    'use strict';

    seguimientoReportePasoDosController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'ajustesServicio',
        'viabilidadServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'seguimientoServicio',
        'sesionServicios',
        'trasladosServicio'
    ];

    function seguimientoReportePasoDosController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        ajustesServicio,
        viabilidadServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        seguimientoServicio,
        sesionServicios,
        trasladosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.IdNivel = $sessionStorage.idNivel;
        vm.notificarGuardado = notificarGuardado;

        vm.globalVariables = {
            GuidMacroproceso: justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa),
            GuidInstancia: $sessionStorage.idInstancia,
            IdProyecto: $sessionStorage.proyectoId,
            NivelId: $sessionStorage.idNivel,
            bpin: $sessionStorage.idObjetoNegocio
        }

        vm.notificacionEventHandler = null;
        vm.siguienteDisabled = false;

        vm.handlerComponentes = [
            { id: 1, componente: 'aprobacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            
        ];

        vm.handlerComponentesChecked = {};

        //Inicio
        vm.init = function () {
            vm.guiMacroproceso = constantesBackbone.idEtapaNuevaEjecucion;// vm.globalVariables.GuidMacroproceso;
            $sessionStorage.IdMacroproceso = vm.globalVariables.GuidMacroproceso;
            vm.inicializarComponenteCheck();
            vm.setStorage();
            vm.DatosDocumento();

            

            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, vm.globalVariables.NivelId, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
                const span = document.getElementById('d' + respuesta.data[0].SeccionModificado);
                if (span != undefined && span != null) {
                    span.classList.add("active");
                }
            });
            var validacion = {
                evento: vm.eventoValidar
            }
            let ocultarDevolver = false;
            let oculatarSiguiente = true;
            if (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelCerrarSeguimiento.toLowerCase()) {
                ocultarDevolver = true;
                oculatarSiguiente = true;
                $sessionStorage.GuardarAprobacionEntidadVFExec = true;
            }
            vm.callback({ validacion: validacion, arg: oculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver });
        };

        vm.DatosDocumento = function () {

            $sessionStorage.listadoAccionesTramite.forEach(item => {
                if (item.IdNivel === vm.IdNivel)
                    vm.IdRolNivel = item.Roles[0].IdRol;
            });
            //Luego se busca si el usuario tiene ese rol asociado
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            var rol = roles.find(x => x === vm.IdRolNivel);
            if (rol !== undefined)
                vm.IdRol = rol;


            //Esta variable se utilizan al momento de llamar el controlador documentos-soporte-vi
            vm.tipoTramiteId = $sessionStorage.tipoTramiteId;
            vm.idtipotramitepresupuestal = null;
            vm.tramiteId = 0;
            //vm.section = "Viabilidad";
            //vm.NivelArchivo = constantesBackbone.idNivelViabilidadDefinitiva;
            vm.TipoArchivo = 'proyectos'
            vm.NivelValidacion = constantesBackbone.idNivelJustificaciónAJustesProyecto;

        }

        vm.setStorage = function () {
            $sessionStorage.IdMacroproceso = vm.globalVariables.GuidMacroproceso;
        }
        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
        }

        vm.eventoValidar = function () {

            vm.inicializarComponenteCheck();
            seguimientoServicio.obtenerErroresProyecto(vm.globalVariables.GuidMacroproceso, vm.globalVariables.IdProyecto, vm.globalVariables.GuidInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);

                //var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = false;// respuesta.data[indexobs].Errores == null ? false : true;

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1), errorObservacion);
            });
        }



        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }

        vm.visualizarAlerta = function (error, errorObservacion) {
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }

            let vocultarDevolver = false;
            let voculatarSiguiente = vm.siguienteDisabled;

            //Si Deja los botones habilitados dependiendo de las respuesta del paso 2 y 4
            if ($sessionStorage.Respuesta !== undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelCerrarSeguimiento.toLowerCase() )) {
                vocultarDevolver = $sessionStorage.Respuesta;
                //Miestras hacen las validaciones siempre en el paso 2 el finalizar estara deshabilitado
                voculatarSiguiente = true;
                //voculatarSiguiente = voculatarSiguiente === false ? !$sessionStorage.Respuesta : voculatarSiguiente;
            }
            else if ($sessionStorage.Respuesta === undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelCerrarSeguimiento.toLowerCase() )) {
                vocultarDevolver = true;
                voculatarSiguiente = true;
            }
           
            if (vm.deshabilitar === true) {
                voculatarSiguiente = true;
                vocultarDevolver = true;
            }
            
            vm.callback({ validacion: validacion, arg: voculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
            //vm.callback({ validacion: { tieneError: error }, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        }

        /* ---------------------- Validaciones ---------------*/

        /**
         * Función que recibe los estados de los componentes hijos
         * @param {any} estado true: valido, false: invalido
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
        }

        /**
         * Función que crea las referencias de los métodos de los hijos con el padre 
         * @param {any} handler función referenciada
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
        //    var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
        //    if (indx != -1) {
        //        vm.handlerComponentes[indx].handlerValidacion = handler;
        //        vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        //    }
        //};

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx != "-1") {
                vm.handlerComponentes[indx].handlerValidacion = handler;
                vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
            }
        };

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        /**
         * Función que envía listado de errores a componentes hijos 
         * por referencia configurada en vm.notificacionValidacion.
         * @param {any} errores
         */
        vm.notificacionValidacionHijos = function (errores) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null && errores != null) {
                    try {
                        vm.handlerComponentes[i].handlerValidacion({ errores });
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerValidacion del componente = ' + vm.handlerComponentes[i].componente + '!!');
                    }
                }
            }
        };

        /**
        * Función que visualiza alerta de error tab de componente
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.showAlertError = function (nombreComponente, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        /* ---------------------- Justificación ---------------*/

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
           vm.notificacionCambios(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        };

        vm.notificacionCambiosAjustes = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };

        function notificarGuardado() {
            let vocultarDevolver = false
            if ($sessionStorage.GuardarAprobacionEntidadVFExec !== undefined)
                vocultarDevolver = $sessionStorage.GuardarAprobacionEntidadVFExec;

            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });

        }
    }

    angular.module('backbone').component('seguimientoReportePasoDos', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/reporteAvance/seguimientoReportePasoDos.html",
        controller: seguimientoReportePasoDosController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();