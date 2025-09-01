(function () {
    'use strict';

    modalAgregarPoliticaTransversalMlController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'comunesServicio',
        'utilidades'
    ];

    function modalAgregarPoliticaTransversalMlController(
        $uibModalInstance,
        $sessionStorage,
        comunesServicio,
        utilidades
    ) {
        var vm = this;
        vm.init = init;
        vm.cerrar = $sessionStorage.close;
        vm.guardar = guardar;
        vm.listaPoliticasProyecto = [];        
        vm.RespuestaAgregar = null;
        vm.guardo = false;
        vm.options = [];
        var lstRolesTodo = $uibModalInstance.usuario.roles;
        var lsRoles = [];
        for (var ls = 0; ls < lstRolesTodo.length; ls++)
            lsRoles.push(lstRolesTodo[ls].IdRol)

        var parametros = {
            "Aplicacion": nombreAplicacionBackbone,
            "ListaIdsRoles": lsRoles,
            "IdUsuario": usuarioDNP,
            "IdObjeto": $uibModalInstance.idInstancia,      //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
            "InstanciaId": $uibModalInstance.idInstancia,   //'88ea329d-f240-4868-9df7-86c74fb2ecfa', 
            "IdFiltro": $uibModalInstance.idAccionAnterior
        }

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.obtenerPoliticasTransversales();
        }

        vm.obtenerPoliticasTransversales = function () {
            return comunesServicio.consultarPoliticasTransversalesProgramacion($uibModalInstance.tramiteproyectoid).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {                        
                        var arreglolistas = jQuery.parseJSON(respuesta.data);                        
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosGenerales = arregloGeneral.Politicas;
                        var listaPoliticasProy = [];
                        var enProyecto = false;

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            if (arregloDatosGenerales[pl].EnProyecto) {
                                enProyecto = true;
                            }

                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                deshabilitar: enProyecto
                            }

                            listaPoliticasProy.push(politicasProyecto);
                            enProyecto = false;
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }

        function guardar() {
            var listaPolitica = vm.listaPoliticasProyectos;
            vm.ff = [];
            listaPolitica.forEach(ff => {
                if (ff.activado) {
                    vm.ff.push(ff);
                }
            });

            var parametros = {
                ProyectoId: 0,
                bpin: $uibModalInstance.tramiteproyectoid,
                Politicas: vm.ff
            };

            return comunesServicio.agregarPoliticasTransversalesProgramacion(parametros)
                .then(function (response) {
                    let exito = response.statusText;
                    if (exito == "OK") {
                        vm.obtenerPoliticasTransversales();
                        utilidades.mensajeSuccess('Las nuevas políticas se visualizan ahora en la tabla "Políticas transversales asociadas."', false, function funcionContinuar() {
                            $sessionStorage.close();
                        }, false, 'Los datos se han agregado con éxito'); 
                    }
                    else {
                        var mensaje = response.data.Mensaje;
                        utilidades.mensajeError(mensaje.substr(mensaje.indexOf(':') + 1), false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });

        }
   
    }
    angular.module('backbone').controller('modalAgregarPoliticaTransversalMlController',
        modalAgregarPoliticaTransversalMlController, {
            bindings: {         
                tramiteproyectoid: '@',
                proyectoid: '@',
            },
        });
})();
