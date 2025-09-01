/// <reference path="../../../model/AlertaMonitoreoConfigModel.js" />
/// <reference path="../../../model/AlertaMonitoreoReglaModel.js" />
/// <reference path="../../../model/Constantes/CondicionalConstante.js" />
/// <reference path="../../../model/Constantes/OperadorConstante.js" />
/// <reference path="../../../model/Constantes/TipoAlertaConstante.js" />
/// <reference path="../../../model/Constantes/TipoValorColumnaConstante.js" />

(function () {
    'use strict';

    angular.module('backbone')
        .controller('crearAlertaConfiguracionController', crearAlertaConfiguracionController);

    crearAlertaConfiguracionController.$inject = [
        '$scope', 
        '$uibModalInstance',
        'servicioConsolaAlertas',
        'AlertaMonitoreoConfigModel',
        'AlertaMonitoreoReglaModel',
        'CondicionalConstante',
        'OperadorConstante',
        'TipoValorColumnaConstante',
        'TipoAlertaConstante',
        'ClassificacionAlertaConstante',
        'idAlerta',
        'peticion',
        'uiMaskConfig',
        'TipoColumnaConstante',
        'listarGrid',
        '$timeout'
    ];

    function crearAlertaConfiguracionController(
        $scope,
        $uibModalInstance,
        servicioConsolaAlertas,
        AlertaMonitoreoConfigModel,
        AlertaMonitoreoReglaModel,
        CondicionalConstante,
        OperadorConstante,
        TipoValorColumnaConstante,
        TipoAlertaConstante,
        ClassificacionAlertaConstante,
        idAlerta,
        peticion,
        uiMaskConfig,
        TipoColumnaConstante,
        listarGrid,
        $timeout
    ) {
        const vm = this;

        //#region Variables
        const model = new AlertaMonitoreoConfigModel();
        const tipoValor = TipoValorColumnaConstante;
        const tiposColumnas = TipoColumnaConstante
        const condicionales = CondicionalConstante;
        const operadores = OperadorConstante;
        const tiposAlertas = [];
        const classificacionAlerta = ClassificacionAlertaConstante;
        
        const _validadoresTipos = {
            [tiposColumnas.Int]:      (regla) => Number.isInteger(+regla.Valor),
            [tiposColumnas.String]:   (regla) => !!regla.Valor,
            [tiposColumnas.Datetime]: (regla) => new moment(regla.Valor).isValid(),
            [tiposColumnas.Date]:     (regla) => new moment(regla.Valor).isValid(),
            [tiposColumnas.Money]:    (regla) => {
                if(!/(\d)\.\d{2}/gi.test(regla.Valor))
                    regla.Valor = regla.Valor.replace(/(\d$)/gi, "$1.00");
                
                return new RegExp(/^\d{1,3}(?:\,\d{3})*.\d{2}$/).test(regla.Valor)
            },
        }

        const _mascaras = {
            [tiposColumnas.Datetime]: "mask-datetime",
            [tiposColumnas.Date]: "mask-datetime",
            [tiposColumnas.Money]: "mask-currency",
            [tiposColumnas.Int]: "mask-numeric",
            [tiposColumnas.String]: "mask-none"
        }

        const _validadores = [
            (item) => ([{ invalido: !item.NombreAlerta, mensaje: "Ingrese un nombre para la alerta"}]),
            (item) => ([{ invalido: !item.TipoAlerta, mensaje: 'Seleccione el tipo de alerta' }]),
            (item) => ([{ invalido: !item.MensajeAlerta, mensaje: 'Ingrese un mensaje para la alerta' }]),
            (item) => ([{ invalido: !item.Classificacion, mensaje: 'Seleccione la clasificación de la alerta' }]),
            (item) => {
                return item.AlertasReglasDtos.reduce((acc, regla, index) => {
                    const numero = ++index;
                    const columnaUno = vm.columnas.find(x => x.Id == regla.MapColumnasUnoId);

                    acc.push(...[
                        { 
                            invalido: !regla.Condicional, 
                            mensaje: `Ingrese un operador lógico para el condicional número ${numero}` 
                        },
                        {
                            invalido: regla.ActivarValor && !regla.Valor,
                            mensaje: `Ingrese un valor para el condicional número ${numero}`
                        },
                        {
                            invalido: !regla.MapColumnasUnoId,
                            mensaje: `Seleccione una columna para el condicional número ${numero}`
                        },
                        {
                            invalido: regla.ActivarColunaDos && !regla.MapColumnasDosId,
                            mensaje: `Seleccione la segunda columna para el condicional número ${numero}`
                        },
                        {
                            invalido: regla.ActivarValor && columnaUno && !_validadoresTipos[columnaUno.TipoColumna](regla),
                            mensaje: `Ingrese un valor válido para el tipo de columna seleccionado en condicional número ${numero}`
                        },
                        {
                            invalido: regla.Condicional && !vm.obtenerCondicionalesPorTipoColumna(regla.MapColumnasUnoId).map(x => x.valor).includes(regla.Condicional),
                            mensaje: `El operador seleccionado para el tipo de columna no es válido en condicional número ${numero}`
                        }
                    ])

                    return acc;
                }, [])
            }
        ];

        const _condicionalesNumericas = [1, 2, 3, 4, 5, 6];
        const _condicionalesTexto = [1, 6, 7];

        const tipoColumnasCondicionales = {
            [tiposColumnas.Int]:      _condicionalesNumericas,
            [tiposColumnas.String]:   _condicionalesTexto,
            [tiposColumnas.Datetime]: _condicionalesNumericas,
            [tiposColumnas.Date]:     _condicionalesNumericas,
            [tiposColumnas.Money]:    _condicionalesNumericas
        }

        vm.tipoValor = tipoValor;
        vm.tiposColumnas = tiposColumnas;
        vm.tiposAlertas = tiposAlertas;
        vm.condicionales = condicionales;
        vm.columnas = [];
        vm.operadores = operadores;
        vm.classificacionAlerta = classificacionAlerta;
        vm.nMascara = {};
        vm.tipoColumnasCondicionales = tipoColumnasCondicionales
        vm.model = model;  
              
        //#endregion

        //#region Metodos

        function cambiarMascara(index, idColumna, tipoColumna){
            if(tipoColumna){
                vm.nMascara[index] = _mascaras[tipoColumna];
                return;
            }
            
            if(idColumna && idColumna)
                tipoColumna = vm.columnas.filter((c) => c.Id == idColumna)[0].TipoColumna;

            vm.model.AlertasReglasDtos[index].Valor = null;
            vm.nMascara[index] = _mascaras[tipoColumna] || "";
            vm.cambiarTipoColumna(index);  
        }

        function cambiarTipoColumna(index) {
            const regla = vm.model.AlertasReglasDtos[index];
            const columnaUno = vm.columnas.find(x => x.Id == regla.MapColumnasUnoId);
            regla.MapColumnasUno = columnaUno;

            if(!regla.MapColumnasDosId)
                return;

            const columnaDos = vm.columnas.find(x => x.Id == regla.MapColumnasDosId);
            regla.MapColumnasDos = columnaDos;

            if(columnaUno.TipoColumna == columnaDos.TipoColumna)
                return;

            toastr.warning("El tipo de datos de la columna uno debe ser el mismo que el tipo de datos de la columna dos");
            regla.MapColumnasUnoId = null;
            regla.MapColumnasDosId = null;
        }

        function adicionarRegla() {
            vm.model.AlertasReglasDtos.push(new AlertaMonitoreoReglaModel({
                Operador: 1,
                TipoValor: 1
            }));
        }

        function removerRegla(regla, index) {
            
            if(index == null || index == undefined)
                return;

            if (vm.model.AlertasReglasDtos.length == 1) {
                toastr.error("La alerta debe tener al menos un condicional");
                return;
            }
            
            vm.model.AlertasReglasDtos.splice(index, 1);
            return;
        }

        function _validarAlerta(model) {
            try
            {
                const mensajes = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if(resultado.invalido)
                            mensajes.push(resultado.mensaje);
                    });  
                })

                if(mensajes.length)
                    _mostarToast(mensajes);

                return !mensajes.length;
            }
            catch(e) {
                console.log(e);
                toastr.error("Error inesperado al validar la alerta");
                return false;
            }
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
              if (!message)
                return;
        
              toastr.warning(message);
            })
        }

        function guardar(){
            const valido = _validarAlerta(vm.model);
            if(!valido)
                return;
            
            servicioConsolaAlertas.guardarAlertaConfig(peticion, vm.model).then(exito, error);

            function exito(respuesta) {
                listarGrid()
                toastr.success("Alerta guardada con éxito");
                $uibModalInstance.close(vm.items);
            }

            function error(respuesta) {
                if(respuesta.status == 400){
                    const { Data } = respuesta.data;
                        _mostarToast(Data.length && Data || ["Error inesperado al validar la alerta"]);
                    return; 
                }
                toastr.error("Error inesperado al validar la alerta");
            }
        }

        function cerrar() {
            $uibModalInstance.dismiss('cerrar');
        }

        function listarTipoAlerta() {
            servicioConsolaAlertas.obtenerListaTipoAlerta(peticion).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    //TODO: Filtro temporal, por el momento debería mostrar solo el tipo de Proyecto.
                    vm.tiposAlertas = respuesta.data.filter(x => x.Name == 'Proyecto');
                }
            }

            function error(respuesta) {
                vm.tiposAlertas = [];
            }
        }

        function listarColumnas() {
            servicioConsolaAlertas.obtenerListaColumnas(peticion).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    vm.columnas = respuesta.data;
                }
            }

            function error(respuesta) {
                vm.columnas = [];
            }
        }

        async function _cargarAlerta(idAlerta) {
            return await servicioConsolaAlertas.obtenerAlertaPorId(peticion, idAlerta).then(data => {
                vm.model = new AlertaMonitoreoConfigModel(data.data[0]);
                (vm.model.AlertasReglasDtos || []).forEach((regla, index) => {
                    cambiarMascara(index, regla.MapColumnasUnoId, regla.MapColumnasUno.TipoColumna);
                })
            });
        }

        async function init() {
            listarTipoAlerta();
            listarColumnas();
            
            if(idAlerta){
                await _cargarAlerta(idAlerta);   
                return;
            } else {
                var regla = new AlertaMonitoreoReglaModel();
                regla.TipoValor = 1;
                vm.model.AlertasReglasDtos.push(regla);
            }
        }

        function obtenerCondicionalesPorTipoColumna(columnaId){
            if(!columnaId)
                return [];

            const tipoColumna = vm.columnas.find(x => x.Id == columnaId).TipoColumna;
            const condicionalesTipoColumna = vm.tipoColumnasCondicionales[tipoColumna];
            return vm.condicionales.filter(x => condicionalesTipoColumna.includes(x.valor));
        }

        vm.init = init;
        vm.adicionarRegla = adicionarRegla;
        vm.removerRegla = removerRegla;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.cambiarMascara = cambiarMascara;
        vm.cambiarTipoColumna = cambiarTipoColumna;
        vm.obtenerCondicionalesPorTipoColumna = obtenerCondicionalesPorTipoColumna;
        vm.listarGrid = listarGrid;

        //#endregion
    }
})();