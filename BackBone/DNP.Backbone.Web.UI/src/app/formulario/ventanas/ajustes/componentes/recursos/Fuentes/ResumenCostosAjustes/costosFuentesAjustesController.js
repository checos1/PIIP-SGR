(function() {
    'use strict';

    costosFuentesAjustesController.$inject = [
        'costosFuentesAjustesServicio',
        '$sessionStorage',
        'constantesBackbone',
        '$scope',
    ];

    function costosFuentesAjustesController(
        costosFuentesAjustesServicio,
        $sessionStorage,
        constantesBackbone,
        $scope,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idNivel;
        vm.idAccion = $sessionStorage.idNivel;
        vm.idFormulario = $sessionStorage.idNivel;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.existeDiferencia = false;
        vm.ConvertirNumero = ConvertirNumero;

        vm.listaResumen_inv = [];
        vm.listaResumen_prei = [];
        vm.listaResumen_ope = [];
        vm.listaResumen = [];
        let resumenSubtotal_ope = {};
        let resumenSubtotal_prei = {};
        let resumenSubtotal_inv = {};
        let mensajeInv = '';
        let mensajeOpe = '';
        let mensajePrei = '';
        vm.mensaje = "";
        vm.errores = {
            preinversion: false,
            inversion: false,
            operacion: false
        }


        vm.inicializar = function() {
            vm.validacionerrorfuentes({ handler: vm.notificacionValidacionPadre });

            vm.listaResumen_inv = [];
            vm.listaResumen_prei = [];
            vm.listaResumen_ope = [];
            vm.listaResumen = [];

            obtenerCostosPIIPvsFuentesPIIP();
            vm.validacionguardadopadre({ handler: vm.recargarHijo });
            vm.recargarresumen({ handler: vm.recargarHijo });
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.recargarHijo = function() {
            obtenerCostosPIIPvsFuentesPIIP();
        }

        vm.diferencia = function(valor) {
            return valor;
        }

        function obtenerCostosPIIPvsFuentesPIIP() {
            vm.listaResumen_inv = [];
            vm.listaResumen_prei = [];
            vm.listaResumen_ope = [];
            vm.listaResumen = [];

            mensajeInv = '';
            mensajeOpe = '';
            mensajePrei = '';

            return costosFuentesAjustesServicio.obtenerCostosPIIPvsFuentesPIIP($sessionStorage.idInstancia, vm.idUsuario, vm.idInstancia).then(
                function(respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.listaResumen_inv = [];
                        vm.listaResumen_prei = [];
                        vm.listaResumen_ope = [];
                        vm.listaResumen = [];

                        let mensaje = "";

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosEtapas = arregloGeneral.Etapas;

                        var currentYear = new Date().getFullYear();

                        let total_invCostos = 0,
                            total_invAjuste = 0,
                            total_opeCostos = 0,
                            total_opeAjuste = 0,
                            total_preiCostos = 0,
                            total_preiAjuste = 0,
                            esDiferente = false

                        for (var ls = 0; ls < arregloDatosEtapas.length; ls++) {
                            switch (arregloDatosEtapas[ls].Etapa) {
                                case 'Operación':
                                    {
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {
                                            if (arregloDatosResumen[res].Vigencia >= currentYear && arregloDatosResumen[res].Valorcosto !== arregloDatosResumen[res].ValorFuentes) {
                                                mensajeOpe += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                            }
                                            vm.errores.operacion = (mensajeOpe.length > 0)
                                            var resumenOpe = {
                                                icono: "",
                                                diferente: esDiferente,
                                                vigencia: arregloDatosResumen[res].Vigencia,
                                                ope_costos: arregloDatosResumen[res].Valorcosto,
                                                ope_ajuste: arregloDatosResumen[res].ValorFuentes,
                                            }

                                            total_opeCostos += arregloDatosResumen[res].Valorcosto;
                                            total_opeAjuste += arregloDatosResumen[res].ValorFuentes;

                                            vm.listaResumen_ope.push(resumenOpe);
                                            esDiferente = false;
                                        }

                                        if (mensajeOpe) {
                                            mensajeOpe = 'En vigencia: ' + mensajeOpe.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capitulo de costos de actividades para ajustar la información.';
                                        }

                                        resumenSubtotal_ope = {
                                            icono: "=",
                                            tituloOperacion: "Total Operación",
                                            subtotal_opeCostos: total_opeCostos,
                                            subtotal_opeAjuste: total_opeAjuste,
                                        }
                                        break;
                                    }
                                case 'Preinversión':
                                    {
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {
                                            if (arregloDatosResumen[res].Vigencia >= currentYear && arregloDatosResumen[res].Valorcosto !== arregloDatosResumen[res].ValorFuentes) {
                                                mensajePrei += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                            }
                                            vm.errores.preinversion = (mensajePrei.length > 0)
                                            var resumenPrei = {
                                                icono: "",
                                                diferente: esDiferente,
                                                vigencia: arregloDatosResumen[res].Vigencia,
                                                prei_costos: arregloDatosResumen[res].Valorcosto,
                                                prei_ajuste: arregloDatosResumen[res].ValorFuentes,
                                            }

                                            total_preiCostos += arregloDatosResumen[res].Valorcosto;
                                            total_preiAjuste += arregloDatosResumen[res].ValorFuentes;

                                            vm.listaResumen_prei.push(resumenPrei);
                                            esDiferente = false;
                                        }

                                        if (mensajePrei) {
                                            mensajePrei = 'En vigencia: ' + mensajePrei.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capitulo de costos de actividades para ajustar la información.';
                                        }

                                        resumenSubtotal_prei = {
                                            icono: "=",
                                            tituloPreinversion: "Total Preinversión",
                                            subtotal_preiCostos: total_preiCostos,
                                            subtotal_preiAjuste: total_preiAjuste,
                                        }
                                        break;
                                    }
                                case 'Inversión':
                                    {
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {

                                            if (arregloDatosResumen[res].Vigencia >= currentYear && parseFloat(arregloDatosResumen[res].Valorcosto) !== parseFloat(arregloDatosResumen[res].ValorFuentes)) {
                                                mensajeInv += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                            }
                                            vm.errores.inversion = (mensajeInv.length > 0);

                                            var resumenInv = {
                                                icono: "",
                                                diferente: esDiferente,
                                                vigencia: arregloDatosResumen[res].Vigencia,
                                                inv_costos: arregloDatosResumen[res].Valorcosto,
                                                inv_ajuste: arregloDatosResumen[res].ValorFuentes,
                                            }

                                            total_invCostos += arregloDatosResumen[res].Valorcosto;
                                            total_invAjuste += arregloDatosResumen[res].ValorFuentes;

                                            vm.listaResumen_inv.push(resumenInv);
                                            esDiferente = false;
                                        }

                                        if (mensajeInv) {
                                            mensajeInv = 'En vigencia: ' + mensajeInv.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capitulo de costos de actividades para ajustar la información.';
                                        }

                                        resumenSubtotal_inv = {
                                            icono: "=",
                                            tituloInversion: "Total Inversión",
                                            subtotal_invCostos: total_invCostos,
                                            subtotal_invAjuste: total_invAjuste,
                                        }
                                        break;
                                    }
                            }
                        }
                        vm.mostrarTab(1);
                    } else {
                        toastr.error("No se encontraron datos para el Bpin a consultar...");
                    }
                });
        }

        vm.mostrarTab = function(origen) {
            let listafinal = [];
            let resumenSubtotalFinal = {};
            let mensaje = "";
            vm.mensaje = "";
            switch (origen) {
                case 1:
                    {
                        for (var ls = 0; ls < vm.listaResumen_prei.length; ls++) {
                            var resumenFinal = {
                                diferente: vm.listaResumen_prei[ls].diferente,
                                vigencia: vm.listaResumen_prei[ls].vigencia,
                                Costos: vm.listaResumen_prei[ls].prei_costos,
                                Ajuste: vm.listaResumen_prei[ls].prei_ajuste,
                            }
                            listafinal.push(resumenFinal);

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Preinversión",
                            Costos: resumenSubtotal_prei.subtotal_preiCostos,
                            Ajuste: resumenSubtotal_prei.subtotal_preiAjuste
                        }

                        mensaje = mensajePrei;
                        vm.mensaje = mensajePrei;

                        $('ul.tabsCostosAct > li').removeClass('active');
                        $('ul.tabsCostosAct > li:first-child').addClass('active');

                        break;
                    }
                case 2:
                    {
                        for (var ls = 0; ls < vm.listaResumen_inv.length; ls++) {
                            var resumenFinal = {
                                diferente: vm.listaResumen_inv[ls].diferente,
                                vigencia: vm.listaResumen_inv[ls].vigencia,
                                Costos: vm.listaResumen_inv[ls].inv_costos,
                                Ajuste: vm.listaResumen_inv[ls].inv_ajuste,
                            }
                            listafinal.push(resumenFinal);

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Inversión",
                            Costos: resumenSubtotal_inv.subtotal_invCostos,
                            Ajuste: resumenSubtotal_inv.subtotal_invAjuste
                        }

                        mensaje = mensajeInv;
                        vm.mensaje = mensajeInv;
                        break;
                    }
                case 3:
                    {

                        for (var ls = 0; ls < vm.listaResumen_ope.length; ls++) {
                            var resumenFinal = {
                                diferente: vm.listaResumen_ope[ls].diferente,
                                vigencia: vm.listaResumen_ope[ls].vigencia,
                                Costos: vm.listaResumen_ope[ls].ope_costos,
                                Ajuste: vm.listaResumen_ope[ls].ope_ajuste,
                            }
                            listafinal.push(resumenFinal);

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Operación",
                            Costos: resumenSubtotal_ope.subtotal_opeCostos,
                            Ajuste: resumenSubtotal_ope.subtotal_opeAjuste
                        }

                        mensaje = mensajeOpe;
                        vm.mensaje = mensajeOpe;
                        break;
                    }
            }

            vm.listaResumen = listafinal;
            vm.ResumenSubTotal = resumenSubtotalFinal;
            vm.mensajeCompleto = mensaje;
            if (mensaje !== "") {
                if (vm.mensajeCompleto.length > 79) {
                    vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
                } else {
                    vm.mensaje = vm.mensajeCompleto;
                }
                vm.mostrarFlujo = false;
            } else {
                $("#ver").html("");
            }
        }

        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo) {
                vm.mensaje = vm.mensajeCompleto;
                /* $("#ver").html(vm.mensajeCompleto );*/
            } else {
                vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
                /*  $("#ver").html(vm.mensajeCompleto.substring(0,78) + "...VER MAS");*/
            }
        }

        vm.notificacionValidacionPadre = function(handler) {
            obtenerCostosPIIPvsFuentesPIIP();
        }

        /**
         * Función validación Error = COST003
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarErrorActividades = function(codError, descErrores) {}

        /* --------------------------------- Notificación de Validaciones ---------------------------*/


    }

    angular.module('backbone').component('costosFuentesAjustes', {
        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ResumenCostosAjustes/costosFuentesAjustes.html",
        controller: costosFuentesAjustesController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&',
            validacionerrorfuentes: '&',
            validacionguardadopadre: '&',
            recargarresumen: '&'
        }
    });
})();