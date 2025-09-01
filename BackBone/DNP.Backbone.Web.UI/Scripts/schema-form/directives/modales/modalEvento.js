(function () {
    'use strict'

    angular.module('backbone')
        .controller('modalEventoController', modalEventoController);

    modalEventoController.$inject = ['$sce', '$scope', '$filter' , '$uibModalInstance', '$log', 'ejecutorReglasServicios', 'eventoCustom', 'formulario'];

    function modalEventoController($sce, $scope, $filter, $uibModalInstance, $log, ejecutorReglasServicios, eventoCustom, formulario) {
        var vm = this;
        vm.idPantalla = $filter('language')('IdFromCfgSwagger');
        vm.url = $sce.trustAsResourceUrl(eventoCustom.Url);
        vm.aceptar = aceptar;
        vm.cancelar = cancelar;
        vm.tituloModalEvento = "Modal Evento";
       

        vm.esCampoVisible = esCampoVisible;
        vm.controlarError = controlarError;
        vm.ocultarCampos = ocultarCampos;
        vm.mostrarCampos = mostrarCampos;
        vm.establecerValorEnCampos = establecerValorEnCampos;
        vm.obtenerValor = obtenerValor;
        vm.evaluar = evaluar;
        vm.ejecutarRegla = ejecutarRegla;

        if ( formulario.form != null ) {
            vm.form = JSON.parse(formulario.form);
            vm.schema = JSON.parse(formulario.schema);
            vm.model = JSON.parse(formulario.model);
            vm.NombreFormulario = formulario.nombre;
            vm.formEncabezado = formulario.formEncabezado;
            vm.schemaEncabezado = formulario.schemaEncabezado;
            vm.modelEncabezado = formulario.modelEncabezado;
            vm.formPie = formulario.formPie;
            vm.schemaPie = formulario.schemaPie;
            vm.modelPie = formulario.modelPie;
        }

        function aceptar() {
            vm.submittedCondiciones = true;

            if (vm.FormCondicion.$valid) {
                /* var condicion = armarEvento();
 
                 $uibModalInstance.close(condicion);*/
            }
        }

        function cancelar() {
            $uibModalInstance.close(false);
        }


        //Manejo de formulario

        function esCampoVisible(campo) {
            var visible = true;
            if (campo) {
                var objeto = ejecutorReglasServicios.obtenerCampoDelFormulario(vm.form, campo);
                if (objeto) {

                    if (objeto.buttonTypeCustom && objeto.condicion && Object.getOwnPropertyNames(vm.model).length > 0 && Object.getOwnPropertyNames(objeto.condicion).length > 0) {
                        Object.getOwnPropertyNames(vm.model).forEach(function (val, idx, array) {
                            if (val === objeto.condicion.expresion1.valor) {
                                visible = evaluar(vm.model[val], objeto.condicion.operador.tipo, objeto.condicion.expresion2.valor);
                                return visible;
                            }
                        });
                    }
                    else
                        return objeto.visible;
                }
            }
            return visible;
        }

        function controlarError(error) {
            ejecutorReglasServicios.controlarError(error);
        }

        function ocultarCampos(campos) {
            vm.form = ejecutorReglasServicios.ocultarCampos(vm.form, campos);
        }

        function mostrarCampos(campos) {
            vm.form = ejecutorReglasServicios.mostrarCampos(vm.form, campos);
        }

        function establecerValorEnCampos(campos, valor) {
            vm.model = ejecutorReglasServicios.establecerValorEnCampos(vm.model, campos, valor);
        }

        function obtenerValor(valor, formato) {
            return ejecutorReglasServicios.obtenerValor(vm.model, valor, formato);
        }

        function evaluar(expresion1, operador, expresion2) {
            return ejecutorReglasServicios.evaluar(expresion1, operador, expresion2);
        }

        function ejecutarRegla(newValue, oldValue) {
            if (vm.schema && vm.schema.reglas)
                eval(vm.schema.reglas);
        }

        //
    }
})();