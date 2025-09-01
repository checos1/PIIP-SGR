(function () {
    'use strict';

    viabilidadController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'viabilidadServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'constantesBackbone',
        'sesionServicios',
        'trasladosServicio',
    ];

    function viabilidadController(
        $scope,
        utilidades,
        $sessionStorage,
        viabilidadServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        constantesBackbone,
        sesionServicios,
        trasladosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";

        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        if (vm.ProyectoId === undefined)
            vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.TramiteId = $sessionStorage.TramiteId;
        $sessionStorage.sessionDocumentos = 0;


        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        

        vm.visualizarCumple;

        vm.init = function () {
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '' });
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
                vm.SeccionId = respuesta.data[0].SeccionId;
            });

            obtenerConfiguracionEntidades();
            //ObtenerAmpliarDevolucionTramite();
            vm.visualizarCumple = $sessionStorage.visualizarCumple;
            //para consultar los parametros que se pasan al controllador archivosFormulario
            vm.obtenerDetalleTramiteyRol();
        };

        function obtenerConfiguracionEntidades() {
            return viabilidadServicio.obtenerConfiguracionEntidades(vm.ProyectoId, vm.IdNivel).then(
                function (respuesta) {
                    vm.ConfiguracionEntidades = respuesta.data;
                    $sessionStorage.AplicaTecnico = false;

                    if (respuesta.data != null && respuesta.data != "") {
                        if (respuesta.data.AplicaTecnico == "SI") {
                            $sessionStorage.AplicaTecnico = true;
                            vm.modeloPreguntas = { perfilTecnico: true };
                        }
                    }
                }
            );
        }

        vm.obtenerDetalleTramiteyRol = function () {
            vm.IdRolNivel = [];
            //Se busca el rol asociado al paso de las variables globales
            $sessionStorage.listadoAccionesTramite.forEach(item => {
                if (item.IdNivel === vm.IdNivel)
                    item.Roles.forEach(rol => {
                        vm.IdRolNivel.push(rol);
                    });
            });
            //Luego se busca si el usuario tiene ese rol asociado
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            vm.IdRolNivel.forEach(rolNivel => {
                var rol = roles.find(x => x === rolNivel.IdRol);
                if (rol !== undefined)
                    vm.IdRol = rol;
            });

       
            //Esta variable se utilizan al momento de llamar el controlador documentos-soporte-vi
            vm.tipoTramiteId = $sessionStorage.tipoTramiteId;
            vm.idtipotramitepresupuestal = $sessionStorage.IdTipoTramitePresupuestal;
            vm.tramiteId = $sessionStorage.TramiteId;
            vm.section = "Viabilidad";
            vm.NivelArchivo = constantesBackbone.idNivelViabilidadDefinitiva;
            vm.TipoArchivo = 'proyectos'
            vm.NivelValidacion = constantesBackbone.idNivelViabilidadSectorialPreliminar;

        }

        $scope.tab = 1;

        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };

        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
        }

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'viabilidad', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'soporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'viabilidad': true,
                'soporte':true,
            };
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

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            viabilidadServicio.obtenerErroresViabilidad(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var findErrorsValidaciones = respuesta.data.findIndex(p => p.Errores != null && p.Capitulo != '');
                var findErrorsCumple = respuesta.data.findIndex(p => p.Errores != null && p.Capitulo === '');
                var findErrors = respuesta.data.findIndex(p => p.Errores != null && ((vm.visualizarCumple != true && p.Capitulo != '') || (vm.visualizarCumple == true)));
                vm.visualizarAlerta((findErrors != -1), errorObservacion, (findErrorsValidaciones != -1), (findErrorsCumple != -1));
                //error = (findErrors != -1);
            });
        }

        vm.visualizarAlerta = function (error, errorObservacion, tieneErroresValidacion, tieneErroresCumple) {
            var siguiente = "Siguiente";
            if (tieneErroresValidacion) utilidades.mensajeError("Existen campos obligatorios sin diligenciar. Por favor, ingrese la información faltante.", null, null);        
            else if (!tieneErroresCumple) utilidades.mensajeSuccess("Validación realizada satisfactoriamente. Para continuar, de clic en “Siguiente”", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        }

        function ObtenerAmpliarDevolucionTramite() {
            viabilidadServicio.ObtenerAmpliarDevolucionTramite(vm.ProyectoId, vm.TramiteId).then(function (respuesta) {
                if (respuesta > 0) {
                    vm.callback({ botonDevolver: true, botonSiguiente: true, ocultarDevolver: true });
                }
            });
        }

        /* ---------------------- Validaciones ---------------*/

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }        

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        vm.notificacionValidacionHijos = function (errores) {
            let error = {};
            let validarSoporte = true;
            var erroresList = errores.filter(p => p.Seccion == "soporte");
            if (erroresList[0] !== undefined  )
                      validarSoporte = erroresList[0].Capitulo === null ? false : true;
            

            if ($sessionStorage.sessionDocumentos < 100 && vm.IdNivel.toLowerCase() === vm.NivelValidacion.toLowerCase() && validarSoporte) {
                error = {
                    Seccion: "soporte",
                    Capitulo: "documentopaso",
                    Errores: '{"soportedocumentopaso":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                }
            }
            else {
                error = {
                    Seccion: "soporte",
                    Capitulo: "alojararchivos",
                    Errores: null,
                }
            }
            errores.push(error);



            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null && errores != null) vm.handlerComponentes[i].handlerValidacion({ errores });
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

        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            var estado = true;
            var listHijos = Object.keys(vm.handlerComponentesChecked);
            if (listHijos.length == 0 || newValue === oldValue) {
                return;
            }
            listHijos.forEach(p => {
                if (vm.handlerComponentesChecked[p] == false) {
                    estado = false;
                }
            });
            vm.validarSiguiente = estado;
            vm.callback({ arg: !estado, aprueba: false, titulo: '' });
        });

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
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
    }

    angular.module('backbone').component('viabilidad', {
        templateUrl: "src/app/formulario/ventanas/viabilidad/viabilidad.html",
        controller: viabilidadController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();