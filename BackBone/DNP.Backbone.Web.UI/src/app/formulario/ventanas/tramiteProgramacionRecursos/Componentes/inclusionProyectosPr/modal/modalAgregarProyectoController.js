(function () {
    'use strict';

    modalAgregarProyectoController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'comunesServicio',
        'tramiteProgramacionRecursosServicio',
        '$scope',
    ];

    function modalAgregarProyectoController(
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        tramiteVigenciaFuturaServicio,
        justificacionCambiosServicio,
        constantesBackbone,
        comunesServicio,
        tramiteProgramacionRecursosServicio,
        $scope,
    ) {
        var vm = this;
        vm.init = init;
        //vm.guardar = guardar;
        vm.cerrar = $sessionStorage.close;
        vm.ProyectoId = $uibModalInstance.proyectoId;
        //vm.TramiteId = $uibModalInstance.tramiteId;
        vm.EntidadDestinoId = $uibModalInstance.InstanciaSeleccionada.entidadId;
        vm.TramiteId = $uibModalInstance.InstanciaSeleccionada.tramiteId;
        vm.TipoRolId = 0;
        //vm.IdInstancia = $uibModalInstance.idInstancia;
        vm.IdInstancia = $uibModalInstance.InstanciaSeleccionada.IdInstancia;
        vm.IdFlujo = $uibModalInstance.InstanciaSeleccionada.IdFlujo;
        vm.idNivel = $uibModalInstance.idNivel;
        vm.idRol = '';
        vm.idsroles = [];
        vm.parametrosObjetosNegocioDto = {};
        vm.textoBuscarBPIN = '';
        vm.textoBuscarNombre = '';
        vm.sinResultados = false;
        vm.mostrarGrilla = false;
        vm.habilitaLimpiar = false;
        vm.seleccionguardado = false;

        vm.datosProyectos = [];

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }    
        }


        vm.buscarProyecto = function () {
            vm.datosProyectos = [];
            vm.datosResponse = {};
            vm.habilitaLimpiar = true;

            vm.textoBuscarBPIN = vm.textoBuscarBPIN == undefined ? "" : vm.textoBuscarBPIN;
            vm.textoBuscarNombre = vm.textoBuscarNombre == undefined ? "" : vm.textoBuscarNombre;

            if (vm.textoBuscarBPIN == '' && vm.textoBuscarNombre == '') {
                vm.errorBusqueda = true;
                vm.habilitaLimpiar = false;
                
            }
            else {                
                var textoBPIN = vm.textoBuscarBPIN == "" ? "null" : vm.textoBuscarBPIN;
                var textoNombre = vm.textoBuscarNombre == "" ? "null" : vm.textoBuscarNombre;
                vm.errorBusqueda = false;
                // datos de prueba
                //tramiteProgramacionRecursosServicio.ObtenerProgramacionBuscarProyecto(204, 2291, vm.textoBuscarBPIN, vm.textoBuscarNombre).then(function (response) {
                tramiteProgramacionRecursosServicio.ObtenerProgramacionBuscarProyecto(vm.EntidadDestinoId, vm.TramiteId, textoBPIN, textoNombre).then(function (response) {
                    if (response === undefined || typeof response === 'string') {
                        vm.mensajeError = response;
                        utilidades.mensajeError(response);
                    } else {

                        if (response.data.length == 0) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            //utilidades.mensajeWarning("", true, false, false, false, 'No se encontraron resultados.');
                            vm.sinResultados = true;
                            vm.mostrarGrilla = false;

                        }
                        else {
                            $scope.datos = jQuery.parseJSON(jQuery.parseJSON(response.data));

                            if ($scope.datos.Proyectos != null) {
                                $scope.datos.Proyectos.forEach(archivo => {

                                    vm.datosProyectos.push({
                                        ProyectoId: archivo.ProyectoId,
                                        BPIN: archivo.CodigoBPIN,
                                        NombreProyecto: archivo.NombreProyecto,
                                        select: false,
                                        PeriodoProyectoId: archivo.PeriodoProyectoId,
                                        Accion: archivo.Accion,
                                        TipoProyecto: archivo.TipoProyecto,
                                        TramiteId: vm.TramiteId,
                                        EntidadId: vm.EntidadDestinoId
                                    });
                                });
                                //vm.textoBuscarBPIN = '';
                                //vm.textoBuscarNombre = '';    
                                vm.mostrarGrilla = true;
                                vm.sinResultados = false;
                            }
                            else {
                                vm.mostrarGrilla = false;
                                vm.sinResultados = true;
                            }
                        }
                    }
                }, error => {
                    console.log(error);
                });
            }

        }

        vm.limpiarBusqueda = function () {
            vm.textoBuscarBPIN = '';
            vm.textoBuscarNombre = '';
            vm.sinResultados = false;
            vm.mostrarGrilla = false;
            vm.errorBusqueda = false;
        }

        vm.guardar = function () {
            let Programacion = {};
            let ValoresProgramacion = [];

            if (vm.mostrarGrilla && !vm.sinResultados) {
                angular.forEach(vm.datosProyectos, function (series) {
                    if (series.select) {
                        let valores = {
                            ProyectoId: series.ProyectoId,
                            DistribucionCuotaComunicadaNacionCSF: 0,
                            DistribucionCuotaComunicadaNacionSSF: 0,
                            DistribucionCuotaComunicadaPropios: 0
                        };

                        ValoresProgramacion.push(valores);
                        vm.seleccionguardado = true;
                    }
                });

                //ObtenerSeccionCapitulo();
                Programacion.TramiteId = vm.TramiteId;
                Programacion.NivelId = vm.idNivel;
                Programacion.EntidadDestinoId = vm.EntidadDestinoId;
                Programacion.ValoresProgramacion = ValoresProgramacion;
                Programacion.seccionCapitulo = 0;

                if (vm.seleccionguardado) {
                return comunesServicio.GuardarDatosInclusion(Programacion).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Exito) {
                                // guardarCapituloModificado();
                                //vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });

                                utilidades.mensajeSuccess('Estos apareceran al final del listado en la última paginación', false, false, false, "Los proyectos seleccionados han sido incluidos en la tabla con éxito.");
                                vm.seleccionguardado = false;
                                $sessionStorage.close();
                            }
                            else {
                                utilidades.mensajeError(respuesta.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("", null, "Error al realizar la operación");
                        }
                    });
                }
                else utilidades.mensajeError("", null, "Error al realizar la operación");
            }
            else { utilidades.mensajeError("", null, "Error al realizar la operación"); }
        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            vm.seccionCapitulo = $uibModalInstance.seccionCapitulo
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $uibModalInstance.idInstancia,
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

    }

    angular
        .module('backbone')
        .controller('modalAgregarProyectoController', modalAgregarProyectoController);





})();

