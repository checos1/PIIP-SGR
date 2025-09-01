(function () {
    'use strict';

    resumenInformacionPresupuestalController.$inject = [
        '$scope',
        'justificacionCambiosServicio',
        'tramiteVigenciaFuturaServicio',
        '$sessionStorage',
        'utilidades',
        'constantesBackbone'
    ];

    function resumenInformacionPresupuestalController(
        $scope,
        justificacionCambiosServicio,
        tramiteVigenciaFuturaServicio,
        $sessionStorage,
        utilidades,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.nombreComponente = "informacionpresupuestalresumen";
        vm.mensajetabla = "El sistema no ha actualizado los datos de esta tabla desde el formulario de ajustes.";
        vm.mensajeEncabezado = "Vigencias futuras con valores constantes";

        vm.TramiteId = $sessionStorage.TramiteId;
        vm.informacionPresupuestal = {};
        vm.disabled = true;
        vm.EsConstante = true;

        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;

        vm.init = function () {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.obtenerInformacionPresupuestal(vm.tramiteid);
                }
            });            
        };

        vm.obtenerInformacionPresupuestal = function (TramiteId) {         

            tramiteVigenciaFuturaServicio.ObtenerInformacionPresupuestalValores(TramiteId).then(
                function (response) {
                    if (response.data != null && response.data != "") {

                        vm.EsConstante = response.data.AplicaConstante;

                        if (vm.EsConstante) {
                            vm.mensajeEncabezado = "Vigencias futuras con valores constantes";
                        }
                        else {
                            vm.mensajeEncabezado = "Vigencias futuras con valores corrientes";
                        }

                        if (response.data.ResumensolicitadoFuentesVigenciaFutura != null && response.data.DetalleProductosVigenciaFutura != null) {

                            vm.mensajetabla = "El sistema ha actualizado los datos de esta tabla. Desde ajustes se incluyeron los datos";

                            var infoPresupuestal = {};

                            var listaVigencias = [];                           

                            var sumNacionConstante = 0;
                            var sumPropiosConstante = 0;
                            var sumNacionCorriente = 0;
                            var sumPropiosCorriente = 0;

                            response.data.ResumensolicitadoFuentesVigenciaFutura.forEach(vigencia => {

                                listaVigencias.push({
                                    Vigencia: vigencia.Vigencia,
                                    Deflactor: vigencia.Deflactor,
                                    ValorFuentesNacion: vigencia.ValorFuentesNacion,
                                    ValorFuentesPropios: vigencia.ValorFuentesPropios,
                                    NacionCorriente: vigencia.ValorFuentesNacion * (vm.EsConstante ? vigencia.Deflactor : 1),
                                    PropiosCorriente: vigencia.ValorFuentesPropios * (vm.EsConstante ? vigencia.Deflactor : 1)
                                });

                                sumNacionConstante = sumNacionConstante + vigencia.ValorFuentesNacion;
                                sumPropiosConstante = sumPropiosConstante + vigencia.ValorFuentesPropios;
                                sumNacionCorriente = sumNacionCorriente + (vigencia.ValorFuentesNacion * (vm.EsConstante ? vigencia.Deflactor : 1));
                                sumPropiosCorriente = sumPropiosCorriente + (vigencia.ValorFuentesPropios * (vm.EsConstante ? vigencia.Deflactor : 1));
                            });

                            listaVigencias.push({
                                Vigencia: 'Total',
                                Deflactor: 'N/A',
                                ValorFuentesNacion: sumNacionConstante,
                                ValorFuentesPropios: sumPropiosConstante,
                                NacionCorriente: sumNacionCorriente,
                                PropiosCorriente: sumPropiosCorriente
                            });

                            var listaObjetivos = [];

                            var numObjetivo = 1;
                            response.data.DetalleProductosVigenciaFutura.forEach(objetivo => {

                                var listaProductos = [];

                                var numProducto = 1;
                                var conteoNumProducto = numObjetivo + '.' + numProducto

                                objetivo.Productos.forEach(producto => {
                                    var listaValores = [];
                                    var sumValorSolicitado = 0;
                                    var sumValorSolicitadoCorriente = 0;

                                    producto.Vigencias.forEach(valor => {

                                        listaValores.push({
                                            Vigencia: valor.Vigencia,
                                            ValorSolicitado: valor.ValorSolicitadoVF,
                                            ValorSolicitadoCorriente: valor.ValorSolicitadoVF * (vm.EsConstante ? valor.Deflactor : 1),
                                        })

                                        sumValorSolicitado = sumValorSolicitado + valor.ValorSolicitadoVF;
                                        sumValorSolicitadoCorriente = sumValorSolicitadoCorriente + (valor.ValorSolicitadoVF * (vm.EsConstante ? valor.Deflactor : 1));
                                    });

                                    listaValores.push({
                                        Vigencia: 'Total por valores',
                                        ValorSolicitado: sumValorSolicitado,
                                        ValorSolicitadoCorriente: sumValorSolicitadoCorriente
                                    })

                                    if (producto.TotalValores > 0) {
                                        listaProductos.push({
                                            ProductoId: producto.ProductoId,
                                            //NumeroProducto: 'Producto ' + numProducto + '.',
                                            NumeroProducto: 'Producto ' + conteoNumProducto + '.',
                                            NombreProducto: producto.NombreProducto,
                                            TotalValores: producto.TotalValores,
                                            TotalValoresCorrientes: sumValorSolicitadoCorriente,
                                            labelEncabezado: vm.mensajeEncabezado,
                                            Vigencias: listaValores
                                        });

                                        numProducto++;
                                    }
                                });                                

                                if (listaProductos.length > 0) {
                                    listaObjetivos.push({
                                        ObjetivoEspecificoId: objetivo.ObjetivoEspecificoId,
                                        NumeroObjetivo: 'Objetivo ' + numObjetivo + '.',
                                        ObjetivoEspecifico: objetivo.ObjetivoEspecifico,
                                        Productos: listaProductos
                                    });

                                    numObjetivo++;
                                }
                            });                            

                            infoPresupuestal.ProyectoId = response.data.ProyectoId;
                            infoPresupuestal.AnoBase = response.data.AñoBase,
                            infoPresupuestal.labeltable = vm.mensajetabla,
                            infoPresupuestal.ResumensolicitadoFuentesVigenciaFutura = listaVigencias;
                            infoPresupuestal.DetalleProductosVigenciaFutura = listaObjetivos;

                            vm.informacionPresupuestal = infoPresupuestal;
                        }
                        else {
                            vm.informacionPresupuestal = {
                                ProyectoId: '-',
                                AnoBase: '-',
                                labeltable: vm.mensajetabla,
                                ResumensolicitadoFuentesVigenciaFutura: [                                    
                                    {
                                        Vigencia: '-',
                                        Deflactor: 'Dato pendiente',
                                        ValorFuentesNacion: 'Dato pendiente',
                                        ValorFuentesPropios: 'Dato pendiente',
                                        NacionCorriente: 'Dato pendiente',
                                        PropiosCorriente: 'Dato pendiente'
                                    },
                                    {
                                        Vigencia: 'Total',
                                        Deflactor: 'N/A',
                                        ValorFuentesNacion: 'Dato pendiente',
                                        ValorFuentesPropios: 'Dato pendiente',
                                        NacionCorriente: 'Dato pendiente',
                                        PropiosCorriente: 'Dato pendiente'
                                    }
                                ]                               
                            };
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError('No fue posible consultar la información presupuestal');
                });           
        }

        vm.BtnObjetivos = function (campo) {
            var variable = $("#img" + campo)[0].outerHTML;
            if (variable.includes('Img/btnMas.svg')) {
                $("#img" + campo).attr('src', 'Img/btnMenos.svg');
            }
            else {
                $("#img" + campo).attr('src', 'Img/btnMas.svg');
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }
    }

    angular.module('backbone').component('resumenInformacionPresupuestal', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/resumenInformacionPresupuestal/resumenInformacionPresupuestal.html",
        controller: resumenInformacionPresupuestalController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
        }
    });

})();