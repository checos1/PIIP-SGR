(function () {
    'use strict';

    ajustesController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'ajustesServicio',
        'viabilidadServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'sesionServicios',
    ];



    function ajustesController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        ajustesServicio,
        viabilidadServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        sesionServicios
    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.eventoValidar = eventoValidar;
        vm.eventoHabilitarEdicion = eventoHabilitarEdicion;
        vm.ProyectoId = $sessionStorage.proyectoId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.DescripcionAccionNivel = $sessionStorage.DescripcionAccionNivel;
        vm.verificaConpes = verificaConpes;
        vm.nombreaccion = $sessionStorage.nombreAccion;
        vm.aprueba = false;
        $("#editarButton").hide();
        $("#validarButton").show();
        $sessionStorage.edicionConpes = true;
        vm.conpesDocumentos = false;
        vm.justificacionConpes = false;
        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.DatosGeneralesProyectos = [{
            ProyectoId: 0,
            NombreProyecto: "",
            BPIN: "",
            EntidadId: 0,
            Entidad: "",
            SectorId: 0,
            Sector: "",
            EstadoId: 0,
            Estado: "",
            Horizonte: "",
            Valor: 0
        }];

        vm.notificacionEventHandler = null;
        vm.siguienteDisabled = false;

        /**
         * Listado de componentes hijos, obligatorio para estructura de validación
         * */
        vm.handlerComponentes = [
            { id: 1, componente: 'datosgenerales', handlerValidacion: null, handlerCambios: null, handlerCapitulos: []},
            { id: 2, componente: 'recursos', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 3, componente: 'focalizacionpol', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 4, componente: 'justificacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 5, componente: 'soporte', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] }
        ];

        vm.handlerComponentesChecked = {};

        //Inicio
        vm.init = function () {
            $sessionStorage.IdMacroproceso = vm.guiMacroproceso;
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: eventoValidar }, arg: true, aprueba: false, titulo: '' });
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
            });
            //Esto se agrega para el funcionamiento del controlador de documentos
            vm.obtenerDetalleTramiteyRol();
        };

        vm.abrirMGA = function () {
            ajustesServicio.ObtenerTokenMGA(vm.BPIN, tipoUsuarioAutenticado).then(function (respuesta) {
                window.open(respuesta.data, '_blank').focus();
            });
        };

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
        }

        //Esto se agrega para el funcionamiento del controlador de documentos
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
            vm.idtipotramitepresupuestal = null;
            vm.tramiteId = $sessionStorage.TramiteId;
            //vm.section = "Viabilidad";
            //vm.NivelArchivo = constantesBackbone.idNivelViabilidadDefinitiva;
            vm.TipoArchivo = 'proyectos'
            vm.NivelValidacion = constantesBackbone.idNivelJustificaciónAJustesProyecto;

        }

        function eventoValidar() {
            vm.ProyectoId = $sessionStorage.proyectoId;
            vm.inicializarComponenteCheck();
            ajustesServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                if (indexobs < 0) {
                    var errorObservacion = false;
                } else {
                    var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;
                }
                vm.visualizarAlerta((findErrors != -1), errorObservacion)
            });
        }

        function eventoHabilitarEdicion() {
            $("#editarButton").hide();
            $("#validarButton").show();
        }

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
        }

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo) {
                $("#ver").html('Ocultar qué es esto');
            }
            else {
                $("#ver").html('Ver qué es esto');
            }
        }

        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;


        function verificaConpes(arg, aprueba, titulo = '') {
            vm.aprueba = arg;
            vm.justificacionConpes = vm.infoArchivo.conpes.justificacion;
            vm.conpesDocumentos = vm.infoArchivo.conpes.conpesDocumentos;

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

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion,
            }

            vm.siguienteDisabled = (hijosCorrectos == false);
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
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
        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
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
            let error = {};

            if ($sessionStorage.sessionDocumentos < 100 && vm.IdNivel.toLowerCase() === vm.NivelValidacion.toLowerCase()) {
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
            vm.siguienteDisabled = true
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
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
    }

    angular.module('backbone').component('ajustes', {
        templateUrl: "src/app/formulario/ventanas/ajustes/ajustes.html",
        controller: ajustesController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();