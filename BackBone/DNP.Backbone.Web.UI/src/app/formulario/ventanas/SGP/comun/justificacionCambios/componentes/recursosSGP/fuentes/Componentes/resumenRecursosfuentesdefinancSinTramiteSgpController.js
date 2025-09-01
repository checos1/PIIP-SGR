(function () {
    'use strict';

    resumenRecursosfuentesdefinancSinTramiteSgpController.$inject = [
        'resumenFuentesSinTramiteSgpServicio',
        '$sessionStorage',
        'constantesBackbone',
        '$scope',
    ];

    function resumenRecursosfuentesdefinancSinTramiteSgpController(
        resumenFuentesSinTramiteSgpServicio,
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
        vm.nombreComponente = "resumenRecursosfuentesdefinancSinTramiteSgp";

        vm.ConvertirNumero = ConvertirNumero;

        vm.listaResumen_inv = [];
        vm.listaResumen_prei = [];
        vm.listaResumen_ope = [];
        vm.listaResumen;
        let resumenSubtotal_ope = {};
        let resumenSubtotal_prei = {};
        let resumenSubtotal_inv = {};
        vm.mensaje = "";
        vm.verInversion = 0;
        vm.verOperacion = 0;
        vm.verPreinversion = 0;

        vm.init = function () {
            obtenerResumenFuentes();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre });
            vm.recargarresumen({ handler: vm.recargar });
        }

        vm.recargar = function () {
            //debugger
            obtenerResumenFuentes();
        }

        function ConvertirNumero(numero) {
            if (numero == undefined) {
                numero = 0;
            }
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.notificacionValidacionPadre = function (listadoerrores) {
            console.log('ACA ' + listadoerrores);
            if (listadoerrores != undefined) {
                listadoerrores.forEach(p => {
                    switch (p.Error) {
                        case "AFFR001":
                            vm.validarAFFR001(p.Descripcion);
                            break;
                        case "AFFR002":
                            vm.validarAFFR002(p.Descripcion);
                            break;
                        case "AFFR003":
                            vm.validarAFFR003(p.Descripcion);
                            break;
                        default:
                            console.log("Error Fuente de Financiacion no configurado");
                            break;
                    };
                });
            }
        }

        function obtenerResumenFuentes() {
            vm.listaResumen_inv = [];
            vm.listaResumen_prei = [];
            vm.listaResumen_ope = [];
            vm.listaResumen;
            return resumenFuentesSinTramiteSgpServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        let mensaje = "";

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosEtapas = arregloGeneral.Etapas;

                        let total_invSolicitados = 0,
                            total_invDecreto = 0,
                            total_invFirme = 0,
                            total_invAjuste = 0,
                            total_opeSolicitados = 0,
                            total_opeDecreto = 0,
                            total_opeFirme = 0,
                            total_opeAjuste = 0,
                            total_preiSolicitados = 0,
                            total_preiDecreto = 0,
                            total_preiFirme = 0,
                            total_preiAjuste = 0,
                            total_invDiferencia = 0,
                            total_opeDiferencia = 0,
                            total_preiDiferencia = 0

                        for (var ls = 0; ls < arregloDatosEtapas.length; ls++) {
                            switch (arregloDatosEtapas[ls].ETAPA) {
                                case 'Operación':
                                    {
                                        if (arregloDatosEtapas[ls].Resumen !== null) {
                                            var arregloDatosResumen = arregloDatosEtapas[ls].Resumen;
                                            for (var res = 0; res < arregloDatosResumen.length; res++) {

                                                if (arregloDatosResumen[res].valortotalactual > 0) {
                                                    var resumenOpe = {
                                                        icono: "",
                                                        vigencia: arregloDatosResumen[res].Vigencia,
                                                        ope_solicitado: arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos,
                                                        ope_decreto: arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion,
                                                        ope_firme: arregloDatosResumen[res].valortotalfirmeformato == null ? 0 : arregloDatosResumen[res].valortotalfirmeformato,
                                                        ope_ajuste: arregloDatosResumen[res].valortotalactualformato == null ? 0 : arregloDatosResumen[res].valortotalactualformato,
                                                        ope_diferencia: arregloDatosResumen[res].Diferenciaformato == null ? 0 : arregloDatosResumen[res].Diferenciaformato,
                                                    }

                                                    total_opeSolicitados += arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos;
                                                    total_opeDecreto += arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion;
                                                    total_opeFirme += arregloDatosResumen[res].valortotalfirme == null ? 0 : arregloDatosResumen[res].valortotalfirme;
                                                    total_opeAjuste += arregloDatosResumen[res].valortotalactual == null ? 0 : arregloDatosResumen[res].valortotalactual;
                                                    total_opeDiferencia += arregloDatosResumen[res].Diferencia == null ? 0 : arregloDatosResumen[res].Diferencia;
                                                    if (resumenOpe.ope_diferencia != 0) {
                                                        vm.listaResumen_ope.push(resumenOpe);
                                                    }
                                                }
                                            }
                                        }

                                        resumenSubtotal_ope = {
                                            icono: "=",
                                            tituloOperacion: "Total Operación",
                                            subtotal_opeSolicitado: total_opeSolicitados,
                                            subtotal_opeDecreto: total_opeDecreto,
                                            subtotal_opeFirme: total_opeFirme,
                                            subtotal_opeAjuste: total_opeAjuste,
                                            subtotal_opeDiferencia: total_opeDiferencia,
                                        }
                                        break;
                                    }
                                case 'Preinversión':
                                    {
                                        if (arregloDatosEtapas[ls].Resumen !== null) {
                                            var arregloDatosResumen = arregloDatosEtapas[ls].Resumen;
                                            for (var res = 0; res < arregloDatosResumen.length; res++) {
                                                if (arregloDatosResumen[res].valortotalactual > 0) {
                                                    var resumenPrei = {
                                                        icono: "",
                                                        vigencia: arregloDatosResumen[res].Vigencia,
                                                        prei_solicitado: arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos,
                                                        prei_decreto: arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion,
                                                        prei_firme: arregloDatosResumen[res].valortotalfirmeformato == null ? 0 : arregloDatosResumen[res].valortotalfirmeformato,
                                                        prei_ajuste: arregloDatosResumen[res].valortotalactualformato == null ? 0 : arregloDatosResumen[res].valortotalactualformato,
                                                        prei_diferencia: arregloDatosResumen[res].Diferenciaformato == null ? 0 : arregloDatosResumen[res].Diferenciaformato,
                                                    }

                                                    total_preiSolicitados += arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos;
                                                    total_preiDecreto += arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion;
                                                    total_preiFirme += arregloDatosResumen[res].valortotalfirme == null ? 0 : arregloDatosResumen[res].valortotalfirme;
                                                    total_preiAjuste += arregloDatosResumen[res].valortotalactual == null ? 0 : arregloDatosResumen[res].valortotalactual;
                                                    total_preiDiferencia += arregloDatosResumen[res].Diferencia == null ? 0 : arregloDatosResumen[res].Diferencia;
                                                    if (resumenPrei.prei_diferencia != 0) {
                                                        vm.listaResumen_prei.push(resumenPrei);
                                                    }
                                                }
                                            }
                                        }

                                        resumenSubtotal_prei = {
                                            icono: "=",
                                            tituloOperacion: "Total Operación",
                                            subtotal_preiSolicitado: total_preiSolicitados,
                                            subtotal_preiDecreto: total_preiDecreto,
                                            subtotal_preiFirme: total_preiFirme,
                                            subtotal_preiAjuste: total_preiAjuste,
                                            subtotal_preiDiferencia: total_preiDiferencia,
                                        }
                                        break;
                                    }
                                case 'Inversión':
                                    {
                                        if (arregloDatosEtapas[ls].Resumen !== null) {
                                            var arregloDatosResumen = arregloDatosEtapas[ls].Resumen;
                                            for (var res = 0; res < arregloDatosResumen.length; res++) {
                                                if (arregloDatosResumen[res].valortotalactual > 0) {
                                                    var resumenInv = {
                                                        icono: "",
                                                        vigencia: arregloDatosResumen[res].Vigencia,
                                                        inv_solicitado: arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos,
                                                        inv_decreto: arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion,
                                                        inv_firme: arregloDatosResumen[res].valortotalfirmeformato == null ? 0 : arregloDatosResumen[res].valortotalfirmeformato,
                                                        inv_ajuste: arregloDatosResumen[res].valortotalactualformato == null ? 0 : arregloDatosResumen[res].valortotalactualformato,
                                                        inv_diferencia: arregloDatosResumen[res].Diferenciaformato == null ? 0 : arregloDatosResumen[res].Diferenciaformato,
                                                    }

                                                    total_invSolicitados += arregloDatosResumen[res].SolicitadoGestionRecursos == null ? 0 : arregloDatosResumen[res].SolicitadoGestionRecursos;
                                                    total_invDecreto += arregloDatosResumen[res].InicialesDecretoLiquidacion == null ? 0 : arregloDatosResumen[res].InicialesDecretoLiquidacion;
                                                    total_invFirme += arregloDatosResumen[res].valortotalfirme == null ? 0 : arregloDatosResumen[res].valortotalfirme;
                                                    total_invAjuste += arregloDatosResumen[res].valortotalactual == null ? 0 : arregloDatosResumen[res].valortotalactual;
                                                    total_invDiferencia += arregloDatosResumen[res].Diferencia == null ? 0 : arregloDatosResumen[res].Diferencia;
                                                    if (resumenInv.inv_diferencia != 0) {
                                                        vm.listaResumen_inv.push(resumenInv);
                                                    }
                                                }
                                            }
                                        }

                                        resumenSubtotal_inv = {
                                            icono: "=",
                                            tituloInversion: "Total Inversión",
                                            subtotal_invSolicitado: total_invSolicitados,
                                            subtotal_invDecreto: total_invDecreto,
                                            subtotal_invFirme: total_invFirme,
                                            subtotal_invAjuste: total_invAjuste,
                                            subtotal_invDiferencia: total_invDiferencia,
                                        }
                                        break;
                                    }
                            }
                        }

                        var resumenTotal = {
                            icono: "=",
                            total_solicitado: total_invSolicitados + total_opeSolicitados + total_preiSolicitados, //+ totalInversionCostos + totalOperacionCostos),
                            total_decreto: total_invDecreto + total_opeDecreto + total_preiDecreto,
                            total_firme: total_invFirme + total_opeFirme + total_preiFirme,
                            total_ajuste: total_invAjuste + total_opeAjuste + total_preiAjuste,
                            total_diferencia: total_invDiferencia + total_opeDiferencia + total_preiDiferencia,
                        }

                        var tab = 0;
                        if (total_invDiferencia != 0) {
                            vm.verInversion = 1;
                            tab = 2;
                        }
                        if (total_opeDiferencia != 0) {
                            vm.verOperacion = 1;
                            tab = 3;
                        }
                        if (total_preiDiferencia != 0) {
                            vm.verPreinversion = 1;
                            tab = 1;
                        }

                        var valorProyecto = $sessionStorage.valorTotalProyectoEncabezado;

                        if ((total_invAjuste + total_opeAjuste + total_preiAjuste) !== valorProyecto) {
                            vm.existeDiferencia = false;
                        }

                        vm.listaTotal = resumenTotal;
                        vm.mostrarTab(tab);
                    } else {
                        toastr.error("No se encontraron datos para el Bpin a consultar...");
                    }
                });
        }

        vm.mostrarTab = function (origen) {
            let listafinal = [];
            let resumenSubtotalFinal = {};
            let mensaje = "";
            vm.mensaje = "";
            switch (origen) {
                case 1:
                    {
                        for (var ls = 0; ls < vm.listaResumen_prei.length; ls++) {
                            var resumenFinal = {
                                vigencia: vm.listaResumen_prei[ls].vigencia,
                                Solicitado: vm.listaResumen_prei[ls].prei_solicitado,
                                Decreto: vm.listaResumen_prei[ls].prei_decreto,
                                Firme: vm.listaResumen_prei[ls].prei_firme,
                                Ajuste: vm.listaResumen_prei[ls].prei_ajuste,
                                Diferencia: vm.listaResumen_prei[ls].prei_diferencia,
                            }
                            if (resumenFinal.Diferencia != 0) {
                                listafinal.push(resumenFinal);
                            }

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Preinversión",
                            Solicitado: resumenSubtotal_prei.subtotal_preiSolicitado,
                            Decreto: resumenSubtotal_prei.subtotal_preiDecreto,
                            Firme: resumenSubtotal_prei.subtotal_preiFirme,
                            Ajuste: resumenSubtotal_prei.subtotal_preiAjuste,
                            Diferencia: resumenSubtotal_prei.subtotal_preiDiferencia,
                        }

                        break;
                    }
                case 2:
                    {
                        for (var ls = 0; ls < vm.listaResumen_inv.length; ls++) {
                            var resumenFinal = {
                                vigencia: vm.listaResumen_inv[ls].vigencia,
                                Solicitado: vm.listaResumen_inv[ls].inv_solicitado,
                                Decreto: vm.listaResumen_inv[ls].inv_decreto,
                                Firme: vm.listaResumen_inv[ls].inv_firme,
                                Ajuste: vm.listaResumen_inv[ls].inv_ajuste,
                                Diferencia: vm.listaResumen_inv[ls].inv_diferencia,
                            }
                            if (resumenFinal.Diferencia != 0) {
                                listafinal.push(resumenFinal);
                            }

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Inversión",
                            Solicitado: resumenSubtotal_inv.subtotal_invSolicitado,
                            Decreto: resumenSubtotal_inv.subtotal_invDecreto,
                            Firme: resumenSubtotal_inv.subtotal_invFirme,
                            Ajuste: resumenSubtotal_inv.subtotal_invAjuste,
                            Diferencia: resumenSubtotal_inv.subtotal_invDiferencia
                        }
                        break;
                    }
                case 3:
                    {

                        for (var ls = 0; ls < vm.listaResumen_ope.length; ls++) {
                            var resumenFinal = {
                                vigencia: vm.listaResumen_ope[ls].vigencia,
                                Solicitado: vm.listaResumen_ope[ls].ope_solicitado,
                                Decreto: vm.listaResumen_ope[ls].ope_decreto,
                                Firme: vm.listaResumen_ope[ls].ope_firme,
                                Ajuste: vm.listaResumen_ope[ls].ope_ajuste,
                                Diferencia: vm.listaResumen_ope[ls].ope_diferencia,
                            }
                            if (resumenFinal.Diferencia != 0) {
                                listafinal.push(resumenFinal);
                            }

                        }

                        resumenSubtotalFinal = {
                            icono: "=",
                            titulo: "Total Operación",
                            Solicitado: resumenSubtotal_ope.subtotal_opeSolicitado,
                            Decreto: resumenSubtotal_ope.subtotal_opeDecreto,
                            Firme: resumenSubtotal_ope.subtotal_opeFirme,
                            Ajuste: resumenSubtotal_ope.subtotal_opeAjuste,
                            Diferencia: resumenSubtotal_ope.subtotal_opeDiferencia
                        }
                        break;
                    }
            }

            vm.listaResumen = listafinal;
            vm.ResumenSubTotal = resumenSubtotalFinal;
            vm.mensajeCompleto = mensaje;

            setTimeout(function () {
                var contenido = $('#divresumenrff');
                var altura = contenido.height();
                $('#resumenrff').height(altura + 200);
            }, 200
            );
        }

        function formatearNumero(n, sep, decimals) {
            sep = sep || ","; // Default to period as decimal separator
            decimals = decimals || 2; // Default to 2 decimals

            var unidad = n.toLocaleString().split(sep)[0];
            var decimal = n.toFixed == undefined || n.toFixed(decimals).split(".")[1] == undefined ? 0 : n.toFixed(decimals).split(".")[1];

            return unidad +
                sep +
                decimal;
        }

        vm.validarAFFR001 = function (errores) {
            var ValidacionFFR1 = document.getElementById(vm.nombreComponente + "-validacionffr1-error");
            if (ValidacionFFR1 != undefined) {
                ValidacionFFR1.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                ValidacionFFR1.classList.remove('hidden');
            }
        }

        vm.validarAFFR002 = function (errores) {
            var ValidacionFFR2 = document.getElementById(vm.nombreComponente + "-validacionffr2-error");
            if (ValidacionFFR2 != undefined) {
                ValidacionFFR2.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                ValidacionFFR2.classList.remove('hidden');
            }
        }

        vm.validarAFFR003 = function (errores) {
            var ValidacionFFR3 = document.getElementById(vm.nombreComponente + "-validacionffr3-error");
            if (ValidacionFFR3 != undefined) {
                ValidacionFFR3.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                ValidacionFFR3.classList.remove('hidden');
            }
        }

        vm.errores = {
            'AFFR001': vm.validarAFFR001,
            'AFFR002': vm.validarAFFR002,
            'AFFR003': vm.validarAFFR003,
        }

    }

    angular.module('backbone').component('resumenRecursosfuentesdefinancSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/fuentes/Componentes/resumenRecursosfuentesdefinancSinTramiteSgp.html",
        controller: resumenRecursosfuentesdefinancSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            disabled: '=',
            callback: '&',
            errores: '&',
            recargarresumen: '&'
        }
    });
})();