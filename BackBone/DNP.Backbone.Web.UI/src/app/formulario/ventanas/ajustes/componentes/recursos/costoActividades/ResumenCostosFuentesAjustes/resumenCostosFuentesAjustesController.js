(function () {
    'use strict';

    resumenCostosFuentesAjustesController.$inject = [
        'costosFuentesAjustesServicio',
        '$sessionStorage',
        'constantesBackbone',
        '$scope',
    ];

    function resumenCostosFuentesAjustesController(
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

        vm.mostrarMensaje = false
        vm.listaResumen_inv = [];
        vm.listaResumen_prei = [];
        vm.listaResumen_ope = [];
        vm.listaResumen;
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
        };

        vm.pagina = 1;


        vm.inicializar = function () {
            vm.validacionerrorfuentes({ handler: vm.notificacionValidacionPadre});
            vm.refrescar({ handler: vm.notificacionValidacionPadre });

            vm.listaResumen = [];
            vm.listaResumen_inv = [];
            vm.listaResumen_prei = [];
            vm.listaResumen_ope = [];

            obtenerCostosPIIPvsFuentesPIIP();
        }

        function ConvertirNumero(numero) {

            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.diferencia = function (valor) {
            return valor;
        }

        function obtenerCostosPIIPvsFuentesPIIP() {

            vm.listaResumen = [];
            vm.listaResumen_inv = [];
            vm.listaResumen_prei = [];
            vm.listaResumen_ope = [];

            mensajeInv = '';
            mensajeOpe = '';
            mensajePrei = '';

            return costosFuentesAjustesServicio.obtenerCostosPIIPvsFuentesPIIP($sessionStorage.idInstancia, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    //var respuesta = "{\"BPIN\":\"202100000000014\",\"Etapas\":[{\"Etapa\":\"Inversión\",\"Valores\":[{\"Vigencia\":2021,\"Valorcosto\":4007844000000.00,\"ValorFuentes\":1320000000000.00},{\"Vigencia\":2022,\"Valorcosto\":1740000000000.00,\"ValorFuentes\":1740000000000.00},{\"Vigencia\":2023,\"Valorcosto\":5528844000000.00,\"ValorFuentes\":1830000000000.00},{\"Vigencia\":2024,\"Valorcosto\":1658844000000.00,\"ValorFuentes\":540000000000.00}]},{\"Etapa\":\"Operación\",\"Valores\":[{\"Vigencia\":2021,\"Valorcosto\":0.00,\"ValorFuentes\":1320000000000.00},{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":1740000000000.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":1830000000000.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":540000000000.00}]},{\"Etapa\":\"Preinversión\",\"Valores\":[{\"Vigencia\":2021,\"Valorcosto\":0.00,\"ValorFuentes\":1320000000000.00},{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":1740000000000.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":1830000000000.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":540000000000.00}]}]}";
                    if (respuesta.data != null && respuesta.data != "") {
                    //if (respuesta != null && respuesta != "") {
                        let mensaje = "";
                        var isError = false;

                        vm.listaResumen = [];
                        vm.listaResumen_inv = [];
                        vm.listaResumen_prei = [];
                        vm.listaResumen_ope = [];

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosEtapas = arregloGeneral.Etapas;
                        //var arregloDatosEtapas = arreglolistas.Etapas;

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
                                        esDiferente = false;
                                        isError = false;
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {
                                            if (arregloDatosResumen[res].Vigencia >= currentYear && arregloDatosResumen[res].Valorcosto !== arregloDatosResumen[res].ValorFuentes) {
                                                mensajeOpe += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                                isError = true;
                                            }                                            
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

                                        vm.errores.operacion = isError
                                        if (mensajeOpe) {
                                            mensajeOpe = 'En vigencia: ' + mensajeOpe.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capítulo de fuentes de financiación para ajustar la información.';
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
                                        esDiferente = false;
                                        isError = false;
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {
                                            if (arregloDatosResumen[res].Vigencia >= currentYear && arregloDatosResumen[res].Valorcosto !== arregloDatosResumen[res].ValorFuentes) {
                                                mensajePrei += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                                isError = true;
                                            }
                                            
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
                                        vm.errores.preinversion = isError
                                        if (mensajePrei) {
                                            mensajePrei = 'En vigencia: ' + mensajePrei.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capítulo de fuentes de financiación para ajustar la información.';
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
                                        esDiferente = false;
                                        isError = false;
                                        var arregloDatosResumen = arregloDatosEtapas[ls].Valores;
                                        for (var res = 0; res < arregloDatosResumen.length; res++) {

                                            if (arregloDatosResumen[res].Vigencia >= currentYear && parseFloat(arregloDatosResumen[res].Valorcosto) !== parseFloat(arregloDatosResumen[res].ValorFuentes)) {
                                                mensajeInv += ', ' + arregloDatosResumen[res].Vigencia;
                                                esDiferente = true;
                                                isError = true;
                                            }
                                          
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
                                        vm.errores.inversion = isError
                                        if (mensajeInv) {
                                            mensajeInv = 'En vigencia: ' + mensajeInv.substr(1) + ', el valor de las fuentes de financiación no coinciden con los costos de las actividades. Es posible que deba ir al capítulo de fuentes de financiación para ajustar la información.';
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

        vm.mostrarTab = function (origen) {

            vm.pagina = origen;

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

                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Preinversión",
                            Costos: resumenSubtotal_prei.subtotal_preiCostos,
                            Ajuste: resumenSubtotal_prei.subtotal_preiAjuste
                        }

                        mensaje = mensajePrei;
                        vm.mensaje = mensajePrei;
                        vm.mostrarMensaje = vm.errores.preinversion;
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

                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Inversión",
                            Costos: resumenSubtotal_inv.subtotal_invCostos,
                            Ajuste: resumenSubtotal_inv.subtotal_invAjuste
                        }

                        mensaje = mensajeInv;
                        vm.mensaje = mensajeInv;
                        vm.mostrarMensaje = vm.errores.inversion;
                        break;
                    }
                case 3:
                    {

                        for (var ls = 0; ls < vm.listaResumen_ope.length; ls++) {
                            var resumenFinal = {
                                diferente: vm.listaResumen_prei[ls].diferente,
                                vigencia: vm.listaResumen_ope[ls].vigencia,
                                Costos: vm.listaResumen_ope[ls].ope_costos,
                                Ajuste: vm.listaResumen_ope[ls].ope_ajuste,
                            }
                            listafinal.push(resumenFinal);
                            
                        }

                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Operación",
                            Costos: resumenSubtotal_ope.subtotal_opeCostos,
                            Ajuste: resumenSubtotal_ope.subtotal_opeAjuste
                        }

                        mensaje = mensajeOpe;
                        vm.mensaje = mensajeOpe;
                        vm.mostrarMensaje = vm.errores.operacion
                        break;
                    }
            }

            vm.listaResumen = listafinal;
            vm.ResumenSubTotal = resumenSubtotalFinal;
            vm.mensajeCompleto = mensaje;
            if (mensaje !== "") {
                if (vm.mensajeCompleto.length > 79) {
                    vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
                }
                else {
                    vm.mensaje = vm.mensajeCompleto;
                }
                vm.mostrarFlujo = false;
            }
            else {
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
            }
            else {
                vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
                /*  $("#ver").html(vm.mensajeCompleto.substring(0,78) + "...VER MAS");*/
            }
        }

        vm.notificacionValidacionPadre = function (handler) {
            obtenerCostosPIIPvsFuentesPIIP();
        }

        /**
         * Función validación Error = COST003
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarErrorActividades = function (codError, descErrores) {
        }

        /* --------------------------------- Notificación de Validaciones ---------------------------*/

        
    }

    angular.module('backbone').component('resumenCostosFuentesAjustes', {
        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/ResumenCostosFuentesAjustes/resumenCostosFuentesAjustes.html",
        controller: resumenCostosFuentesAjustesController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&',
            validacionerrorfuentes: '&',
            refrescar: '&'
        }
    });
})();