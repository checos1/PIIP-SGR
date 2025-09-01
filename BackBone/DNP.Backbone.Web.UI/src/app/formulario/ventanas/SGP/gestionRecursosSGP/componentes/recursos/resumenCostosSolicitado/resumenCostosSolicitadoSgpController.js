(function () {
    'use strict';

    resumenCostosSolicitadoSgpController.$inject = ['$scope', 'gestionRecursosSGPServicio', '$sessionStorage'];

    function resumenCostosSolicitadoSgpController(
        $scope,
        gestionRecursosSGPServicio,
        $sessionStorage
    ) {
        var vm = this;
        vm.lang = "es";
        vm.init = init;
        var listaResumen = [];
        vm.listaResumen;
        let resumenSubtotal = {};
        vm.mensaje = "";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        $sessionStorage.preInversionSgpActive = true;
        $sessionStorage.inversionSgpActive = true;
        $sessionStorage.operacionSgpActive = true;

        vm.etapaEdicion = "";

        function init() {

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.idetapa = 1;
            vm.obtenerResumen(vm.BPIN);
            vm.recargarresumen({ handler: vm.recargar });
        }

        vm.recargar = function () {
            $scope.$watch('vm.etapa', function () {

                vm.obtenerResumen(vm.BPIN);

            });

        }

        vm.obtenerResumen = function (bpin) {

            return gestionRecursosSGPServicio.obtenerResumenCostosVsSolicitadoSgp(bpin, usuarioDNP, "").then(
                function (respuesta) {

                    listaResumen = [];

                    if (respuesta.data != null && respuesta.data != "") {

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosValores = arregloGeneral.Valores;

                        let totalPreinversioCostos = 0,
                            totalPreinversionSolicitado = 0,
                            totalInversionCostos = 0,
                            totalInversionSolicitado = 0,
                            totalOperacionCostos = 0,
                            totalOperacionSolicitado = 0


                        for (var ls = 0; ls < arregloDatosValores.length; ls++) {

                            var resumen = {
                                icono: "",
                                Vigencia: arregloDatosValores[ls].Vigencia,
                                PreinversioCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].PreinversiónCostos),
                                PreinversionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].PreinversiónSolicitado),
                                InversionCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].InversiónCostos),
                                InversionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].InversiónSolicitado),
                                OperacionCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].OperaciónCostos),
                                OperacionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(arregloDatosValores[ls].OperaciónSolicitado),

                            }
                            totalPreinversioCostos += arregloDatosValores[ls].PreinversiónCostos;
                            totalPreinversionSolicitado += arregloDatosValores[ls].PreinversiónSolicitado;
                            totalInversionCostos += arregloDatosValores[ls].InversiónCostos;
                            totalInversionSolicitado += arregloDatosValores[ls].InversiónSolicitado;
                            totalOperacionCostos += arregloDatosValores[ls].OperaciónCostos
                            totalOperacionSolicitado += arregloDatosValores[ls].OperaciónSolicitado

                            listaResumen.push(resumen);
                        }                                               

                        resumenSubtotal = {
                            icono: "=",
                            tituloPreinversion: "Total Preinversión",
                            tituloInversion: "Total Inversión",
                            tituloOperacion: "Total Operación",
                            PreinversioCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalPreinversioCostos),
                            PreinversionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalPreinversionSolicitado),
                            InversionCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalInversionCostos),
                            InversionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalInversionSolicitado),
                            OperacionCostos: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalOperacionCostos),
                            OperacionSolicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalOperacionSolicitado),
                        }

                        let resumenTotal = {
                            icono: "=",
                            costo: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalPreinversioCostos + totalInversionCostos + totalOperacionCostos),
                            solicitado: new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(totalPreinversionSolicitado + totalInversionSolicitado + totalOperacionSolicitado),
                        }

                        vm.listaTotal = resumenTotal;

                        //Validar Tabs activas
                        vm.mostrarTab(1);
                        $("#liDgenerales").addClass('active');
                        $("#liRecursos").removeClass("active");
                        $('#lijustificacion').removeClass('active');

                        if (totalPreinversioCostos <= 0 && totalPreinversionSolicitado <= 0) {
                            $("#liDgenerales").removeClass("active");
                            $('#liDgenerales').addClass('disabled');
                            $('#liDgenerales').find('a').removeAttr('data-toggle');                            
                            /*$sessionStorage.setItem('preInversionSgpActive', false);*/
                            vm.mostrarTab(2);
                            $("#liRecursos").addClass('active');
                            $sessionStorage.preInversionSgpActive = false;
                        }

                        if (totalInversionCostos <= 0 && totalInversionSolicitado <= 0) {
                            $("#liRecursos").removeClass("active");
                            $('#liRecursos').addClass('disabled');
                            $('#liRecursos').find('a').removeAttr('data-toggle');
                            //$sessionStorage.setItem('inversionSgpActive', false);
                            vm.mostrarTab(3);
                            $('#lijustificacion').addClass('active');
                            $sessionStorage.inversionSgpActive = false;
                        }

                        if (totalOperacionCostos <= 0 && totalOperacionSolicitado <= 0) {
                            $('#lijustificacion').removeClass('active');
                            $('#lijustificacion').addClass('disabled');
                            $('#lijustificacion').find('a').removeAttr('data-toggle');
                            //$sessionStorage.setItem('operacionSgpActive', false);
                            $sessionStorage.operacionSgpActive = false;
                        }
                        
                        let idSpanAlertComponent = document.getElementById("alert-Preinversion");

                        if (listaResumen.filter(c => c.PreinversioCostos !== c.PreinversionSolicitado).length > 0)
                        {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                        else
                        {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }
                        idSpanAlertComponent = document.getElementById("alert-inversion");
                        if (listaResumen.filter(c => c.InversionCostos !== c.InversionSolicitado).length > 0)
                        {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                        else
                        {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }
                        idSpanAlertComponent = document.getElementById("alert-operacion");
                        if (listaResumen.filter(c => c.OperacionCostos !== c.OperacionSolicitado).length > 0)
                        {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                        else
                        {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }
                    }
                }
            );

        };


        vm.mostrarTab = function (origen) {
            let listafinal = [];
            let resumenSubtotalFinal = {};
            let mensaje = "";
            vm.mensaje = "";

            switch (origen) {
                case 1:
                    {
                        for (let ls = 0; ls < listaResumen.length; ls++)
                        {
                            let resumenFinal = {
                                vigencia: listaResumen[ls].Vigencia,
                                Costos: listaResumen[ls].PreinversioCostos,
                                Solicitado: listaResumen[ls].PreinversionSolicitado
                            }
                            listafinal.push(resumenFinal);
                            if (listaResumen[ls].PreinversioCostos !== listaResumen[ls].PreinversionSolicitado)
                            {
                                mensaje += "En vigencia " + listaResumen[ls].Vigencia + " - ajuste no coinciden los valores. ";
                            }
                        }

                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Preinversión",
                            Costos: resumenSubtotal.PreinversioCostos,
                            Solicitado: resumenSubtotal.PreinversionSolicitado
                        }
                        break;
                    }
                case 2:
                    {
                        for (var ls = 0; ls < listaResumen.length; ls++)
                        {
                            var resumenFinal = {
                                icono: "",
                                vigencia: listaResumen[ls].Vigencia,
                                Costos: listaResumen[ls].InversionCostos,
                                Solicitado: listaResumen[ls].InversionSolicitado
                            }
                            listafinal.push(resumenFinal);
                            if (listaResumen[ls].InversionCostos !== listaResumen[ls].InversionSolicitado)
                            {
                                mensaje += "En vigencia " + listaResumen[ls].Vigencia + " - ajuste no coinciden los valores. ";
                            }
                        }
                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Inversión",
                            Costos: resumenSubtotal.InversionCostos,
                            Solicitado: resumenSubtotal.InversionSolicitado
                        }
                        break;
                    }
                case 3:
                    {

                        for (var ls = 0; ls < listaResumen.length; ls++) {
                            var resumenFinal = {

                                vigencia: listaResumen[ls].Vigencia,
                                Costos: listaResumen[ls].OperacionCostos,
                                Solicitado: listaResumen[ls].OperacionSolicitado
                            }
                            listafinal.push(resumenFinal);
                            if (listaResumen[ls].OperacionCostos !== listaResumen[ls].OperacionSolicitado)
                            {
                                mensaje += "En vigencia " + listaResumen[ls].Vigencia + " - ajuste no coinciden los valores. ";
                            }
                        }
                        resumenSubtotalFinal =
                        {
                            icono: "=",
                            titulo: "Total Operación",
                            Costos: resumenSubtotal.OperacionCostos,
                            Solicitado: resumenSubtotal.OperacionSolicitado
                        }
                        break;
                    }
            }

            vm.listaResumen = listafinal;
            vm.ResumenSubTotal = resumenSubtotalFinal;
            vm.mensajeCompleto = mensaje;
            if (mensaje !== "") {
                if (vm.mensajeCompleto.length > 79)
                {
                    vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
                }
                else
                {
                    vm.mensaje = vm.mensajeCompleto;
                }
                vm.mostrarFlujo = false;
            }
            else
            {
                vm.mensaje = "";
            }
        }

        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo)
            {
                vm.mensaje = vm.mensajeCompleto;
            }
            else
            {
                vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
            }
        }
    }

    angular.module('backbone').component('resumenCostosSolicitadoSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/resumenCostosSolicitado/resumenCostosSolicitadoSgp.html",
        controller: resumenCostosSolicitadoSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            recargarresumen: '&',
            etapa: '@'
        }
    });

})();